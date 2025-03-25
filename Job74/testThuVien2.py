import cv2
import pytesseract

def segment_image(image_path):
    # Đọc ảnh
    plate = cv2.imread(image_path)
    
    if plate is None:
        print("Không thể mở ảnh! Kiểm tra lại đường dẫn.")
        return ""

    # Chuyển ảnh sang grayscale
    plate = cv2.cvtColor(plate, cv2.COLOR_BGR2GRAY)

    # Áp dụng threshold để tách chữ và nền
    _, threshold = cv2.threshold(plate, 0, 255, cv2.THRESH_BINARY + cv2.THRESH_OTSU)

    # Chuyển ảnh thành text bằng Tesseract
    text = pytesseract.image_to_string(threshold, lang="eng", config="--psm 7")

    print(f"🚗 Biển số nhận diện: {text.strip()}")
    return text.strip()

# Gọi hàm với đường dẫn ảnh
segment_image(r"D:\Documents\Work\NganGiang\HAQUANGDUNG\Job74\GreenParking\0000_00532_b.jpg")
