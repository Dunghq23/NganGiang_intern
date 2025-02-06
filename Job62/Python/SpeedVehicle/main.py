import cv2
import numpy as np
from ultralytics import YOLO
from collections import defaultdict

class SpeedDetector:
    def __init__(self, model_path='yolov8n.pt', lane_width_meters=3.5, skip_frames=2):
        # Khởi tạo mô hình YOLO để nhận diện đối tượng
        self.model = YOLO(model_path)
        
        # Chiều rộng làn đường chuẩn (mặc định 3.5 mét)
        self.lane_width_meters = lane_width_meters
        
        # Từ điển để theo dõi các phương tiện
        self.vehicle_tracking = {}
        
        # Cờ kiểm tra đã hiệu chuẩn chưa
        self.calibration_done = False
        
        # Số pixel trên một mét
        self.meters_per_pixel = None
        
        # ID tiếp theo cho phương tiện mới
        self.next_id = 0
        
        # Số khung hình bỏ qua giữa các lần xử lý
        self.skip_frames = skip_frames
        
        # Từ điển ánh xạ các lớp phương tiện được hỗ trợ
        self.vehicle_classes = {
            2: 'Car',
            3: 'Motorcycle',
            5: 'Bus', 
            7: 'Truck'
        }
        
        # Khởi tạo từ điển thống kê phương tiện cho mỗi khung hình
        self.frame_vehicle_stats = defaultdict(lambda: {
            'total_vehicles': 0,
            'vehicle_counts': {cls: 0 for cls in self.vehicle_classes.values()},
            'speeds': []
        })

    def calculate_speed(self, pos1, pos2, time_diff):
        # Kiểm tra đã hiệu chuẩn chưa
        if not self.calibration_done:
            raise ValueError("Cần hiệu chuẩn trước khi tính vận tốc!")
        
        # Tránh chia cho số 0
        if time_diff < 0.001:
            return 0.0
            
        # Tính khoảng cách giữa hai điểm
        dx = pos2[0] - pos1[0]
        dy = pos2[1] - pos1[1]
        distance_pixels = np.sqrt(dx**2 + dy**2)
        
        # Chuyển đổi khoảng cách từ pixel sang mét
        distance_meters = distance_pixels * self.meters_per_pixel
        
        # Tính vận tốc (mét/giây)
        speed_mps = distance_meters / time_diff
        
        # Chuyển đổi sang km/h và giới hạn vận tốc
        speed_kmh = min(max(speed_mps * 3.6, 0), 200)
        return speed_kmh

    def process_video(self, video_path, output_path='output_video.mp4', lane_width_pixels=100):
        # Hiệu chuẩn số pixel trên mét nếu chưa thực hiện
        if not self.calibration_done:
            self.meters_per_pixel = self.lane_width_meters / lane_width_pixels
            self.calibration_done = True
        
        # Mở video
        cap = cv2.VideoCapture(video_path)
        if not cap.isOpened():
            raise ValueError("Không thể mở video!")
            
        # Lấy thông số video
        fps = cap.get(cv2.CAP_PROP_FPS)
        frame_time = 1/fps if fps > 0 else 0.033
        width = int(cap.get(cv2.CAP_PROP_FRAME_WIDTH))
        height = int(cap.get(cv2.CAP_PROP_FRAME_HEIGHT))
        
        # Tính thời gian thực của khung hình
        actual_frame_time = frame_time * (self.skip_frames + 1)
        
        # Thiết lập ghi video
        fourcc = cv2.VideoWriter_fourcc(*'mp4v')
        out = cv2.VideoWriter(output_path, fourcc, fps/(self.skip_frames + 1), (width, height))
        
        # Khởi tạo bộ đếm khung hình
        frame_count = 0
        processed_count = 0
        
        # Vòng lặp xử lý video
        while cap.isOpened():
            ret, frame = cap.read()
            if not ret:
                break
                
            frame_count += 1
            
            # Bỏ qua các khung hình không cần thiết
            if frame_count % (self.skip_frames + 1) != 0:
                continue
                
            processed_count += 1
            
            # Đặt lại thống kê cho khung hình hiện tại
            current_frame_stats = self.frame_vehicle_stats[processed_count]
            current_frame_stats['total_vehicles'] = 0
            for cls in self.vehicle_classes.values():
                current_frame_stats['vehicle_counts'][cls] = 0
            current_frame_stats['speeds'].clear()
            
            # Nhận diện đối tượng
            results = self.model(frame, stream=True)
            current_frame_ids = set()
            
            # Xử lý từng đối tượng được nhận diện
            for result in results:
                boxes = result.boxes
                for box in boxes:
                    # Kiểm tra nếu là loại phương tiện được hỗ trợ
                    if int(box.cls[0]) in self.vehicle_classes:
                        vehicle_class = self.vehicle_classes[int(box.cls[0])]
                        
                        # Tính tọa độ trung tâm của phương tiện
                        x_center = (box.xyxy[0][0] + box.xyxy[0][2]) / 2
                        y_center = (box.xyxy[0][1] + box.xyxy[0][3]) / 2
                        current_pos = (x_center, y_center)
                        
                        # Cập nhật thống kê
                        current_frame_stats['total_vehicles'] += 1
                        current_frame_stats['vehicle_counts'][vehicle_class] += 1
                        
                        # Tìm ID phương tiện phù hợp
                        min_dist = float('inf')
                        matched_id = None
                        
                        # Kiểm tra với các phương tiện đã theo dõi trước đó
                        for obj_id, data in self.vehicle_tracking.items():
                            if data['positions'] and processed_count - data['last_update'] < 5:
                                last_pos = data['positions'][-1]
                                dist = np.sqrt((last_pos[0] - x_center)**2 + 
                                             (last_pos[1] - y_center)**2)
                                if dist < min_dist and dist < 100:
                                    min_dist = dist
                                    matched_id = obj_id
                        
                        # Tạo ID mới nếu chưa tìm thấy
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
                        
                        # Cập nhật thông tin vị trí và thời gian
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
                            current_frame_stats['speeds'].append(speed)
                            
                            # Lấy trung bình vận tốc 3 khung hình gần nhất
                            if len(track['velocities']) > 3:
                                speed = np.mean(track['velocities'][-3:])
                        
                        # Vẽ hình chữ nhật và ghi chú cho phương tiện
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
            
            # Hiển thị thống kê phương tiện trên khung hình
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
            
            # Xóa các phương tiện không còn xuất hiện
            for obj_id in list(self.vehicle_tracking.keys()):
                if (obj_id not in current_frame_ids and 
                    processed_count - self.vehicle_tracking[obj_id]['last_update'] > 5):
                    del self.vehicle_tracking[obj_id]
            
            # Ghi và hiển thị khung hình
            out.write(frame)
            cv2.imshow('Vehicle Speed Detection', frame)
            if cv2.waitKey(1) & 0xFF == ord('q'):
                break
                
        # Giải phóng tài nguyên
        cap.release()
        out.release()
        cv2.destroyAllWindows()
        
        # Trả về thông tin theo dõi phương tiện và thống kê
        return self.vehicle_tracking, self.frame_vehicle_stats

def main():
    # Tạo instance của SpeedDetector
    detector = SpeedDetector(
        lane_width_meters=3.5,  # Chiều rộng làn đường
        skip_frames=3  # Số khung hình bỏ qua giữa các lần xử lý
    )
    
    # Đường dẫn video đầu vào
    video_path = 'b.mp4'
    
    # Xử lý video
    detector.process_video(
        video_path=video_path,
        output_path='output_speed.mp4',
        lane_width_pixels=243  # Chiều rộng làn đường trong ảnh
    )

if __name__ == "__main__":
    main()