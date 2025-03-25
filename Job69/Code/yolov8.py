import cv2
import numpy as np
import matplotlib.pyplot as plt
import ultralytics
from IPython.display import Image
from ultralytics import YOLO

# read image
img = cv2.imread("image\img7.jpg")
img_mask2 = cv2.imread("image\img7_mask.jpg")
plt.imshow(img)
plt.imshow(img_mask2)

# phép bitwise and
# xóa phần ảnh giống nhau
img_mask = cv2.bitwise_and(img, img_mask2)
plt.imshow(img_mask)

# tìm contours của img_mask
temp = img_mask.copy()
print(temp.shape)
gray = cv2.cvtColor(temp, cv2.COLOR_BGR2GRAY)
contours, hierarchy = cv2.findContours(gray, cv2.RETR_TREE, cv2.CHAIN_APPROX_SIMPLE)
# vẽ contours và tính diện tích contours lớn nhất
area = 0
for c in contours:
    cv2.drawContours(temp, [c], -1, (0, 255, 0), 2)
    area = max(cv2.contourArea(c), area)
# in kết quả
print(f"Diện tích vùng pixel lớn nhất: {area} pixel")
plt.imshow(temp)

model = YOLO('yolov8s-seg.pt')  # set model
results = model.predict(source=img_mask, save=True, show=True, boxes = False, conf = 0.15)  # predict() returns a named tuple
# gọi danh sách các lớp đối tượng trong model
names = model.names
print(names)
list = ["car", "truck", "motorcycle", "bycicle", "bus", "person"]
Sum_area = 0
num_car = 0

# duyệt lần lượt các đối tượng nhận diện được
for i in range(len(results[0].masks)):
    # Nếu dối tượng là các xe thuộc 'list':
    if names[int(results[0].boxes.cls[i])] in list:
         # tính tổng diện tich của xe
        Sum_area += np.count_nonzero(results[0].masks.data[i])
         # đếm tổng số xe
        num_car += 1

print("total: {} pixel".format(Sum_area))
print("Số xe chiếm: {} % lòng đường".format(np.round(Sum_area/area*100, 2)))
print('Số lượng xe: {}'.format(num_car))
Image(f'{results[0].save_dir}\image0.jpg')