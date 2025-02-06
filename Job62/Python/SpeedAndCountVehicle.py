import cv2
import numpy as np
from ultralytics import YOLO
from collections import defaultdict

class SpeedDetector:
    def __init__(self, model_path='yolov8n.pt', lane_width_meters=3.5, skip_frames=2):
        self.model = YOLO(model_path)
        self.lane_width_meters = lane_width_meters
        self.vehicle_tracking = {}
        self.calibration_done = False
        self.meters_per_pixel = None
        self.next_id = 0
        self.skip_frames = skip_frames
        
        # Danh sách các loại phương tiện luôn được hiển thị
        self.vehicle_classes = {
            2: 'Car',
            3: 'Motorcycle',
            5: 'Bus', 
            7: 'Truck'
        }
        
        # Khởi tạo từ điển với các loại phương tiện luôn được đặt về 0
        self.frame_vehicle_stats = defaultdict(lambda: {
            'total_vehicles': 0,
            'vehicle_counts': {cls: 0 for cls in self.vehicle_classes.values()},
            'speeds': []
        })

    def calculate_speed(self, pos1, pos2, time_diff):
        if not self.calibration_done:
            raise ValueError("Cần hiệu chuẩn trước khi tính vận tốc!")
        
        if time_diff < 0.001:
            return 0.0
            
        dx = pos2[0] - pos1[0]
        dy = pos2[1] - pos1[1]
        distance_pixels = np.sqrt(dx**2 + dy**2)
        distance_meters = distance_pixels * self.meters_per_pixel
        speed_mps = distance_meters / time_diff
        
        speed_kmh = min(max(speed_mps * 3.6, 0), 200)
        return speed_kmh

    def process_video(self, video_path, output_path='output_video.mp4', lane_width_pixels=100):
        if not self.calibration_done:
            self.meters_per_pixel = self.lane_width_meters / lane_width_pixels
            self.calibration_done = True
        
        cap = cv2.VideoCapture(video_path)
        if not cap.isOpened():
            raise ValueError("Không thể mở video!")
            
        fps = cap.get(cv2.CAP_PROP_FPS)
        frame_time = 1/fps if fps > 0 else 0.033
        width = int(cap.get(cv2.CAP_PROP_FRAME_WIDTH))
        height = int(cap.get(cv2.CAP_PROP_FRAME_HEIGHT))
        
        actual_frame_time = frame_time * (self.skip_frames + 1)
        
        fourcc = cv2.VideoWriter_fourcc(*'mp4v')
        out = cv2.VideoWriter(output_path, fourcc, fps/(self.skip_frames + 1), (width, height))
        
        frame_count = 0
        processed_count = 0
        
        while cap.isOpened():
            ret, frame = cap.read()
            if not ret:
                break
                
            frame_count += 1
            
            if frame_count % (self.skip_frames + 1) != 0:
                continue
                
            processed_count += 1
            
            # Đặt lại về 0 cho tất cả các loại phương tiện
            current_frame_stats = self.frame_vehicle_stats[processed_count]
            current_frame_stats['total_vehicles'] = 0
            for cls in self.vehicle_classes.values():
                current_frame_stats['vehicle_counts'][cls] = 0
            current_frame_stats['speeds'].clear()
            
            results = self.model(frame, stream=True)
            current_frame_ids = set()
            
            for result in results:
                boxes = result.boxes
                for box in boxes:
                    if int(box.cls[0]) in self.vehicle_classes:
                        vehicle_class = self.vehicle_classes[int(box.cls[0])]
                        x_center = (box.xyxy[0][0] + box.xyxy[0][2]) / 2
                        y_center = (box.xyxy[0][1] + box.xyxy[0][3]) / 2
                        current_pos = (x_center, y_center)
                        
                        current_frame_stats['total_vehicles'] += 1
                        current_frame_stats['vehicle_counts'][vehicle_class] += 1
                        
                        min_dist = float('inf')
                        matched_id = None
                        
                        for obj_id, data in self.vehicle_tracking.items():
                            if data['positions'] and processed_count - data['last_update'] < 5:
                                last_pos = data['positions'][-1]
                                dist = np.sqrt((last_pos[0] - x_center)**2 + 
                                             (last_pos[1] - y_center)**2)
                                if dist < min_dist and dist < 100:
                                    min_dist = dist
                                    matched_id = obj_id
                        
                        if matched_id is None:
                            matched_id = self.next_id
                            self.next_id += 1
                            self.vehicle_tracking[matched_id] = {
                                'type': vehicle_class,
                                'positions': [],
                                'timestamps': [],
                                'velocities': [],
                                'last_update': processed_count
                            }
                        
                        current_frame_ids.add(matched_id)
                        track = self.vehicle_tracking[matched_id]
                        
                        track['positions'].append(current_pos)
                        track['timestamps'].append(processed_count * actual_frame_time)
                        track['last_update'] = processed_count
                        
                        speed = 0
                        if len(track['positions']) >= 2:
                            prev_pos = track['positions'][-2]
                            prev_time = track['timestamps'][-2]
                            current_time = track['timestamps'][-1]
                            time_diff = current_time - prev_time
                            
                            speed = self.calculate_speed(prev_pos, current_pos, time_diff)
                            track['velocities'].append(speed)
                            current_frame_stats['speeds'].append(speed)
                            
                            if len(track['velocities']) > 3:
                                speed = np.mean(track['velocities'][-3:])
                        
                        x1, y1, x2, y2 = box.xyxy[0]
                        cv2.rectangle(frame, (int(x1), int(y1)), (int(x2), int(y2)), 
                                    (0, 255, 0), 2)
                        cv2.putText(frame, 
                                  f'{vehicle_class} ID:{matched_id} Speed:{speed:.1f} km/h', 
                                  (int(x1), int(y1)-10), 
                                  cv2.FONT_HERSHEY_SIMPLEX, 
                                  0.5, 
                                  (0, 255, 0), 
                                  2)
            
            # Hiển thị thống kê phương tiện trên frame
            y_offset = 30
            cv2.putText(frame, f"Frame {processed_count}", 
                       (10, y_offset), 
                       cv2.FONT_HERSHEY_SIMPLEX, 0.7, (0, 0, 255), 2)
            
            y_offset += 30
            cv2.putText(frame, f"Total Vehicles: {current_frame_stats['total_vehicles']}", 
                       (10, y_offset), 
                       cv2.FONT_HERSHEY_SIMPLEX, 0.6, (255, 0, 0), 2)
            
            y_offset += 25
            for vehicle_type, count in current_frame_stats['vehicle_counts'].items():
                cv2.putText(frame, f"{vehicle_type}: {count}", 
                           (10, y_offset), 
                           cv2.FONT_HERSHEY_SIMPLEX, 0.5, (0, 255, 0), 2)
                y_offset += 25
            
            for obj_id in list(self.vehicle_tracking.keys()):
                if (obj_id not in current_frame_ids and 
                    processed_count - self.vehicle_tracking[obj_id]['last_update'] > 5):
                    del self.vehicle_tracking[obj_id]
            
            out.write(frame)
            cv2.imshow('Vehicle Speed Detection', frame)
            if cv2.waitKey(1) & 0xFF == ord('q'):
                break
                
        cap.release()
        out.release()
        cv2.destroyAllWindows()
        
        return self.vehicle_tracking, self.frame_vehicle_stats

def main():
    detector = SpeedDetector(
        lane_width_meters=3.5,
        skip_frames=3
    )
    
    video_path = 'b.mp4'
    detector.process_video(
        video_path=video_path,
        output_path='output_speed.mp4',
        lane_width_pixels=243
    )

if __name__ == "__main__":
    main()