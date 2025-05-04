import argparse
import argparse
import os
import cv2
import numpy as np
import logging
import warnings
from skimage.filters import threshold_local
from skimage import measure
import imutils
from ultralytics import YOLO
import tensorflow as tf
from tensorflow.keras.models import load_model
import shutil
from PIL import Image, ImageDraw, ImageFont

# ==== LOGGING & WARNING SETUP ====
os.environ['TF_CPP_MIN_LOG_LEVEL'] = '3'
logging.getLogger('tensorflow').setLevel(logging.ERROR)
logging.getLogger('absl').setLevel(logging.ERROR)
logging.getLogger('ultralytics').setLevel(logging.ERROR)
warnings.filterwarnings("ignore", category=UserWarning, module='tensorflow')

CONFIDENCE_THRESHOLD = 0.85  # Đặt giá trị độ chính xác tối thiểu là 85%


# ==== UTILS ====
def convert_to_square(image, pad_color=255):
    h, w = image.shape[:2]
    size_diff = abs(h - w)
    if h == w:
        return image
    if len(image.shape) == 2:
        pad = [(size_diff // 2, size_diff - size_diff // 2), (0, 0)] if h < w else [(0, 0), (size_diff // 2, size_diff - size_diff // 2)]
    else:
        pad = [(0, 0), (0, 0), (0, 0)]
        if h < w:
            pad[0] = (size_diff // 2, size_diff - size_diff // 2)
        else:
            pad[1] = (size_diff // 2, size_diff - size_diff // 2)
    return np.pad(image, pad, mode='constant', constant_values=pad_color)

def preprocess_license_plate(plate_img):
    hsv = cv2.cvtColor(plate_img, cv2.COLOR_BGR2HSV)
    _, _, v = cv2.split(hsv)
    T = threshold_local(v, 15, offset=10, method="gaussian")
    return ((v > T) * 255).astype("uint8")

def extract_and_sort_characters(thresh, plate_img):
    inverted = cv2.bitwise_not(thresh)
    resized = imutils.resize(inverted, width=400)
    blurred = cv2.medianBlur(resized, 5)
    labels = measure.label(blurred, connectivity=2, background=0)

    candidates = []
    scale_x = plate_img.shape[1] / resized.shape[1]
    scale_y = plate_img.shape[0] / resized.shape[0]

    for label in np.unique(labels):
        if label == 0:
            continue
        mask = (labels == label).astype("uint8") * 255
        contours, _ = cv2.findContours(mask, cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_SIMPLE)
        if not contours:
            continue

        contour = max(contours, key=cv2.contourArea)
        x, y, w, h = cv2.boundingRect(contour)
        aspect_ratio = w / float(h)
        solidity = cv2.contourArea(contour) / float(w * h)
        height_ratio = h / float(plate_img.shape[0])

        if 0.1 < aspect_ratio < 1.0 and solidity > 0.1 and 0.35 < height_ratio < 2.0:
            x_orig, y_orig = int(x * scale_x), int(y * scale_y)
            w_orig, h_orig = int(w * scale_x), int(h * scale_y)
            char_img = plate_img[y_orig:y_orig + h_orig, x_orig:x_orig + w_orig]
            square_char = convert_to_square(char_img)
            resized_char = cv2.resize(square_char, (28, 28), cv2.INTER_AREA)
            candidates.append((resized_char, (y, x)))

    candidates = sorted(candidates, key=lambda x: x[1][0])
    mid_y = np.median([pos[0] for _, pos in candidates])
    top = sorted([c for c in candidates if c[1][0] < mid_y], key=lambda x: x[1][1])
    bottom = sorted([c for c in candidates if c[1][0] >= mid_y], key=lambda x: x[1][1])
    return top + bottom

def save_characters(candidates, original_filename, output_dir):
    prefix = os.path.splitext(os.path.basename(original_filename))[0]
    for idx, (char_img, _) in enumerate(candidates):
        filename = os.path.join(output_dir, f"{prefix}_char_{idx:02d}.png")
        cv2.imwrite(filename, char_img)

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
        return cv2.warpAffine(image, M, (w, h), flags=cv2.INTER_CUBIC, borderMode=cv2.BORDER_REPLICATE)
    return image

def nhan_dien_ky_tu(path, model_path, img_width=64, img_height=64, grayscale=True):
    model = load_model(model_path)
    if grayscale:
        img = cv2.imread(path, cv2.IMREAD_GRAYSCALE)
        img = cv2.resize(img, (img_width, img_height))
        img = np.expand_dims(img, axis=-1)
    else:
        img = cv2.imread(path)
        img = cv2.cvtColor(img, cv2.COLOR_BGR2RGB)
        img = cv2.resize(img, (img_width, img_height))
    img = img.astype('float32') / 255.0
    img = np.expand_dims(img, axis=0)
    prediction = model.predict(img, verbose=0)
    predicted_class = np.argmax(prediction, axis=1)[0]
    # idx_to_label = {
    #     0: '0', 1: '1', 2: '2', 3: '3', 4: '4', 5: '5', 6: '6', 7: '7', 8: '8', 9: '9',
    #     10: 'A', 11: 'B', 12: 'C', 13: 'D', 14: 'E', 15: 'F', 16: 'G', 17: 'H',
    #     18: 'K', 19: 'L', 20: 'M', 21: 'N', 22: 'P', 23: 'R', 24: 'S', 25: 'T',
    #     26: 'U', 27: 'V', 28: 'X', 29: 'Y', 30: 'Z'
    # }
    # return idx_to_label[predicted_class], np.max(prediction)
    
    confidence = np.max(prediction)
    # So sánh độ chính xác với biến toàn cục CONFIDENCE_THRESHOLD
    if confidence > CONFIDENCE_THRESHOLD:
        idx_to_label = {
            0: '0', 1: '1', 2: '2', 3: '3', 4: '4', 5: '5', 6: '6', 7: '7', 8: '8', 9: '9',
            10: 'A', 11: 'B', 12: 'C', 13: 'D', 14: 'E', 15: 'F', 16: 'G', 17: 'H',
            18: 'K', 19: 'L', 20: 'M', 21: 'N', 22: 'P', 23: 'R', 24: 'S', 25: 'T',
            26: 'U', 27: 'V', 28: 'X', 29: 'Y', 30: 'Z'
        }
        return idx_to_label[predicted_class], confidence
    else:
        # Nếu độ chính xác dưới ngưỡng, trả về kết quả rỗng
        return "", 0

def process_single_image(image_path, yolo_model, model_path, cropped_dir, char_output_dir, output_image_dir):
    img = cv2.imread(image_path)
    results = yolo_model(image_path)
    for i, result in enumerate(results):
        for j, box in enumerate(result.boxes):
            x1, y1, x2, y2 = map(int, box.xyxy[0])
            plate = img[y1:y2, x1:x2]
            if plate.size == 0:
                continue
            plate = deskew_license_plate(plate)
            binary = preprocess_license_plate(plate)
            candidates = extract_and_sort_characters(binary, plate)
            prefix = f"plate_{i}_{j}"
            for idx, (char_img, _) in enumerate(candidates):
                filename = os.path.join(char_output_dir, f"{prefix}_char_{idx:02d}.png")
                cv2.imwrite(filename, char_img)
            recognized_text = ""
            image_paths = sorted([f for f in os.listdir(char_output_dir) if f.startswith(prefix) and f.endswith(".png")])
            # for img_file in image_paths:
            #     full_path = os.path.join(char_output_dir, img_file)
            #     ky_tu, _ = nhan_dien_ky_tu(full_path, model_path)
            #     recognized_text += ky_tu
            
            for img_file in image_paths:
                full_path = os.path.join(char_output_dir, img_file)
                ky_tu, confidence = nhan_dien_ky_tu(full_path, model_path)
                if confidence > CONFIDENCE_THRESHOLD:  # Kiểm tra độ chính xác so với ngưỡng toàn cục
                    recognized_text += ky_tu
                    
            print(f"{recognized_text}")
            img_pil = Image.fromarray(cv2.cvtColor(img, cv2.COLOR_BGR2RGB))
            draw = ImageDraw.Draw(img_pil)
            try:
                font = ImageFont.truetype("arial.ttf", 32)
            except:
                font = ImageFont.load_default()
            draw.text((x1, y1 - 40), recognized_text, font=font, fill=(255, 0, 0))
            draw.rectangle([(x1, y1), (x2, y2)], outline=(0, 255, 0), width=3)
            
            # Lưu kết quả vào thư mục output_image_dir thay vì hiển thị
            result_image_path = os.path.join(output_image_dir, f"result_{i}_{j}.png")
            img_result = cv2.cvtColor(np.array(img_pil), cv2.COLOR_RGB2BGR)
            cv2.imwrite(result_image_path, img_result)

# ==== MAIN ====
if __name__ == "__main__":
    parser = argparse.ArgumentParser(description="License Plate Recognition from Image")
    parser.add_argument("image_path", type=str, help="Path to the input image")
    parser.add_argument("--model_path", type=str, required=True, help="Path to character recognition model (.h5)")
    parser.add_argument("--yolo_path", type=str, required=True, help="Path to YOLOv8 model (.pt)")
    parser.add_argument("--cropped_dir", type=str, default="./Test/Cropped", help="Directory for cropped plates")
    parser.add_argument("--char_output_dir", type=str, default="./Test/output_chars", help="Directory for character output")
    parser.add_argument("--output_image_dir", type=str, default="./Test/output_images", help="Directory for saving output images")

    args = parser.parse_args()

    os.makedirs(args.cropped_dir, exist_ok=True)
    os.makedirs(args.char_output_dir, exist_ok=True)
    os.makedirs(args.output_image_dir, exist_ok=True)

    yolo_model = YOLO(args.yolo_path)

    process_single_image(args.image_path, yolo_model, args.model_path, args.cropped_dir, args.char_output_dir, args.output_image_dir)



"""
python your_script.py "test_images/car.jpg" \
  --model_path "models/model_nhan_dang_bien_so_final_2.h5" \
  --yolo_path "models/License-plate-detection.pt" \
  --cropped_dir "./outputs/cropped" \
  --char_output_dir "./outputs/chars"
"""