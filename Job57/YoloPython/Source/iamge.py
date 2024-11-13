from collections import Counter
from ultralytics import YOLO
import cv2
import time

# Tải mô hình YOLO nhỏ hơn để tăng tốc độ xử lý
model = YOLO('./Model/yolov8n.pt')

# Đường dẫn tới ảnh đầu vào
image_path = r'D:\HAQUANGDUNG\NganGiang_intern\Job57\YoloCsharp\assets\images\vlxx.jpg'
output_path = './Output/vehicle_image_detected.jpg'

# Đọc ảnh
image = cv2.imread(image_path)

# Kiểm tra xem ảnh có được mở thành công không
if image is None:
    print("Không thể mở ảnh")
    exit()

# Thiết lập kích thước mới cho ảnh
new_width = 640
new_height = 480
resized_image = cv2.resize(image, (new_width, new_height))

# Bắt đầu đo thời gian
start_time = time.time()

# Sử dụng mô hình YOLO để phát hiện đối tượng trong ảnh
results = model(resized_image, verbose=False)

# Xử lý kết quả đầu tiên từ danh sách `results`
result = results[0]  # Lấy phần tử đầu tiên trong danh sách

# Đếm từng loại phương tiện
vehicle_counts = Counter()
for box in result.boxes:
    class_id = int(box.cls)  # Lấy id của lớp
    class_name = result.names[class_id]  # Lấy tên lớp dựa trên id
    vehicle_counts[class_name] += 1  # Cập nhật số lượng

# Vẽ bounding box lên ảnh kết quả
annotated_image = result.plot()

# Hiển thị tổng số phương tiện trên ảnh
cv2.putText(annotated_image, f'Total Vehicles: {sum(vehicle_counts.values())}', (10, 40),
            cv2.FONT_HERSHEY_SIMPLEX, 1, (0, 0, 255), 2)

# Lưu ảnh đầu ra
cv2.imwrite(output_path, annotated_image)

# Tính toán và in tổng thời gian xử lý
end_time = time.time()
total_time = end_time - start_time
print(f"Tổng thời gian xử lý: {total_time:.2f} giây")

# In ra số lượng từng loại phương tiện
print("Số lượng từng loại phương tiện:")
for vehicle, count in vehicle_counts.items():
    print(f"{vehicle}: {count}")


# Ghi kết quả vào file .txt
with open(r"D:\HAQUANGDUNG\NganGiang_intern\Job57\VehicleDetection\resources\Text\OutputDetection\vehicle_counts.txt", 'w', encoding='utf-8') as file:
    file.write(f"Tổng thời gian xử lý: {total_time:.2f} giây\n")
    file.write("Số lượng từng loại phương tiện:\n")
    for vehicle, count in vehicle_counts.items():
        file.write(f"{vehicle}: {count}\n")
    file.write(f"Tổng số phương tiện: {sum(vehicle_counts.values())}\n")