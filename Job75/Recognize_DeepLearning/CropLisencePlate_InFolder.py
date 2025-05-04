import os
import cv2
import numpy as np
from ultralytics import YOLO

# Load YOLO model for license plate detection
yolo_model = YOLO("./Model/License-plate-detection.pt")

# Directory paths
image_folder = "../Job74/Dataset"
output_dir = r"./Cropped"

# Create output directory if it doesn't exist
os.makedirs(output_dir, exist_ok=True)

def deskew_license_plate(image):
    """Correct skew of the license plate image."""
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

def process_image_for_license_plate(image_path):
    """Detect and crop license plates from images."""
    img = cv2.imread(image_path)
    if img is None:
        print(f"⚠️ Không thể đọc ảnh: {image_path}")
        return
    
    results = yolo_model(image_path, verbose=False)
    
    for i, result in enumerate(results):
        for j, box in enumerate(result.boxes):
            x1, y1, x2, y2 = map(int, box.xyxy[0])
            license_plate = img[y1:y2, x1:x2]
            
            if license_plate.size == 0:
                print(f"⚠️ Không thể cắt vùng biển số {j} trong ảnh {image_path}")
                continue
            
            # Deskew the license plate before saving
            deskewed_plate = deskew_license_plate(license_plate)
            
            file_name = os.path.basename(image_path)
            cropped_path = os.path.join(output_dir, f"{os.path.splitext(file_name)[0]}_plate_{i}_{j}.jpg")
            cv2.imwrite(cropped_path, deskewed_plate)
            print(f"✅ Đã lưu biển số: {cropped_path}")

# Process all images in the folder
image_files = sorted([f for f in os.listdir(image_folder) if f.endswith(('.png', '.jpg'))])
for image_file in image_files:
    image_path = os.path.join(image_folder, image_file)
    process_image_for_license_plate(image_path)
