import time
from ultralytics import YOLO
import cv2
import numpy as np
import matplotlib.pyplot as plt

# Đường dẫn ảnh
original_image_path = r"D:\Documents\Work\NganGiang\HAQUANGDUNG\Job69\Code\image\img7.jpg"
processed_image_path = r"D:\Documents\Work\NganGiang\HAQUANGDUNG\Job69\Code\image\img7_mask.jpg"

# Bắt đầu đo thời gian tổng
start_total = time.time()

# 1. CHUYỂN ẢNH LÒNG ĐƯỜNG THÀNH ẢNH NHỊ PHÂN
start_processing = time.time()
processed_image = cv2.imread(processed_image_path)
gray_road_mask = cv2.cvtColor(processed_image, cv2.COLOR_BGR2GRAY)
_, road_mask = cv2.threshold(gray_road_mask, 200, 255, cv2.THRESH_BINARY)
road_area = np.count_nonzero(road_mask == 255)
non_road_area = np.count_nonzero(road_mask == 0)
end_processing = time.time()

# 2. TÍNH DIỆN TÍCH CÁC PHƯƠNG TIỆN
start_detection = time.time()
model = YOLO("yolov8s-seg.pt")
original_image = cv2.imread(original_image_path)
original_image = cv2.cvtColor(original_image, cv2.COLOR_BGR2RGB)
results = model(original_image)
end_detection = time.time()

names = model.names
vehicle_list = ["car", "truck", "motorcycle", "bycicle", "bus", "person"]
sum_vehicle_area = 0
num_vehicles = 0

for i in range(len(results[0].masks)):
    class_id = int(results[0].boxes.cls[i])
    if names[class_id] in vehicle_list:
        mask = results[0].masks.data[i]
        vehicle_area = np.count_nonzero(mask)
        sum_vehicle_area += vehicle_area
        num_vehicles += 1


# 3. TÍNH % DIỆN TÍCH PHƯƠNG TIỆN CHIẾM TRÊN LÒNG ĐƯỜNG
if road_area > 0:
    percentage_occupancy = (sum_vehicle_area / road_area) * 100
else:
    percentage_occupancy = 0

# 4. VẼ BIỂU ĐỒ
start_plotting = time.time()
fig, axs = plt.subplots(2, 3, figsize=(16, 8))

axs[0, 0].imshow(original_image)
axs[0, 0].set_title("Ảnh Gốc")
axs[0, 0].axis("off")

axs[0, 1].imshow(road_mask, cmap="gray")
axs[0, 1].set_title("Lòng Đường Sau Xử Lý")
axs[0, 1].axis("off")

axs[0, 2].imshow(results[0].masks.data.sum(axis=0), cmap="gray")
axs[0, 2].set_title("Phương Tiện Chiếm Dụng")
axs[0, 2].axis("off")

labels = ["Lòng đường (pixel)", "Phương tiện (pixel)", "Số xe"]
values = [road_area, sum_vehicle_area, num_vehicles]
axs[1, 0].bar(labels, values, color=['blue', 'red', 'green'])
for i, v in enumerate(values):
    axs[1, 0].text(i, v + max(values) * 0.02, f"{v:.2f}", ha='center', fontsize=12, fontweight='bold')
axs[1, 0].set_title("Thống kê diện tích và số lượng xe")

sizes = [sum_vehicle_area, road_area - sum_vehicle_area]
labels = [f"Phương tiện ({percentage_occupancy:.2f}%)", "Phần còn lại"]
colors = ["red", "blue"]
axs[1, 1].pie(sizes, labels=labels, autopct="%1.1f%%", colors=colors, startangle=140)
axs[1, 1].set_title("Tỷ lệ phương tiện chiếm trên lòng đường")

axs[1, 2].axis("off")
table_data = [
    ["Tổng diện tích lòng đường", f"{road_area} pixel"],
    ["Tổng số xe phát hiện", f"{num_vehicles}"],
    ["Diện tích phương tiện", f"{sum_vehicle_area} pixel"],
    ["Tỷ lệ chiếm dụng", f"{percentage_occupancy:.2f}%"]
]
axs[1, 2].table(cellText=table_data, colLabels=["Thông số", "Giá trị"], cellLoc="center", loc="center")
axs[1, 2].set_title("Bảng Thống Kê")

plt.tight_layout()
plt.show()
end_plotting = time.time()

# Kết thúc đo thời gian tổng
end_total = time.time()

# In thời gian thực thi
processing_time = end_processing - start_processing
detection_time = end_detection - start_detection
plotting_time = end_plotting - start_plotting
total_time = end_total - start_total

print(f"⏳ Thời gian xử lý ảnh: {processing_time:.4f} giây")
print(f"🚗 Thời gian nhận diện phương tiện: {detection_time:.4f} giây")
print(f"📊 Thời gian vẽ biểu đồ: {plotting_time:.4f} giây")
print(f"⏱️ Tổng thời gian chạy chương trình: {total_time:.4f} giây")
