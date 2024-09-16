from ultralytics import YOLO
import cv2
import time

# Tải mô hình YOLO nhỏ hơn để tăng tốc độ xử lý
model = YOLO('./Model/yolov8n.pt')  # Sử dụng phiên bản nhỏ hơn của YOLO để xử lý nhanh hơn

# Mở video
video_path = './Video/vehicle-counting.mp4'
cap = cv2.VideoCapture(video_path)

# Kiểm tra xem video có mở thành công không
if not cap.isOpened():
    print("Không thể mở video")
    exit()

# Lấy kích thước và fps của video đầu vào
frame_width = int(cap.get(cv2.CAP_PROP_FRAME_WIDTH))
frame_height = int(cap.get(cv2.CAP_PROP_FRAME_HEIGHT))
fps = int(cap.get(cv2.CAP_PROP_FPS))

# Thiết lập kích thước mới nhỏ hơn
new_width = 640  # Chiều rộng mới
new_height = 480  # Chiều cao mới

# Định dạng và tạo VideoWriter để ghi video với kích thước mới
output_path = './Output/vehicle-counting_detected.mp4'
fourcc = cv2.VideoWriter_fourcc(*'mp4v')
out = cv2.VideoWriter(output_path, fourcc, fps, (new_width, new_height))

# Bắt đầu đo thời gian
start_time = time.time()

while cap.isOpened():
    ret, frame = cap.read()
    if not ret:
        break

    # Thay đổi kích thước khung hình
    resized_frame = cv2.resize(frame, (new_width, new_height))

    # Sử dụng mô hình YOLO để phát hiện đối tượng trong khung hình
    results = model(resized_frame)

    # Vẽ bounding box lên khung hình kết quả
    for result in results:
        annotated_frame = result.plot()  # Vẽ kết quả lên khung hình

    # Vẽ đường kẻ ngang 1/3 màn hình từ dưới lên
    line_y_position = int(new_height * 2 / 3)  # Tính vị trí đường kẻ (2/3 từ trên xuống)
    cv2.line(annotated_frame, (0, line_y_position), (new_width, line_y_position), (0, 255, 0), 2)

    # Ghi khung hình vào video output
    out.write(annotated_frame)

# Giải phóng tài nguyên
cap.release()
out.release()

# Tính toán và in tổng thời gian xử lý
end_time = time.time()
total_time = end_time - start_time
print(f"Tổng thời gian xử lý: {total_time:.2f} giây")
