from ultralytics import YOLO
import cv2
import os
import numpy as np
from skimage.filters import threshold_local
import matplotlib.pyplot as plt
import imutils
from skimage import measure

import tensorflow as tf
from tensorflow import keras

# Load mô hình
model = keras.models.load_model("./handwritten_model.h5")

# Thư mục chứa ảnh ký tự đã cắt
input_folder = "./output_chars"

# Lưu kết quả dự đoán
predicted_text = ""

# def save_characters(sorted_candidates, save_dir="output_chars"):
#     if not os.path.exists(save_dir):
#         os.makedirs(save_dir)

#     for index, (char, bbox) in enumerate(sorted_candidates):
#         filename = f"{save_dir}/char_{index:02d}.png"
#         cv2.imwrite(filename, char)
#         print(f"Saved {filename}")
        
        
def save_characters(sorted_candidates, original_filename, save_dir="output_chars"):
    if not os.path.exists(save_dir):
        os.makedirs(save_dir)

    # Lấy tên file gốc mà không có phần mở rộng
    file_prefix = os.path.splitext(os.path.basename(original_filename))[0]

    for index, (char, bbox) in enumerate(sorted_candidates):
        filename = f"{save_dir}/{file_prefix}_char_{index:02d}.png"
        cv2.imwrite(filename, char)
        print(f"Saved {filename}")

