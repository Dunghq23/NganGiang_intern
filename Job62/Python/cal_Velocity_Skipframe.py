import cv2
import numpy as np
from ultralytics import YOLO

class SpeedDetector:
    def __init__(self, model_path='yolov8n.pt', lane_width_meters=3.5, skip_frames=2):
        """
        Khởi tạo detector
        skip_frames: số frame bỏ qua giữa mỗi lần xử lý
        """
        self.model = YOLO(model_path)
        self.lane_width_meters = lane_width_meters
        self.vehicle_tracking = {}
        self.calibration_done = False
        self.meters_per_pixel = None
        self.next_id = 0
        self.skip_frames = skip_frames
        
    def calculate_speed(self, pos1, pos2, time_diff):
        """
        Tính vận tốc giữa 2 điểm với thời gian đã tính đến skipframe
        """
        if not self.calibration_done:
            raise ValueError("Cần hiệu chuẩn trước khi tính vận tốc!")
        
        if time_diff < 0.001:
            return 0.0
            
        dx = pos2[0] - pos1[0]
        dy = pos2[1] - pos1[1]
        distance_pixels = np.sqrt(dx**2 + dy**2)
        distance_meters = distance_pixels * self.meters_per_pixel
        speed_mps = distance_meters / time_diff
        
        # Giới hạn tốc độ hợp lý (0-200 km/h)
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
        
        # Thời gian thực giữa các frame được xử lý (tính đến skipframe)
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
            
            # Skip frames
            if frame_count % (self.skip_frames + 1) != 0:
                continue
                
            processed_count += 1
            
            # Detect đối tượng
            results = self.model(frame, stream=True)
            current_frame_ids = set()
            
            for result in results:
                boxes = result.boxes
                for box in boxes:
                    if int(box.cls[0]) in [2, 3, 5, 7]:
                        x_center = (box.xyxy[0][0] + box.xyxy[0][2]) / 2
                        y_center = (box.xyxy[0][1] + box.xyxy[0][3]) / 2
                        current_pos = (x_center, y_center)
                        
                        # Tìm ID phù hợp nhất dựa trên khoảng cách
                        min_dist = float('inf')
                        matched_id = None
                        
                        for obj_id, data in self.vehicle_tracking.items():
                            if data['positions'] and processed_count - data['last_update'] < 5:
                                last_pos = data['positions'][-1]
                                dist = np.sqrt((last_pos[0] - x_center)**2 + 
                                             (last_pos[1] - y_center)**2)
                                if dist < min_dist and dist < 100:  # Ngưỡng 100 pixel
                                    min_dist = dist
                                    matched_id = obj_id
                        
                        if matched_id is None:
                            matched_id = self.next_id
                            self.next_id += 1
                            self.vehicle_tracking[matched_id] = {
                                'positions': [],
                                'timestamps': [],
                                'velocities': [],
                                'last_update': processed_count
                            }
                        
                        current_frame_ids.add(matched_id)
                        track = self.vehicle_tracking[matched_id]
                        
                        # Cập nhật tracking
                        track['positions'].append(current_pos)
                        track['timestamps'].append(processed_count * actual_frame_time)
                        track['last_update'] = processed_count
                        
                        # Tính vận tốc
                        speed = 0
                        if len(track['positions']) >= 2:
                            prev_pos = track['positions'][-2]
                            prev_time = track['timestamps'][-2]
                            current_time = track['timestamps'][-1]
                            time_diff = current_time - prev_time
                            
                            speed = self.calculate_speed(prev_pos, current_pos, time_diff)
                            track['velocities'].append(speed)
                            
                            # Lọc nhiễu với trung bình động
                            if len(track['velocities']) > 3:
                                speed = np.mean(track['velocities'][-3:])
                        
                        # Vẽ boundingbox và vận tốc
                        x1, y1, x2, y2 = box.xyxy[0]
                        cv2.rectangle(frame, (int(x1), int(y1)), (int(x2), int(y2)), 
                                    (0, 255, 0), 2)
                        cv2.putText(frame, 
                                  f'ID: {matched_id} Speed: {speed:.1f} km/h', 
                                  (int(x1), int(y1)-10), 
                                  cv2.FONT_HERSHEY_SIMPLEX, 
                                  0.5, 
                                  (0, 255, 0), 
                                  2)
            
            # Xóa tracking cũ
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
        
        return self.vehicle_tracking

def main():
    # Khởi tạo detector với skipframe
    detector = SpeedDetector(
        lane_width_meters=3.5,
        skip_frames=3  # Skip 2 frames, xử lý frame thứ 3
    )
    
    video_path = 'b.mp4'
    tracking_data = detector.process_video(
        video_path=video_path,
        output_path='output_speed.mp4',
        lane_width_pixels=243
    )
    
    print("\nThống kê vận tốc:")
    for obj_id, data in tracking_data.items():
        if len(data['velocities']) > 0:
            velocities = [v for v in data['velocities'] if 0 <= v <= 200]
            if velocities:
                avg_speed = np.mean(velocities)
                max_speed = np.max(velocities)
                print(f"Xe {obj_id}:")
                print(f"  Vận tốc trung bình: {avg_speed:.1f} km/h")
                print(f"  Vận tốc cao nhất: {max_speed:.1f} km/h")

if __name__ == "__main__":
    main()