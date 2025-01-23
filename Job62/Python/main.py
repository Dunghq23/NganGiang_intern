import cv2
import numpy as np
from PIL import Image
from ultralytics import YOLO

def extract_first_frame(video_path):
    """
    Trích xuất khung hình đầu tiên từ video.
    """
    cap = cv2.VideoCapture(video_path)
    if not cap.isOpened():
        print("Không thể mở video.")
        return None

    ret, frame = cap.read()
    if ret:
        frame_path = "first_frame.jpg"
        cv2.imwrite(frame_path, frame)
        cap.release()
        return frame_path
    else:
        print("Không thể đọc khung hình đầu tiên từ video.")
        cap.release()
        return None

def get_image_properties(image_path):
    """
    Lấy thông tin width, height và DPI của ảnh.
    """
    try:
        with Image.open(image_path) as img:
            width, height = img.size
            dpi = img.info.get("dpi", (96, 96))  # Mặc định trả về (96, 96) nếu không có DPI
            return width, height, dpi[0], dpi[1]
    except Exception as e:
        print(f"Không thể đọc thông tin từ ảnh: {e}")
        return None, None, None, None

def calculate_r_from_dpi(width_px, height_px, dpi_x, dpi_y):
    """
    Tính R (m/pixel) từ kích thước pixel và DPI.
    """
    width_inch = width_px / dpi_x
    height_inch = height_px / dpi_y
    width_m = width_inch * 0.0254
    ppm_x = width_px / width_m
    r_x = 1 / ppm_x
    return r_x

def calculate_velocity(video_path):
    # Load model YOLO
    model = YOLO('yolov8n.pt')
    
    # Trích xuất thông tin từ frame đầu tiên
    first_frame_path = extract_first_frame(video_path)
    if not first_frame_path:
        print("Không thể trích xuất frame đầu tiên.")
        return
    
    width_px, height_px, dpi_x, dpi_y = get_image_properties(first_frame_path)
    if not width_px or not dpi_x:
        print("Không thể lấy thông tin ảnh.")
        return
    
    # Tính R từ DPI
    R = calculate_r_from_dpi(width_px, height_px, dpi_x, dpi_y) 
    print(f"R (m/pixel): {R:.6f}")
    
    # Mở video
    cap = cv2.VideoCapture(video_path)
    
    # Chuẩn bị video writer
    fourcc = cv2.VideoWriter_fourcc(*'mp4v')
    out = cv2.VideoWriter('output_video.mp4', fourcc, 30.0, (640, 480))
    
    # Từ điển lưu trữ vị trí và vận tốc của từng phương tiện
    vehicle_tracking = {}
    
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
                if int(box.cls[0]) in [2, 3, 5, 7]:  # Chỉ xử lý xe
                    obj_id = int(box.id[0]) if box.id is not None else -1
                    x_center = (box.xyxy[0][0] + box.xyxy[0][2]) / 2
                    y_center = (box.xyxy[0][1] + box.xyxy[0][3]) / 2
                    center = (x_center, y_center)
                    
                    if obj_id not in vehicle_tracking:
                        vehicle_tracking[obj_id] = {'positions': [], 'velocities': []}
                    
                    vehicle_tracking[obj_id]['positions'].append(center)
                    
                    if len(vehicle_tracking[obj_id]['positions']) > 1:
                        pos1 = vehicle_tracking[obj_id]['positions'][-2]
                        pos2 = vehicle_tracking[obj_id]['positions'][-1]
                        dx = pos2[0] - pos1[0]
                        dy = pos2[1] - pos1[1]
                        velocity_pixel = np.sqrt(dx**2 + dy**2)
                        velocity_mps = velocity_pixel * R
                        velocity_kmh = velocity_mps * 3.6
                        vehicle_tracking[obj_id]['velocities'].append(velocity_kmh)
                        
                        x1, y1, x2, y2 = box.xyxy[0]
                        cv2.rectangle(frame, (int(x1), int(y1)), (int(x2), int(y2)), (0, 255, 0), 2)
                        cv2.putText(frame, f'ID: {obj_id} Speed: {velocity_kmh:.2f} km/h', 
                                    (int(x1), int(y1)-10), 
                                    cv2.FONT_HERSHEY_SIMPLEX, 0.5, (0, 255, 0), 2)
        
        # Ghi frame đã xử lý
        out.write(frame)
        cv2.imshow('Vehicle Speed Tracking', frame)
        if cv2.waitKey(1) & 0xFF == ord('q'):
            break
    
    cap.release()
    out.release()
    cv2.destroyAllWindows()
    return vehicle_tracking

# Sử dụng
video_path = 'b.mp4'
tracking_data = calculate_velocity(video_path)
