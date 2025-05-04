import cv2
import numpy as np

def deskew_license_plate(image):
    # Hiển thị ảnh gốc
    cv2.imshow("1. Original Image", image)
    cv2.waitKey(0)
    
    # Chuyển ảnh sang ảnh xám
    gray = cv2.cvtColor(image, cv2.COLOR_BGR2GRAY)
    cv2.imshow("2. Grayscale Image", gray)
    cv2.waitKey(0)
    
    # Làm mờ để giảm nhiễu
    blur = cv2.GaussianBlur(gray, (5, 5), 0)
    cv2.imshow("3. Blurred Image", blur)
    cv2.waitKey(0)
    
    # Nhị phân hóa ảnh
    _, binary = cv2.threshold(blur, 0, 255, cv2.THRESH_BINARY + cv2.THRESH_OTSU)
    cv2.imshow("4. Binary Image", binary)
    cv2.waitKey(0)
    
    # Tìm contours
    contours_image = image.copy()
    contours, _ = cv2.findContours(binary, cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_SIMPLE)
    cv2.drawContours(contours_image, contours, -1, (0, 255, 0), 2)
    cv2.imshow("5. All Contours", contours_image)
    cv2.waitKey(0)
    
    # Tìm contour lớn nhất (giả sử đó là biển số)
    if contours:
        largest_contour = max(contours, key=cv2.contourArea)
        
        # Hiển thị contour lớn nhất
        largest_contour_image = image.copy()
        cv2.drawContours(largest_contour_image, [largest_contour], -1, (0, 0, 255), 2)
        cv2.imshow("6. Largest Contour", largest_contour_image)
        cv2.waitKey(0)
        
        # Tìm hình chữ nhật nhỏ nhất bao quanh contour
        rect = cv2.minAreaRect(largest_contour)
        
        # Hiển thị hình chữ nhật bao quanh
        box = cv2.boxPoints(rect)
        box = np.int0(box)
        rect_image = image.copy()
        cv2.drawContours(rect_image, [box], 0, (255, 0, 0), 2)
        cv2.imshow("7. Minimum Area Rectangle", rect_image)
        cv2.waitKey(0)
        
        # Lấy góc của hình chữ nhật
        angle = rect[2]
        print(f"Góc nghiêng ban đầu: {angle} độ")
        print(f"Điểm tâm: {rect[0]}")
        print(f"Kích thước: {rect[1]}")
        print(f"Điểm góc: {box}")
        print(f"Diện tích: {cv2.contourArea(largest_contour)}")
        print(f"Chu vi: {cv2.arcLength(largest_contour, True)}")
        print(f"Chiều dài cạnh: {rect[1][0]}")
        print(f"Chiều rộng cạnh: {rect[1][1]}")
        print(f"Tỉ lệ cạnh: {rect[1][0] / rect[1][1]}")
        print(f"Điểm góc: {box}")        
        # Điều chỉnh góc một cách toàn diện hơn
        if angle < -45:
            angle = 90 + angle
        elif angle > 45:  # Thêm điều kiện này
            angle = angle - 90
        
        print(f"Góc nghiêng sau điều chỉnh: {angle} độ")
        
        # Lấy kích thước của ảnh
        (h, w) = image.shape[:2]
        
        # Tính tâm xoay
        center = (w // 2, h // 2)
        
        # Tạo ma trận xoay
        M = cv2.getRotationMatrix2D(center, angle, 1.0)
        
        # Xoay ảnh
        rotated = cv2.warpAffine(image, M, (w, h), flags=cv2.INTER_CUBIC, 
                                 borderMode=cv2.BORDER_REPLICATE)
        
        # Hiển thị kết quả cuối cùng
        cv2.imshow("8. Deskewed Image", rotated)
        cv2.waitKey(0)
        
        return rotated, angle
    
    return image, 0

# Đọc ảnh
image = cv2.imread(r"./Cropped/0397.jpg")

# Kiểm tra xem ảnh có được đọc thành công không
if image is None:
    print("Không thể đọc ảnh. Vui lòng kiểm tra đường dẫn.")
else:
    # Xoay ảnh biển số
    deskewed_image, angle = deskew_license_plate(image)
    
    # Hiển thị so sánh ảnh gốc và ảnh đã xoay cùng nhau
    comparison = np.hstack((image, deskewed_image))
    cv2.imshow("Comparison: Original vs Deskewed", comparison)
    cv2.waitKey(0)
    
    # Đóng tất cả các cửa sổ
    cv2.destroyAllWindows()