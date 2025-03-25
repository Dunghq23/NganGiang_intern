import os
import cv2
import numpy as np
from skimage.filters import threshold_local
import imutils
from skimage import measure
import matplotlib.pyplot as plt
from ultralytics import YOLO

# Load YOLO model for license plate detection
yolo_model = YOLO("./License-plate-detection.pt")

# Directory paths
input_folder = "./output_chars"
output_dir = r"D:\Documents\Work\NganGiang\HAQUANGDUNG\Job74\Cropped"

# Create output directories if they don't exist
os.makedirs(output_dir, exist_ok=True)

def save_characters(sorted_candidates, original_filename, save_dir="output_chars"):
    """Save character images to specified directory"""
    if not os.path.exists(save_dir):
        os.makedirs(save_dir)
    
    file_prefix = os.path.splitext(os.path.basename(original_filename))[0]
    for index, (char, _) in enumerate(sorted_candidates):
        filename = f"{save_dir}/{file_prefix}_char_{index:02d}.png"
        cv2.imwrite(filename, char)
        print(f"Saved {filename}")

def convert_to_square(image):
    """Convert non-square images to square by padding with zeros"""
    img_h, img_w = image.shape[:2]
    
    if img_h > img_w:
        diff = img_h - img_w
        x1 = np.zeros((img_h, diff // 2), dtype=image.dtype)
        x2 = np.zeros((img_h, diff // 2 + diff % 2), dtype=image.dtype)
        return np.concatenate((x1, image, x2), axis=1)
    elif img_w > img_h:
        diff = img_w - img_h
        x1 = np.zeros((diff // 2, img_w), dtype=image.dtype)
        x2 = np.zeros((diff // 2 + diff % 2, img_w), dtype=image.dtype)
        return np.concatenate((x1, image, x2), axis=0)
    return image

def preprocess_license_plate(license_plate):
    """Preprocess license plate image for character segmentation"""
    hsv = cv2.cvtColor(license_plate, cv2.COLOR_BGR2HSV)
    V = cv2.split(hsv)[2]
    T = threshold_local(V, 15, offset=10, method="gaussian")
    return (V > T).astype("uint8") * 255

def extract_and_sort_characters(thresh, license_plate):
    """Extract and sort characters based on connected components analysis"""
    labels = measure.label(thresh, connectivity=2, background=0)
    candidates = []

    for label in np.unique(labels):
        if label == 0:
            continue
        mask = np.zeros(thresh.shape, dtype="uint8")
        mask[labels == label] = 255
        contours, _ = cv2.findContours(mask, cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_SIMPLE)
        
        if contours:
            contour = max(contours, key=cv2.contourArea)
            x, y, w, h = cv2.boundingRect(contour)
            aspect_ratio = w / float(h)
            solidity = cv2.contourArea(contour) / float(w * h)
            height_ratio = h / float(license_plate.shape[0])

            if 0.1 < aspect_ratio < 1.0 and solidity > 0.1 and 0.35 < height_ratio < 2.0:
                candidate = np.array(mask[y:y + h, x:x + w])
                square_candidate = convert_to_square(candidate)
                square_candidate = cv2.resize(square_candidate, (28, 28), cv2.INTER_AREA)
                square_candidate = square_candidate.reshape((28, 28, 1))
                candidates.append((square_candidate, (y, x)))

    candidates = sorted(candidates, key=lambda x: x[1][0])
    mid_y = np.median([pos[0] for _, pos in candidates])
    top_row = [c for c in candidates if c[1][0] < mid_y]
    bottom_row = [c for c in candidates if c[1][0] >= mid_y]

    top_row = sorted(top_row, key=lambda x: x[1][1])
    bottom_row = sorted(bottom_row, key=lambda x: x[1][1])

    return top_row + bottom_row

def process_image_for_license_plate(image_path):
    """Process image for license plate detection and character recognition"""
    img = cv2.imread(image_path)
    results = yolo_model(image_path)

    for i, result in enumerate(results):
        for j, box in enumerate(result.boxes):
            x1, y1, x2, y2 = map(int, box.xyxy[0])
            license_plate = img[y1:y2, x1:x2]
            
            if license_plate.size == 0:
                print(f"⚠️ Không thể cắt vùng biển số {j}")
                continue

            cropped_path = os.path.join(output_dir, f"license_plate_{i}_{j}.jpg")
            cv2.imwrite(cropped_path, license_plate)

            thresh = preprocess_license_plate(license_plate)
            thresh = cv2.bitwise_not(thresh)
            thresh = imutils.resize(thresh, width=400)
            thresh = cv2.medianBlur(thresh, 5)

            candidates = extract_and_sort_characters(thresh, license_plate)
            save_characters(candidates, image_path)

# # Process the license plate image
# image_path = r"D:\Documents\Work\NganGiang\HAQUANGDUNG\Job74\GreenParking\0000_00532_b.jpg"

# Danh sách các tệp ảnh trong thư mục
image_folder = r"./GreenParking"
image_files = sorted([f for f in os.listdir(image_folder) if f.endswith(('.png', '.jpg'))])
for image_file in image_files:
    image_path = os.path.join(image_folder, image_file) 
    process_image_for_license_plate(image_path)
