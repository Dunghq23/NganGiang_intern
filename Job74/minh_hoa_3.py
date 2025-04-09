import os
import cv2
import numpy as np
from skimage.filters import threshold_local
import imutils
from skimage import measure
import matplotlib.pyplot as plt
from ultralytics import YOLO

# Hàm hiển thị ảnh
def plot_images(images, titles, figsize=(15, 10), rows=1):
    """Hiển thị nhiều ảnh với tiêu đề tương ứng"""
    plt.figure(figsize=figsize)
    for i, (img, title) in enumerate(zip(images, titles)):
        plt.subplot(rows, len(images) // rows + (1 if len(images) % rows != 0 else 0), i + 1)
        
        if len(img.shape) == 2:  # Ảnh grayscale
            plt.imshow(img, cmap='gray')
        else:  # Ảnh màu
            plt.imshow(cv2.cvtColor(img, cv2.COLOR_BGR2RGB))
            
        plt.title(title)
        plt.axis('off')
    plt.tight_layout()
    plt.show()

# Load YOLO model for license plate detection
yolo_model = YOLO("./License-plate-detection.pt")

# Directory paths
input_folder = "./output_chars"
output_dir = r"./Cropped"
# Create output directories if they don't exist
os.makedirs(output_dir, exist_ok=True)

def save_characters(sorted_candidates, original_filename, save_dir="output_chars"):
    """Save character images to specified directory"""
    if not os.path.exists(save_dir):
        os.makedirs(save_dir)
    
    file_prefix = os.path.splitext(os.path.basename(original_filename))[0]
    
    # Hiển thị các ký tự đã tách được
    if sorted_candidates:
        char_images = [char for char, _ in sorted_candidates]
        titles = [f"Char {i}" for i in range(len(char_images))]
        plot_images(char_images, titles, figsize=(12, 4), rows=1)
    
    for index, (char, _) in enumerate(sorted_candidates):
        filename = f"{save_dir}/{file_prefix}_char_{index:02d}.png"
        cv2.imwrite(filename, char)
        print(f"Saved {filename}")

def convert_to_square(image, pad_color=255):
    """Convert non-square images to square by padding with a specified color (default: white)"""
    # Lấy kích thước ảnh
    if len(image.shape) == 3:  # Ảnh màu (h, w, c)
        img_h, img_w, channels = image.shape
    else:  # Ảnh xám (h, w)
        img_h, img_w = image.shape
        channels = 1
    
    if img_h > img_w:
        diff = img_h - img_w
        if channels == 1:  # Ảnh xám
            x1 = np.full((img_h, diff // 2), pad_color, dtype=image.dtype)
            x2 = np.full((img_h, diff // 2 + diff % 2), pad_color, dtype=image.dtype)
        else:  # Ảnh màu
            x1 = np.full((img_h, diff // 2, channels), pad_color, dtype=image.dtype)
            x2 = np.full((img_h, diff // 2 + diff % 2, channels), pad_color, dtype=image.dtype)
        return np.concatenate((x1, image, x2), axis=1)
    
    elif img_w > img_h:
        diff = img_w - img_h
        if channels == 1:  # Ảnh xám
            x1 = np.full((diff // 2, img_w), pad_color, dtype=image.dtype)
            x2 = np.full((diff // 2 + diff % 2, img_w), pad_color, dtype=image.dtype)
        else:  # Ảnh màu
            x1 = np.full((diff // 2, img_w, channels), pad_color, dtype=image.dtype)
            x2 = np.full((diff // 2 + diff % 2, img_w, channels), pad_color, dtype=image.dtype)
        return np.concatenate((x1, image, x2), axis=0)
    
    return image

def preprocess_license_plate(license_plate):
    """Preprocess license plate image for character segmentation"""
    # Tạo bản sao để hiển thị
    images = []
    titles = []
    
    # Thêm ảnh gốc
    images.append(license_plate.copy())
    titles.append("Ảnh biển số gốc")
    
    # Chuyển sang không gian màu HSV
    hsv = cv2.cvtColor(license_plate, cv2.COLOR_BGR2HSV)
    images.append(cv2.cvtColor(hsv, cv2.COLOR_HSV2BGR))  # Chuyển về BGR để hiển thị
    titles.append("Ảnh HSV")
    
    # Trích xuất kênh V
    h, s, v = cv2.split(hsv)
    images.append(v)
    titles.append("Kênh V (Value)")
    
    # Áp dụng ngưỡng cục bộ
    T = threshold_local(v, 15, offset=10, method="gaussian")
    thresh = (v > T).astype("uint8") * 255
    images.append(thresh)
    titles.append("Sau khi áp dụng ngưỡng")
    
    # Hiển thị các bước tiền xử lý
    plot_images(images, titles, figsize=(20, 5), rows=1)
    
    return thresh

def extract_and_sort_characters(thresh, license_plate):
    """Extract and sort characters based on connected components analysis"""
    # Tạo bản sao để hiển thị
    images = []
    titles = []
    
    # Thêm ảnh ngưỡng đầu vào
    images.append(thresh.copy())
    titles.append("Ảnh nhị phân đầu vào")
    
    # Thêm ảnh sau khi đảo ngược
    thresh_inv = cv2.bitwise_not(thresh)
    images.append(thresh_inv)
    titles.append("Ảnh đảo ngược")
    
    # Thêm ảnh sau khi điều chỉnh kích thước
    thresh_resized = imutils.resize(thresh_inv, width=400)
    images.append(thresh_resized)
    titles.append("Điều chỉnh kích thước")
    
    # Thêm ảnh sau khi lọc trung vị
    thresh_blurred = cv2.medianBlur(thresh_resized, 5)
    images.append(thresh_blurred)
    titles.append("Sau khi lọc trung vị")
    
    # Hiển thị các bước xử lý
    plot_images(images, titles, figsize=(20, 5), rows=1)
    
    # Tiếp tục với phân tích thành phần kết nối
    labels = measure.label(thresh_blurred, connectivity=2, background=0)
    
    # Tạo ảnh hiển thị các nhãn
    label_image = np.zeros_like(labels, dtype=np.uint8)
    for label_id in np.unique(labels):
        if label_id == 0:  # Bỏ qua nền
            continue
        label_image[labels == label_id] = np.random.randint(50, 255)
    
    images = [thresh_blurred, label_image]
    titles = ["Ảnh nhị phân", "Các thành phần kết nối"]
    plot_images(images, titles, figsize=(15, 7), rows=1)

    candidates = []
    candidate_rects = license_plate.copy()  # Ảnh để vẽ các hình chữ nhật bao quanh ký tự

    # Tính tỷ lệ scale giữa ảnh gốc và ảnh đã resize
    scale_y = license_plate.shape[0] / thresh_blurred.shape[0]
    scale_x = license_plate.shape[1] / thresh_blurred.shape[1]

    for label in np.unique(labels):
        if label == 0:
            continue
        mask = np.zeros(thresh_blurred.shape, dtype="uint8")
        mask[labels == label] = 255
        contours, _ = cv2.findContours(mask, cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_SIMPLE)
        
        if contours:
            contour = max(contours, key=cv2.contourArea)
            x, y, w, h = cv2.boundingRect(contour)
            aspect_ratio = w / float(h)
            solidity = cv2.contourArea(contour) / float(w * h)
            height_ratio = h / float(license_plate.shape[0])

            if 0.1 < aspect_ratio < 1.0 and solidity > 0.1 and 0.35 < height_ratio < 2.0:
            # if (0.15 < aspect_ratio < 0.95 and  # Tỷ lệ khung hình chặt chẽ hơn
            #     solidity > 0.225 and            # Độ đặc cao hơn
            #     0.35 < height_ratio < 2):    # Tỷ lệ chiều cao điều chỉnh
                # Tính toán tọa độ trên ảnh gốc
                x_orig = int(x * scale_x)
                y_orig = int(y * scale_y)
                w_orig = int(w * scale_x)
                h_orig = int(h * scale_y)

                # Vẽ hình chữ nhật quanh ký tự phát hiện được
                cv2.rectangle(candidate_rects, (x_orig, y_orig), 
                            (x_orig + w_orig, y_orig + h_orig), (0, 255, 0), 2)
                
                # Cắt ký tự từ ảnh gốc
                char_orig = license_plate[y_orig:y_orig + h_orig, x_orig:x_orig + w_orig]
                
                # Chuyển về hình vuông và resize
                square_candidate = convert_to_square(char_orig)
                square_candidate = cv2.resize(square_candidate, (28, 28), cv2.INTER_AREA)
                candidates.append((square_candidate, (y, x)))  # Giữ y, x gốc để sắp xếp

    # Hiển thị các ký tự được phát hiện
    plt.figure(figsize=(12, 8))
    plt.imshow(cv2.cvtColor(candidate_rects, cv2.COLOR_BGR2RGB))
    plt.title("Các ký tự được phát hiện")
    plt.axis('off')
    plt.show()

    # Sắp xếp các ký tự
    candidates = sorted(candidates, key=lambda x: x[1][0])
    mid_y = np.median([pos[0] for _, pos in candidates])
    top_row = [c for c in candidates if c[1][0] < mid_y]
    bottom_row = [c for c in candidates if c[1][0] >= mid_y]

    top_row = sorted(top_row, key=lambda x: x[1][1])
    bottom_row = sorted(bottom_row, key=lambda x: x[1][1])

    return top_row + bottom_row

def process_image_for_license_plate(image_path):
    """Process image for license plate detection and character recognition"""
    img = cv2.imread(image_path)
    if img is None:
        print(f"⚠️ Không thể đọc ảnh: {image_path}")
        return
    
    # Hiển thị ảnh đầu vào
    plt.figure(figsize=(12, 8))
    plt.imshow(cv2.cvtColor(img, cv2.COLOR_BGR2RGB))
    plt.title("Ảnh đầu vào")
    plt.axis('off')
    plt.show()
    
    print(f"Đang xử lý ảnh: {image_path}")
    results = yolo_model(image_path)
    
    # Vẽ các kết quả phát hiện biển số lên ảnh gốc
    result_img = img.copy()
    
    for i, result in enumerate(results):
        for j, box in enumerate(result.boxes):
            x1, y1, x2, y2 = map(int, box.xyxy[0])
            # Vẽ hình chữ nhật quanh biển số
            cv2.rectangle(result_img, (x1, y1), (x2, y2), (0, 255, 0), 2)
            # Hiển thị thông tin confidence
            conf = float(box.conf[0])
            cv2.putText(result_img, f"LP: {conf:.2f}", (x1, y1-10), cv2.FONT_HERSHEY_SIMPLEX, 0.9, (0, 255, 0), 2)
    
    # Hiển thị ảnh có các biển số được phát hiện
    plt.figure(figsize=(12, 8))
    plt.imshow(cv2.cvtColor(result_img, cv2.COLOR_BGR2RGB))
    plt.title("Phát hiện biển số")
    plt.axis('off')
    plt.show()

    for i, result in enumerate(results):
        for j, box in enumerate(result.boxes):
            x1, y1, x2, y2 = map(int, box.xyxy[0])
            license_plate = img[y1:y2, x1:x2]
            
            if license_plate.size == 0:
                print(f"⚠️ Không thể cắt vùng biển số {j}")
                continue

            cropped_path = os.path.join(output_dir, f"license_plate_{i}_{j}.jpg")
            cv2.imwrite(cropped_path, license_plate)
            print(f"Đã lưu biển số tại: {cropped_path}")

            # Xử lý sơ bộ ảnh biển số
            thresh = preprocess_license_plate(license_plate)
            
            # Các bước xử lý tiếp theo đã được chuyển vào hàm extract_and_sort_characters
            # để hiển thị từng bước
            candidates = extract_and_sort_characters(thresh, license_plate)
            save_characters(candidates, image_path)

# Danh sách các tệp ảnh trong thư mục
image_folder = r"./Dataset"
image_files = sorted([f for f in os.listdir(image_folder) if f.endswith(('.png', '.jpg'))])

for image_file in image_files:
    image_path = os.path.join(image_folder, image_file) 
    process_image_for_license_plate(image_path)
    
    # Thêm dòng này để tạm dừng sau mỗi ảnh
    if input("Nhấn Enter để tiếp tục, nhập 'q' để thoát: ").lower() == 'q':
        break


# image_path = r"D:\Documents\Work\NganGiang\HAQUANGDUNG\Job74\GreenParking\0000_06886_b.jpg"
# process_image_for_license_plate(image_path)