import cv2
import re
import easyocr
import logging
from pathlib import Path
from ultralytics import YOLO

# Cấu hình logging
logging.basicConfig(level=logging.INFO, format="%(levelname)s: %(message)s")

# Load model YOLO đã train
YOLO_MODEL_PATH = Path("./License-plate-detection.pt")
model = YOLO(YOLO_MODEL_PATH)

# Thư mục ảnh
IMAGE_PATH = Path(r"D:\Documents\Work\NganGiang\HAQUANGDUNG\Job74\Dataset\0049_04676_b.jpg")
OUTPUT_DIR = Path(r"D:\Documents\Work\NganGiang\HAQUANGDUNG\Job74\Recognize_EASYOCR\Cropped")
OUTPUT_DIR.mkdir(parents=True, exist_ok=True)

# Khởi tạo EasyOCR reader
reader = easyocr.Reader(['en'])

def detect_license_plate(image_path):
    """ Phát hiện biển số xe bằng YOLO và trả về danh sách bounding boxes """
    results = model(str(image_path), verbose=False)
    return results

def crop_and_save(img, bbox, index):
    """ Cắt và lưu ảnh biển số từ bounding box """
    x1, y1, x2, y2 = map(int, bbox.xyxy[0])
    license_plate = img[y1:y2, x1:x2]

    # Kiểm tra nếu ảnh cắt hợp lệ
    if license_plate.size == 0:
        logging.warning(f"Không thể cắt vùng biển số {index}, có thể bbox nằm ngoài ảnh!")
        return None

    # Lưu ảnh biển số đã cắt
    cropped_path = OUTPUT_DIR / f"license_plate_{index}.jpg"
    cv2.imwrite(str(cropped_path), license_plate)
    return cropped_path

def recognize_text(image_path):
    """ Nhận diện ký tự từ ảnh biển số """
    try:
        result = reader.readtext(str(image_path))
        license_plate = "".join(text for _, text, _ in result)

        # Chỉ giữ lại chữ cái và số
        license_plate = re.sub(r'[^A-Za-z0-9]', '', license_plate)
        return license_plate
    except Exception as e:
        logging.error(f"Lỗi khi nhận diện biển số: {e}")
        return None

def main():
    """ Chạy toàn bộ quy trình nhận diện biển số xe và hiển thị kết quả """
    img = cv2.imread(str(IMAGE_PATH))
    if img is None:
        logging.error("Không thể đọc ảnh, vui lòng kiểm tra đường dẫn!")
        return

    results = detect_license_plate(IMAGE_PATH)
    img_display = img.copy()

    for i, result in enumerate(results):
        for j, box in enumerate(result.boxes):
            # Crop và nhận diện
            cropped_path = crop_and_save(img, box, f"{i}_{j}")
            if not cropped_path:
                continue

            plate_text = recognize_text(cropped_path)
            logging.info(f"Biển số xe nhận diện: {plate_text}")

            # Vẽ khung và text lên ảnh gốc
            x1, y1, x2, y2 = map(int, box.xyxy[0])
            cv2.rectangle(img_display, (x1, y1), (x2, y2), (0, 255, 0), 2)

            if plate_text:
                cv2.putText(img_display, plate_text, (x1, y1 - 10),
                            cv2.FONT_HERSHEY_SIMPLEX, 1.0, (0, 0, 255), 2)
    
    import matplotlib.pyplot as plt
    # Chuyển BGR -> RGB để hiển thị đúng màu
    img_rgb = cv2.cvtColor(img_display, cv2.COLOR_BGR2RGB)

    plt.imshow(img_rgb)
    plt.title("Biển số xe đã nhận diện")
    plt.axis("off")
    plt.show()


if __name__ == "__main__":
    main()
