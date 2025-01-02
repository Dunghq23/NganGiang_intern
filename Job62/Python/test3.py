import cv2
import numpy as np
from ultralytics import YOLO

def calculate_velocity(video_path):
    # Load model YOLO
    model = YOLO('yolov8n.pt')
    
    # Mở video
    cap = cv2.VideoCapture(video_path)
    
    # Chuẩn bị video writer
    fourcc = cv2.VideoWriter_fourcc(*'mp4v')
    out = cv2.VideoWriter('output_video.mp4', fourcc, 30.0, (640, 480))
    
    # Từ điển lưu trữ vị trí và vận tốc của từng phương tiện
    vehicle_tracking = {}
    
    frame_count = 0
    
    while cap.isOpened():
        ret, frame = cap.read()
        if not ret:
            break
        
        # Resize frame
        frame = cv2.resize(frame, (720, 560), interpolation=cv2.INTER_AREA)
        
        # Detect đối tượng
        results = model(frame, stream=True)
        
        # Xử lý từng đối tượng được detect
        for result in results:
            boxes = result.boxes
            for box in boxes:
                # Chỉ xử lý xe (class 2, 3, 5, 7 trong COCO dataset)
                if int(box.cls[0]) in [2, 3, 5, 7]:
                    # Lấy ID của đối tượng (nếu có)
                    obj_id = int(box.id[0]) if box.id is not None else -1
                    
                    # Lấy tọa độ trung tâm
                    x_center = (box.xyxy[0][0] + box.xyxy[0][2]) / 2
                    y_center = (box.xyxy[0][1] + box.xyxy[0][3]) / 2
                    center = (x_center, y_center)
                    
                    # Khởi tạo tracking cho đối tượng nếu chưa tồn tại
                    if obj_id not in vehicle_tracking:
                        vehicle_tracking[obj_id] = {
                            'positions': [],
                            'velocities': []
                        }
                    
                    # Lưu vị trí hiện tại
                    vehicle_tracking[obj_id]['positions'].append(center)
                    
                    # Tính vận tốc nếu có ít nhất 2 frame
                    if len(vehicle_tracking[obj_id]['positions']) > 1:
                        # Lấy 2 vị trí gần nhất
                        pos1 = vehicle_tracking[obj_id]['positions'][-2]
                        pos2 = vehicle_tracking[obj_id]['positions'][-1]
                        
                        # Tính vận tốc
                        dx = pos2[0] - pos1[0]
                        dy = pos2[1] - pos1[1]
                        
                        # Tính khoảng cách pixel
                        velocity_pixel = np.sqrt(dx**2 + dy**2)
                        
                        # Chuyển đổi sang km/h (giả sử)
                        pixel_per_meter = 50  # Cần điều chỉnh
                        velocity_mps = velocity_pixel / pixel_per_meter  # m/s
                        velocity_kmh = velocity_mps * 3.6
                        
                        vehicle_tracking[obj_id]['velocities'].append(velocity_kmh)
                        
                        # Vẽ bounding box và vận tốc
                        x1, y1, x2, y2 = box.xyxy[0]
                        cv2.rectangle(frame, (int(x1), int(y1)), (int(x2), int(y2)), (0, 255, 0), 2)
                        cv2.putText(frame, f'ID: {obj_id} Speed: {velocity_kmh:.2f} km/h', 
                                    (int(x1), int(y1)-10), 
                                    cv2.FONT_HERSHEY_SIMPLEX, 0.5, (0, 255, 0), 2)
        
        # Ghi frame đã xử lý
        out.write(frame)
        
        # Hiển thị (tùy chọn)
        cv2.imshow('Vehicle Speed Tracking', frame)
        
        # Thoát nếu nhấn 'q'
        if cv2.waitKey(1) & 0xFF == ord('q'):
            break
        
        frame_count += 1
    
    cap.release()
    out.release()
    cv2.destroyAllWindows()
    
    return vehicle_tracking

# Sử dụng
video_path = 'b.mp4'
tracking_data = calculate_velocity(video_path)