def convert2Square(image):
    """
    Resize non square image(height != width to square one (height == width)
    :param image: input images
    :return: numpy array
    """

    img_h = image.shape[0]
    img_w = image.shape[1]

    # if height > width
    if img_h > img_w:
        diff = img_h - img_w
        if diff % 2 == 0:
            x1 = np.zeros(shape=(img_h, diff//2))
            x2 = x1
        else:
            x1 = np.zeros(shape=(img_h, diff//2))
            x2 = np.zeros(shape=(img_h, (diff//2) + 1))

        squared_image = np.concatenate((x1, image, x2), axis=1)
    elif img_w > img_h:
        diff = img_w - img_h
        if diff % 2 == 0:
            x1 = np.zeros(shape=(diff//2, img_w))
            x2 = x1
        else:
            x1 = np.zeros(shape=(diff//2, img_w))
            x2 = x1

        squared_image = np.concatenate((x1, image, x2), axis=0)
    else:
        squared_image = image

    return squared_image

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

# Hiển thị ảnh gốc
plt.figure(figsize=(10, 5))
plt.subplot(1, 3, 1)
plt.imshow(cv2.cvtColor(img, cv2.COLOR_BGR2RGB))
plt.title("Ảnh gốc")
plt.axis("off")


ALPHA_DICT = {0: 'A', 1: 'B', 2: 'C', 3: 'D', 4: 'E', 5: 'F', 6: 'G', 7: 'H', 8: 'K', 9: 'L', 10: 'M', 11: 'N', 12: 'P',
              13: 'R', 14: 'S', 15: 'T', 16: 'U', 17: 'V', 18: 'X', 19: 'Y', 20: 'Z', 21: '0', 22: '1', 23: '2', 24: '3',
              25: '4', 26: '5', 27: '6', 28: '7', 29: '8', 30: '9', 31: "Background"}


# Duyệt qua các kết quả phát hiện
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

        # Hiển thị ảnh biển số đã cắt
        plt.subplot(1, 3, 2)
        plt.imshow(cv2.cvtColor(license_plate, cv2.COLOR_BGR2RGB))
        plt.title("Biển số cắt")
        plt.axis("off")

        # Chuyển đổi BGR -> HSV
        hsv = cv2.cvtColor(license_plate, cv2.COLOR_BGR2HSV)
        V = cv2.split(hsv)[2]

        # Áp dụng adaptive threshold
        T = threshold_local(V, 15, offset=10, method="gaussian")
        thresh = (V > T).astype("uint8") * 255

        # Hiển thị ảnh đã xử lý threshold
        plt.subplot(1, 3, 3)
        plt.imshow(thresh, cmap="gray")
        plt.title("Sau threshold")
        plt.axis("off")


        # convert black pixel of digits to white pixel
        thresh = cv2.bitwise_not(thresh)
        cv2.imwrite("step2_2.png", thresh)
        thresh = imutils.resize(thresh, width=400)
        thresh = cv2.medianBlur(thresh, 5)

        # connected components analysis
        labels = measure.label(thresh, connectivity=2, background=0)

        candidates = []
        # loop over the unique components
        for label in np.unique(labels):
            # if this is background label, ignore it
            if label == 0:
                continue

            # init mask to store the location of the character candidates
            mask = np.zeros(thresh.shape, dtype="uint8")
            mask[labels == label] = 255

            # find contours from mask
            contours, hierarchy = cv2.findContours(mask, cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_SIMPLE)

            if len(contours) > 0:
                contour = max(contours, key=cv2.contourArea)
                (x, y, w, h) = cv2.boundingRect(contour)

                # rule to determine characters
                aspectRatio = w / float(h)
                solidity = cv2.contourArea(contour) / float(w * h)
                heightRatio = h / float(license_plate.shape[0])

                if 0.1 < aspectRatio < 1.0 and solidity > 0.1 and 0.35 < heightRatio < 2.0:
                    # extract characters
                    candidate = np.array(mask[y:y + h, x:x + w])
                    square_candidate = convert2Square(candidate)
                    square_candidate = cv2.resize(square_candidate, (28, 28), cv2.INTER_AREA)
                    square_candidate = square_candidate.reshape((28, 28, 1))
                    candidates.append((square_candidate, (y, x)))

       # Sắp xếp ký tự theo tọa độ y trước (tức là dòng trên trước dòng dưới)
        candidates = sorted(candidates, key=lambda x: x[1][0])

        # Phân chia thành hai dòng dựa trên tọa độ y
        mid_y = np.median([pos[0] for _, pos in candidates])  # Lấy giá trị y trung bình để phân chia
        top_row = [c for c in candidates if c[1][0] < mid_y]
        bottom_row = [c for c in candidates if c[1][0] >= mid_y]

        # Sắp xếp từng dòng theo trục x (từ trái qua phải)
        top_row = sorted(top_row, key=lambda x: x[1][1])
        bottom_row = sorted(bottom_row, key=lambda x: x[1][1])

        # Hiển thị kết quả
        plt.figure(figsize=(10, 2))
        for idx, (char, position) in enumerate(top_row + bottom_row):
            plt.subplot(1, len(candidates), idx + 1)
            plt.imshow(char.squeeze(), cmap="gray")
            plt.axis("off")
        # plt.show()

        save_characters(top_row + bottom_row, image_path)

        # Lưu ảnh đã threshold
        threshold_path = os.path.join(output_dir, f"license_plate_{i}_{j}_thresh.jpg")
        cv2.imwrite(threshold_path, thresh)


    


# plt.show()

# Lặp qua tất cả các tệp ảnh trong thư mục
for filename in sorted(os.listdir(input_folder)):  # Sắp xếp để đọc theo thứ tự
    if filename.endswith(".png") or filename.endswith(".jpg"):
        img_path = os.path.join(input_folder, filename)

        # Đọc ảnh
        img = cv2.imread(img_path, cv2.IMREAD_GRAYSCALE)

        # Kiểm tra nếu ảnh bị lỗi
        if img is None:
            print(f"Lỗi: Không thể đọc {img_path}")
            continue

        img = cv2.resize(img, (28, 28))  # Resize về đúng kích thước
        img = img / 255.0  # Chuẩn hóa ảnh về [0, 1]
        img = img.reshape(1, 28, 28, 1)  # Định dạng batch cho model

        # Dự đoán ký tự
        prediction = model.predict(img)
        predicted_class = np.argmax(prediction)  # Lấy nhãn có xác suất cao nhất

        # Tra cứu ký tự từ từ điển
        predicted_char = ALPHA_DICT.get(predicted_class, "?")
        predicted_text += predicted_char

        print(f"Ảnh {filename}: Dự đoán '{predicted_char}'")

# In toàn bộ biển số đã nhận diện
print(f"Biển số dự đoán: {predicted_text}")