import cv2

def detectAndDisplay(frame):
    # Chuyển frame sang ảnh đen trắng
    frame_gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)
    frame_gray = cv2.equalizeHist(frame_gray)

    # Phát hiện khuôn mặt
    faces = face_cascade.detectMultiScale(frame_gray)
    for (x, y, w, h) in faces:
        center = (x + w//2, y + h//2)
        frame = cv2.ellipse(frame, center, (w//2, h//2), 0, 0, 360, (255, 0, 255), 4)

        faceROI = frame_gray[y:y+h, x:x+w]
        # Phát hiện mắt trong khuôn mặt
        eyes = eyes_cascade.detectMultiScale(faceROI)
        for (x2, y2, w2, h2) in eyes:
            eye_center = (x + x2 + w2//2, y + y2 + h2//2)
            radius = int(round((w2 + h2)*0.25))
            frame = cv2.circle(frame, eye_center, radius, (255, 0, 0), 4)

    # Hiển thị khung hình
    cv2.imshow('Camera Feed', frame)

# Cài đặt cascade cho khuôn mặt và mắt
face_cascade = cv2.CascadeClassifier(cv2.data.haarcascades + 'haarcascade_frontalface_default.xml')
eyes_cascade = cv2.CascadeClassifier(cv2.data.haarcascades + 'haarcascade_eye.xml')

# Mở camera
cap = cv2.VideoCapture(0)

if not cap.isOpened():
    print("Không thể mở camera!")
    exit()

while True:
    # Đọc khung hình từ camera
    ret, frame = cap.read()
    if not ret:
        print("Không thể nhận khung hình!")
        break

    # Hiển thị khung hình và phát hiện khuôn mặt/mắt
    detectAndDisplay(frame)

    # Nhấn 'q' để thoát khỏi vòng lặp
    if cv2.waitKey(1) & 0xFF == ord('q'):
        break

# Giải phóng camera và đóng cửa sổ
cap.release()
cv2.destroyAllWindows()
