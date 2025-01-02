import cv2
from ultralytics import YOLO

# Load model
model = YOLO('yolov8n.pt')

# Đọc video
video_path = 'a.mp4'
cap = cv2.VideoCapture(video_path)

# Kích thước mới (ví dụ: 640x480)
new_width = 640
new_height = 480


while cap.isOpened():
    ret, frame = cap.read()
    if not ret:
        break
     # Resize frame
    frame_resized = cv2.resize(frame, (new_width, new_height), interpolation=cv2.INTER_AREA)
    
    # Detect 
    results = model(frame_resized)
    
    # Hiển thị kết quả detection
    for result in results:
        # In ra thông tin chi tiết
        boxes = result.boxes
        for box in boxes:
            # Tọa độ
            x1, y1, x2, y2 = box.xyxy[0]
            
            # Lớp đối tượng
            cls = int(box.cls[0])
            
            # Độ tin cậy 
            conf = float(box.conf[0])
            
            # Tên lớp
            class_name = result.names[cls]
            
            print(f"Detected: {class_name}, Confidence: {conf:.2f}, Coordinates: ({x1},{y1},{x2},{y2})")
    
    # Vẽ bounding box (tùy chọn)
    annotated_frame = results[0].plot()
    cv2.imshow('Detection', annotated_frame)
    
    # Thoát nếu nhấn 'q'
    if cv2.waitKey(1) & 0xFF == ord('q'):
        break

cap.release()
cv2.destroyAllWindows()