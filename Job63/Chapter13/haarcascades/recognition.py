import cv2

# Khởi tạo camera
camera = cv2.VideoCapture(0, cv2.CAP_V4L2)
camera.set(cv2.CAP_PROP_FRAME_WIDTH, 500)
camera.set(cv2.CAP_PROP_FRAME_HEIGHT, 500)

# Tải mô hình nhận diện khuôn mặt
recognizer = cv2.face.LBPHFaceRecognizer_create()
recognizer.read('./trainer/trainer.yml')

# Tải tệp cascade để phát hiện khuôn mặt
face_cascade = cv2.CascadeClassifier('./haarcascade_frontalface_default.xml')
font = cv2.FONT_HERSHEY_SIMPLEX

# Khởi tạo danh sách lưu tên người dùng
names = []

# Đọc dữ liệu từ tệp tin names.txt
try:
    with open("names.txt", "r") as file:
        for line in file:
            face_id, face_name = line.strip().split(': ')
            names.append(face_name)
    print("[INFO] Danh sách tên đã tải:", names)
except FileNotFoundError:
    print("[ERROR] Tệp tin names.txt không tồn tại. Vui lòng kiểm tra lại!")
    exit()
except ValueError:
    print("[ERROR] Dữ liệu trong tệp tin names.txt không đúng định dạng. Vui lòng kiểm tra lại!")
    exit()

# Bắt đầu nhận diện khuôn mặt
while True:
    ret, frame = camera.read()

    if not ret:
        print("[ERROR] Không thể nhận khung hình từ camera.")
        break

    # Chuyển đổi khung hình sang ảnh xám
    gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)

    # Phát hiện khuôn mặt trong khung hình
    faces = face_cascade.detectMultiScale(
        gray, scaleFactor=1.2, minNeighbors=5, minSize=(100, 100), flags=cv2.CASCADE_SCALE_IMAGE)

    print(f"[INFO] Phát hiện {len(faces)} khuôn mặt.")

    # Xử lý từng khuôn mặt được phát hiện
    for (x, y, w, h) in faces:
        roi_gray = gray[y:y + h, x:x + w]
        cv2.rectangle(frame, (x, y), (x + w, y + h), (255, 0, 0), 2)

        # Dự đoán ID và độ tin cậy
        id, confidence = recognizer.predict(roi_gray)

        # Xử lý kết quả dự đoán
        if 0 < id <= len(names) and confidence < 100:
            recognized_name = names[id - 1]
            confidence_text = "  {0}%".format(round(100 - confidence))
        else:
            recognized_name = "unknown"
            confidence_text = "  {0}%".format(round(100 - confidence))

        # Hiển thị tên và độ tin cậy
        cv2.putText(frame, str(recognized_name), (x + 5, y - 5), font, 1, (255, 255, 255), 2)
        cv2.putText(frame, confidence_text, (x + 5, y + h - 5), font, 1, (255, 255, 0), 1)

    # Hiển thị khung hình
    cv2.imshow("Recognition", frame)

    # Nhấn phím 'q' để thoát
    if cv2.waitKey(1) & 0xFF == ord('q'):
        break

# Giải phóng tài nguyên
camera.release()
cv2.destroyAllWindows()
