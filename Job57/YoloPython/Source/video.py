from ultralytics import YOLO
import cv2

# Tải mô hình YOLO
model = YOLO('./Model/yolov8x.pt')

# Mở video
video_path = './Video/vehicle-counting.mp4'  # Hoặc 0 để bật webcam
cap = cv2.VideoCapture(video_path)

# Kiểm tra xem video có mở thành công không
if not cap.isOpened():
    print("Không thể mở video")
    exit()

# Lấy kích thước và fps của video đầu vào
frame_width = int(cap.get(cv2.CAP_PROP_FRAME_WIDTH))
frame_height = int(cap.get(cv2.CAP_PROP_FRAME_HEIGHT))
fps = int(cap.get(cv2.CAP_PROP_FPS))

# Thiết lập kích thước mới
new_width = 640  # Chiều rộng mới
new_height = 480  # Chiều cao mới

# Định dạng và tạo VideoWriter để ghi video với kích thước mới
output_path = 'output.mp4'
fourcc = cv2.VideoWriter_fourcc(*'mp4v')  # Định dạng video: mp4
out = cv2.VideoWriter(output_path, fourcc, fps, (new_width, new_height))

# Vòng lặp xử lý từng khung hình
while cap.isOpened():
    ret, frame = cap.read()
    if not ret:
        break  # Nếu không đọc được khung hình thì dừng lại

    # Thay đổi kích thước khung hình
    resized_frame = cv2.resize(frame, (new_width, new_height))

    # Sử dụng mô hình YOLO để phát hiện đối tượng trong khung hình
    results = model(resized_frame)
    
    # Vẽ bounding box lên khung hình kết quả
    for result in results:
        annotated_frame = result.plot()  # Vẽ kết quả lên khung hình

    # Ghi khung hình đã thay đổi kích thước vào video output
    out.write(annotated_frame)

# Giải phóng tài nguyên
cap.release()
out.release()
