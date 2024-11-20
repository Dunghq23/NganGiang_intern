from ultralytics import YOLO
import cv2
import time

# Tải mô hình YOLO nhỏ hơn để tăng tốc độ xử lý
model = YOLO('./Model/yolov8m.pt')  # Sử dụng phiên bản nhỏ hơn của YOLO để xử lý nhanh hơn

# Mở video
video_path = './Video/VideoGT_VN.mp4'
cap = cv2.VideoCapture(video_path)

# Kiểm tra xem video có mở thành công không
if not cap.isOpened():
    with open('./Output/log.txt', 'w') as log_file:
        log_file.write("Không thể mở video\n")
    exit()

# Lấy kích thước và fps của video đầu vào
frame_width = int(cap.get(cv2.CAP_PROP_FRAME_WIDTH))
frame_height = int(cap.get(cv2.CAP_PROP_FRAME_HEIGHT))
fps = int(cap.get(cv2.CAP_PROP_FPS))

# Thiết lập kích thước mới lớn hơn
new_width = int(frame_width * 1.5)
new_height = int(frame_height * 1.5)

# Định dạng và tạo VideoWriter để ghi video với kích thước mới
output_path = './Output/VideoGT_VN.mp4'
fourcc = cv2.VideoWriter_fourcc(*'mp4v')
out = cv2.VideoWriter(output_path, fourcc, fps, (new_width, new_height))

# Bắt đầu đo thời gian
start_time = time.time()

# Biến đếm frame
frame_count = 0
process_every_n_frames = 30  # Chỉ xử lý mỗi 30 frame

display_scale = 0.2  # Hiển thị khung hình nhỏ hơn 50%

# Mở file log để ghi
log_path = './Output/log.txt'
with open(log_path, 'w', encoding='utf-8') as log_file:
    log_file.write("Bắt đầu xử lý video...\n")
    log_file.write(f"Video gốc: {video_path}\n")
    log_file.write(f"Kích thước gốc: {frame_width}x{frame_height}, FPS: {fps}\n")
    log_file.write(f"Kích thước mới: {new_width}x{new_height}\n")
    log_file.write(f"Video đầu ra: {output_path}\n")

    while cap.isOpened():
        ret, frame = cap.read()
        if not ret:
            break

        # Tăng bộ đếm frame
        frame_count += 1

        # Chỉ xử lý frame nếu nó là frame thứ process_every_n_frames
        if frame_count % process_every_n_frames != 0:
            continue

        # Ghi log cho từng frame
        start_frame_time = time.time()
        
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

        # Hiển thị khung hình
        # Thay đổi kích thước hiển thị nhỏ hơn
        display_frame = cv2.resize(annotated_frame, (0, 0), fx=display_scale, fy=display_scale)

        # Hiển thị khung hình đã thu nhỏ
        cv2.imshow('Video Output', display_frame)

        # Ghi log kết quả của frame
        end_frame_time = time.time()
        detection_count = len(results[0])  # Số lượng đối tượng phát hiện
        frame_time = end_frame_time - start_frame_time
        log_file.write(f"Frame {frame_count} xử lý xong. Thời gian: {frame_time:.2f}s. "
                       f"Phát hiện {detection_count} đối tượng.\n")

        # Nhấn 'q' để dừng xem video
        if cv2.waitKey(1) & 0xFF == ord('q'):
            log_file.write("Dừng video theo yêu cầu người dùng.\n")
            break

    # Giải phóng tài nguyên
    cap.release()
    out.release()
    cv2.destroyAllWindows()

    # Tính toán và in tổng thời gian xử lý
    end_time = time.time()
    total_time = end_time - start_time
    log_file.write(f"Tổng thời gian xử lý: {total_time:.2f} giây\n")
    log_file.write("Hoàn thành xử lý video.\n")
