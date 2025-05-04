import cv2
import numpy as np
import os

def deskew_license_plate(image):
    gray = cv2.cvtColor(image, cv2.COLOR_BGR2GRAY)
    blur = cv2.GaussianBlur(gray, (5, 5), 0)
    _, binary = cv2.threshold(blur, 0, 255, cv2.THRESH_BINARY + cv2.THRESH_OTSU)
    contours, _ = cv2.findContours(binary, cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_SIMPLE)

    if contours:
        largest_contour = max(contours, key=cv2.contourArea)
        rect = cv2.minAreaRect(largest_contour)
        angle = rect[2]

        if angle < -45:
            angle += 90
        elif angle > 45:
            angle -= 90

        (h, w) = image.shape[:2]
        center = (w // 2, h // 2)
        M = cv2.getRotationMatrix2D(center, angle, 1.0)
        rotated = cv2.warpAffine(image, M, (w, h), flags=cv2.INTER_CUBIC,
                                 borderMode=cv2.BORDER_REPLICATE)
        return rotated
    return image


input_dir = "./Cropped"
output_dir = "./Cropped_Deskewed"

# Tạo thư mục đầu ra nếu chưa tồn tại
os.makedirs(output_dir, exist_ok=True)

# Lặp qua các file ảnh trong thư mục đầu vào
for filename in os.listdir(input_dir):
    if filename.lower().endswith((".png", ".jpg", ".jpeg")):
        image_path = os.path.join(input_dir, filename)
        image = cv2.imread(image_path)

        if image is not None:
            result = deskew_license_plate(image)
            save_path = os.path.join(output_dir, filename)
            cv2.imwrite(save_path, result)
            print(f"✔ Đã xử lý và lưu: {save_path}")
        else:
            print(f"✘ Không thể đọc ảnh: {image_path}")