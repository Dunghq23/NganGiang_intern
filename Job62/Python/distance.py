import cv2
import numpy as np

# Đọc ảnh
image = cv2.imread('./first_frame.jpg')

# Chuyển ảnh sang grayscale
gray = cv2.cvtColor(image, cv2.COLOR_BGR2GRAY)

# Áp dụng Canny để phát hiện cạnh
edges = cv2.Canny(gray, 50, 150)

# Hiển thị kết quả cạnh
cv2.imshow("Edges", edges)

# Lựa chọn vùng ROI (tập trung vào vùng làn đường)
roi = edges[300:600, 200:600]  # Chỉnh toạ độ tuỳ vào vị trí làn đường trong ảnh

# Tìm đường thẳng bằng Hough Transform
lines = cv2.HoughLinesP(roi, 1, np.pi/180, 100, minLineLength=50, maxLineGap=20)

# Vẽ các đường kẻ
if lines is not None:
    for line in lines:
        x1, y1, x2, y2 = line[0]
        cv2.line(image, (x1, y1), (x2, y2), (0, 255, 0), 2)

# Hiển thị ảnh kết quả
cv2.imshow("Detected Lanes", image)
cv2.waitKey(0)
cv2.destroyAllWindows()
