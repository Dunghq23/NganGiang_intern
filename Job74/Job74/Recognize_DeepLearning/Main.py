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
# Tắt log TensorFlow từ C++
os.environ['TF_CPP_MIN_LOG_LEVEL'] = '3'
logging.getLogger('tensorflow').setLevel(logging.ERROR)
logging.getLogger('absl').setLevel(logging.ERROR)
logging.getLogger('ultralytics').setLevel(logging.ERROR)
warnings.filterwarnings("ignore", category=UserWarning, module='tensorflow')


# ==== CONSTANTS ====
CROPPED_DIR = r"D:\Documents\Work\NganGiang\HAQUANGDUNG\Job74\Recognize_DeepLearning\Cropped"
CHAR_OUTPUT_DIR = r"D:\Documents\Work\NganGiang\HAQUANGDUNG\Job74\Recognize_DeepLearning\output_chars"
MODEL_PATH = r"D:\Documents\Work\NganGiang\HAQUANGDUNG\Job74\Recognize_DeepLearning\model_nhan_dang_bien_so_final.h5"
os.makedirs(CROPPED_DIR, exist_ok=True)
os.makedirs(CHAR_OUTPUT_DIR, exist_ok=True)

# ==== MODEL LOADING ====
yolo_model = YOLO("./License-plate-detection.pt")

# ==== UTILS ====

def convert_to_square(image, pad_color=255):
    """Pad image to make it square."""
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
    """Convert plate image to binary using adaptive thresholding."""
    hsv = cv2.cvtColor(plate_img, cv2.COLOR_BGR2HSV)
    _, _, v = cv2.split(hsv)
    T = threshold_local(v, 15, offset=10, method="gaussian")
    return ((v > T) * 255).astype("uint8")

def extract_and_sort_characters(thresh, plate_img):
    """Extract character candidates from binary image."""
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

    # Sort by rows then columns
    candidates = sorted(candidates, key=lambda x: x[1][0])
    mid_y = np.median([pos[0] for _, pos in candidates])
    top = sorted([c for c in candidates if c[1][0] < mid_y], key=lambda x: x[1][1])
    bottom = sorted([c for c in candidates if c[1][0] >= mid_y], key=lambda x: x[1][1])
    return top + bottom

def save_characters(candidates, original_filename):
    """Save sorted character images to output directory."""
    prefix = os.path.splitext(os.path.basename(original_filename))[0]
    for idx, (char_img, _) in enumerate(candidates):
        filename = os.path.join(CHAR_OUTPUT_DIR, f"{prefix}_char_{idx:02d}.png")
        cv2.imwrite(filename, char_img)

def detect_and_crop_license_plate(image_path):
    """Detect and crop license plates from an image."""
    img = cv2.imread(image_path)
    if img is None:
        print(f"⚠️ Cannot read image: {image_path}")
        return []

    results = yolo_model(image_path)
    cropped_paths = []

    for i, result in enumerate(results):
        for j, box in enumerate(result.boxes):
            x1, y1, x2, y2 = map(int, box.xyxy[0])
            plate = img[y1:y2, x1:x2]
            if plate.size == 0:
                continue
            filename = os.path.splitext(os.path.basename(image_path))[0]
            cropped_path = os.path.join(CROPPED_DIR, f"{filename}_plate_{i}_{j}.jpg")
            cv2.imwrite(cropped_path, plate)
            # print(f"✅ Cropped plate saved: {cropped_path}")
            cropped_paths.append(cropped_path)

    return cropped_paths

