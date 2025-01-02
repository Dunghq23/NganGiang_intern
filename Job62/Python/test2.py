import cv2
import numpy as np
from ultralytics import YOLO

def calculate_velocity(video_path):
    # Load model YOLO
    model = YOLO('yolov8n.pt')
    
    # Mở video
    cap = cv2.VideoCapture(video_path)
    
    # Lấy thông số video
    fps = cap.get(cv2.CAP_PROP_FPS)
    width = int(cap.get(cv2.CAP_PROP_FRAME_WIDTH))
    height = int(cap.get(cv2.CAP_PROP_FRAME_HEIGHT))
    
    # Biến lưu trữ vị trí và thông tin tracking
    prev_centers = {}
    velocities = {}
    
    frame_count = 0
    
    while cap.isOpened():
        ret, frame = cap.read()
        if not ret:
            break
        
        # Detect đối tượng
        results = model(frame)
        
        # Xử lý từng đối tượng được detect
        for result in results:
            for box in result.boxes:
                # Chỉ xử lý xe (class 2, 3, 5, 7 trong COCO dataset)
                if int(box.cls[0]) in [2, 3, 5, 7]:
                    # Lấy tọa độ trung tâm
                    x_center = (box.xyxy[0][0] + box.xyxy[0][2]) / 2
                    y_center = (box.xyxy[0][1] + box.xyxy[0][3]) / 2
                    center = (x_center, y_center)
                    
                    # Nếu có frame trước đó
                    if frame_count > 0:
                        # Tính vận tốc
                        dx = center[0] - prev_centers.get(frame_count-1, center)[0]
                        dy = center[1] - prev_centers.get(frame_count-1, center)[1]
                        
                        # Tính vận tốc pixel/frame
                        velocity = np.sqrt(dx**2 + dy**2)
                        
                        # Chuyển đổi sang km/h (giả sử)
                        # Cần hiệu chỉnh pixel_per_meter dựa trên video cụ thể
                        pixel_per_meter = 50  # Giá trị này cần điều chỉnh
                        velocity_mps = velocity / pixel_per_meter  # m/s
                        velocity_kmh = velocity_mps * 3.6
                        
                        velocities[frame_count] = velocity_kmh
                    
                    # Lưu vị trí center
                    prev_centers[frame_count] = center
        
        frame_count += 1
        
        # Hiển thị (tùy chọn)
        if len(velocities) > 0:
            print(f"Frame {frame_count}: Vận tốc = {velocities.get(frame_count-1, 0):.2f} km/h")
    
    cap.release()
    return velocities

# Sử dụng
video_path = 'a.mp4'
velocities = calculate_velocity(video_path)
print(velocities)