import os
import cv2
import numpy as np
from ultralytics import YOLO

# Load YOLO model for license plate detection
yolo_model = YOLO("./License-plate-detection.pt")

# Directory paths
image_folder = "./Dataset"
output_dir = r"./Cropped"

# Create output directory if it doesn't exist
os.makedirs(output_dir, exist_ok=True)

def process_image_for_license_plate(image_path):
    """Detect and crop license plates from images."""
    img = cv2.imread(image_path)
    if img is None:
        print(f"⚠️ Không thể đọc ảnh: {image_path}")
        return
    
    results = yolo_model(image_path)
    
    for i, result in enumerate(results):
        for j, box in enumerate(result.boxes):
            x1, y1, x2, y2 = map(int, box.xyxy[0])
            license_plate = img[y1:y2, x1:x2]
            
            if license_plate.size == 0:
                print(f"⚠️ Không thể cắt vùng biển số {j} trong ảnh {image_path}")
                continue
            
            file_name = os.path.basename(image_path)
            cropped_path = os.path.join(output_dir, f"{os.path.splitext(file_name)[0]}_plate_{i}_{j}.jpg")
            cv2.imwrite(cropped_path, license_plate)
            print(f"✅ Đã lưu biển số: {cropped_path}")

# Process all images in the folder
image_files = sorted([f for f in os.listdir(image_folder) if f.endswith(('.png', '.jpg'))])
for image_file in image_files:
    image_path = os.path.join(image_folder, image_file)
    process_image_for_license_plate(image_path)