def process_single_image(image_path):
    """Full pipeline for a single image: detect plate, crop, segment characters, recognize and draw result."""
    img = cv2.imread(image_path)
    if img is None:
        print(f"⚠️ Cannot read image: {image_path}")
        return

    results = yolo_model(image_path)
    for i, result in enumerate(results):
        for j, box in enumerate(result.boxes):
            x1, y1, x2, y2 = map(int, box.xyxy[0])
            plate = img[y1:y2, x1:x2]
            if plate.size == 0:
                continue

            # Tiền xử lý & tách ký tự
            binary = preprocess_license_plate(plate)
            candidates = extract_and_sort_characters(binary, plate)
            prefix = f"plate_{i}_{j}"
            for idx, (char_img, _) in enumerate(candidates):
                filename = os.path.join(CHAR_OUTPUT_DIR, f"{prefix}_char_{idx:02d}.png")
                cv2.imwrite(filename, char_img)

            # Nhận diện ký tự
            recognized_text = ""
            image_paths = sorted([
                f for f in os.listdir(CHAR_OUTPUT_DIR)
                if f.startswith(prefix) and f.endswith(".png")
            ])
            for img_file in image_paths:
                full_path = os.path.join(CHAR_OUTPUT_DIR, img_file)
                ky_tu, _ = nhan_dien_ky_tu(full_path, MODEL_PATH)
                recognized_text += ky_tu

            print(f"✅ Biển số phát hiện: {recognized_text}")

            # Vẽ biển số lên ảnh
            img_pil = Image.fromarray(cv2.cvtColor(img, cv2.COLOR_BGR2RGB))
            draw = ImageDraw.Draw(img_pil)
            try:
                font = ImageFont.truetype("arial.ttf", 32)
            except:
                font = ImageFont.load_default()
            draw.text((x1, y1 - 40), recognized_text, font=font, fill=(255, 0, 0))

            # Vẽ khung biển số
            draw.rectangle([(x1, y1), (x2, y2)], outline=(0, 255, 0), width=3)
            img_result = cv2.cvtColor(np.array(img_pil), cv2.COLOR_RGB2BGR)

            # Hiển thị ảnh
            cv2.imshow("Ket qua nhan dien bien so bang Deep Learning", img_result)
            cv2.waitKey(0)
            cv2.destroyAllWindows()


def nhan_dien_ky_tu(duong_dan_anh, model_path, img_width=64, img_height=64, grayscale=True):
    """Nhận diện ký tự từ ảnh bằng model đã huấn luyện."""
    model = load_model(model_path)

    if grayscale:
        img = cv2.imread(duong_dan_anh, cv2.IMREAD_GRAYSCALE)
        img = cv2.resize(img, (img_width, img_height))
        img = np.expand_dims(img, axis=-1)
    else:
        img = cv2.imread(duong_dan_anh)
        img = cv2.cvtColor(img, cv2.COLOR_BGR2RGB)
        img = cv2.resize(img, (img_width, img_height))

    img = img.astype('float32') / 255.0
    img = np.expand_dims(img, axis=0)

    prediction = model.predict(img, verbose=0)
    predicted_class = np.argmax(prediction, axis=1)[0]
    probability = np.max(prediction)

    idx_to_label = {
        0: '0', 1: '1', 2: '2', 3: '3', 4: '4', 5: '5', 6: '6', 7: '7', 8: '8', 9: '9',
        10: 'A', 11: 'B', 12: 'C', 13: 'D', 14: 'E', 15: 'F', 16: 'G', 17: 'H',
        18: 'K', 19: 'L', 20: 'M', 21: 'N', 22: 'P', 23: 'R', 24: 'S', 25: 'T',
        26: 'U', 27: 'V', 28: 'X', 29: 'Y', 30: 'Z'
    }

    return idx_to_label[predicted_class], probability

def recognize():
    """Nhận diện toàn bộ ký tự từ thư mục ./output_chars."""
    image_paths = sorted([
        f for f in os.listdir(CHAR_OUTPUT_DIR)
        if f.lower().endswith(('.png', '.jpg', '.jpeg'))
    ])
    license_plate = ""
    for img in image_paths:
        full_path = os.path.join(CHAR_OUTPUT_DIR, img)
        ky_tu, _ = nhan_dien_ky_tu(full_path, MODEL_PATH)
        license_plate += ky_tu
    print(f"Biển số nhận diện được: {license_plate}")

# ==== MAIN ====
if __name__ == "__main__":
    test_image = r"D:\Documents\Work\NganGiang\HAQUANGDUNG\Job74\Dataset/0049_04676_b.jpg"
    process_single_image(test_image)

    # Cleanup
    shutil.rmtree(CHAR_OUTPUT_DIR)
    shutil.rmtree(CROPPED_DIR)
