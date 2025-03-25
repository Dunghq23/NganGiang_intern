import os
import re
import cv2
from google.cloud import vision
from ultralytics import YOLO

# Cấu hình Google Cloud Vision API
os.environ['GOOGLE_APPLICATION_CREDENTIALS'] = r'./ServiceAccountToken.json'

# Load model YOLO đã train
model = YOLO("./License-plate-detection.pt")

# Đọc ảnh
image_path = r"D:\Documents\Work\NganGiang\HAQUANGDUNG\Job74\GreenParking\0000_00532_b.jpg"
img = cv2.imread(image_path)

# Chạy mô hình để phát hiện biển số xe
results = model(image_path)

# Thư mục lưu ảnh biển số cắt
output_dir = r"D:\Documents\Work\NganGiang\HAQUANGDUNG\Job74\Cropped"
os.makedirs(output_dir, exist_ok=True)

# Hàm nhận diện ký tự trên biển số xe
def detect_texts(path):
    client = vision.ImageAnnotatorClient()
    with open(path, "rb") as image_file:
        content = image_file.read()

    image = vision.Image(content=content)
    response = client.text_detection(image=image)
    texts = response.text_annotations

    if not texts:
        return "Không xác định"

    # Chỉ lấy nội dung của biển số xe (dòng đầu tiên)
    text = texts[0].description

    # Giữ lại chỉ chữ và số
    cleaned_text = re.sub(r'[^A-Za-z0-9]', '', text)
    return cleaned_text

# Duyệt qua các kết quả phát hiện biển số xe
for i, result in enumerate(results):
    for j, box in enumerate(result.boxes):
        # Lấy tọa độ của bounding box
        x1, y1, x2, y2 = map(int, box.xyxy[0])

        # Cắt vùng chứa biển số
        license_plate = img[y1:y2, x1:x2]

        # Kiểm tra nếu ảnh cắt có kích thước hợp lệ
        if license_plate.size == 0:
            print(f"⚠️ Không thể cắt vùng biển số {j}, có thể bbox nằm ngoài ảnh!")
            continue

        # Lưu ảnh biển số gốc
        cropped_path = os.path.join(output_dir, f"license_plate_{i}_{j}.jpg")
        cv2.imwrite(cropped_path, license_plate)

        # Nhận diện biển số từ ảnh cắt
        plate_number = detect_texts(cropped_path)
        print(f"Biển số xe nhận diện được: {plate_number}")

        # Vẽ bounding box và biển số lên ảnh gốc
        cv2.rectangle(img, (x1, y1), (x2, y2), (0, 255, 0), 2)
        cv2.putText(img, plate_number, (x1, y1 - 5), cv2.FONT_HERSHEY_SIMPLEX, 0.8, (0, 255, 0), 2)

# Hiển thị ảnh gốc với biển số xe
cv2.imshow("Detected License Plates", img)
cv2.waitKey(0)
cv2.destroyAllWindows()