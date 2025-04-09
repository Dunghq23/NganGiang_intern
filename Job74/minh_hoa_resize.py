import os
import cv2
import numpy as np
from skimage.filters import threshold_local
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


def save_characters(sorted_candidates, original_filename, save_dir="output_chars"):
    """Save character images to specified directory"""
    if not os.path.exists(save_dir):
        os.makedirs(save_dir)
    
    file_prefix = os.path.splitext(os.path.basename(original_filename))[0]
    
    # Hiển thị các ký tự đã tách được
    if sorted_candidates:
        char_images = [char for char, _ in sorted_candidates]
        titles = [f"Char {i}" for i in range(len(char_images))]
        # plot buoc cuoi cung cac ky tu da tach duoc
        plot_images(char_images, titles, figsize=(12, 4), rows=1)
    
    for index, (char, _) in enumerate(sorted_candidates):
        filename = f"{save_dir}/{file_prefix}_char_{index:02d}.png"
        cv2.imwrite(filename, char)
        print(f"Saved {filename}")

def convert_to_square(image):
    """Convert non-square images to square by padding with zeros"""
    # Lấy kích thước ảnh
    if len(image.shape) == 3:  # Ảnh màu (h, w, c)
        img_h, img_w, channels = image.shape
    else:  # Ảnh xám (h, w)
        img_h, img_w = image.shape
        channels = 1
    
    if img_h > img_w:
        diff = img_h - img_w
        if channels == 1:  # Ảnh xám
            x1 = np.zeros((img_h, diff // 2), dtype=image.dtype)
            x2 = np.zeros((img_h, diff // 2 + diff % 2), dtype=image.dtype)
        else:  # Ảnh màu
            x1 = np.zeros((img_h, diff // 2, channels), dtype=image.dtype)
            x2 = np.zeros((img_h, diff // 2 + diff % 2, channels), dtype=image.dtype)
        return np.concatenate((x1, image, x2), axis=1)
    
    elif img_w > img_h:
        diff = img_w - img_h
        if channels == 1:  # Ảnh xám
            x1 = np.zeros((diff // 2, img_w), dtype=image.dtype)
            x2 = np.zeros((diff // 2 + diff % 2, img_w), dtype=image.dtype)
        else:  # Ảnh màu
            x1 = np.zeros((diff // 2, img_w, channels), dtype=image.dtype)
            x2 = np.zeros((diff // 2 + diff % 2, img_w, channels), dtype=image.dtype)
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
    # Tạo bản sao để hiển thị các bước xử lý
    images = []
    titles = []
    
    # Thêm ảnh ngưỡng đầu vào
    images.append(thresh.copy())
    titles.append("Ảnh nhị phân đầu vào")
    
    # Đảo ngược ảnh nhị phân
    thresh_inv = cv2.bitwise_not(thresh)
    images.append(thresh_inv)
    titles.append("Ảnh đảo ngược")
    
    # Resize ảnh gốc và ảnh nhị phân về cùng kích thước để đồng nhất
    STANDARD_WIDTH = 400
    scale = STANDARD_WIDTH / thresh.shape[1]
    standard_height = int(thresh.shape[0] * scale)
    
    # Resize cả ảnh gốc và ảnh nhị phân
    license_plate_resized = cv2.resize(license_plate, (STANDARD_WIDTH, standard_height))
    thresh_resized = cv2.resize(thresh_inv, (STANDARD_WIDTH, standard_height))
    images.append(thresh_resized)
    titles.append("Ảnh đảo ngược (resize)")
    
    # Áp dụng bộ lọc trung vị
    thresh_blurred = cv2.medianBlur(thresh_resized, 5)
    images.append(thresh_blurred)
    titles.append("Sau khi lọc trung vị")
    
    # Hiển thị các bước xử lý
    plot_images(images, titles, figsize=(20, 5), rows=1)

    # Các tham số tỷ lệ now được tính trên cùng một không gian
    # min_height_ratio = 0.3
    # max_height_ratio = 0.8
    # min_width_ratio = 0.03
    # max_width_ratio = 0.15
    
    min_height_ratio = 0.15  # Giảm xuống để bắt nhiều ký tự hơn
    max_height_ratio = 0.9  # Tăng lên để không bỏ sót
    min_width_ratio = 0.02  # Giảm xuống để bắt các ký tự nhỏ
    max_width_ratio = 0.15   # Tăng lên để bao gồm nhiều kích thước hơn

    candidates = []
    candidate_rects = license_plate_resized.copy()
    candidate_masks = np.zeros_like(thresh_blurred)
    
    # Phân tích thành phần kết nối
    labels = measure.label(thresh_blurred, connectivity=2, background=0)
    
    # Tạo ảnh hiển thị các nhãn
    label_image = np.zeros_like(labels, dtype=np.uint8)
    for label_id in np.unique(labels):
        if label_id == 0:  # Bỏ qua nền
            continue
        label_image[labels == label_id] = np.random.randint(50, 255)
    
    # Hiển thị ảnh nhị phân và ảnh các thành phần kết nối
    components_images = [thresh_blurred, label_image]
    components_titles = ["Ảnh nhị phân sau lọc", "Các thành phần kết nối"]
    plot_images(components_images, components_titles, figsize=(15, 7), rows=1)
    
    # Lấy tất cả contour và lưu thông tin
    all_contours = []
    all_contour_img = license_plate_resized.copy()
    
    for label in np.unique(labels):
        if label == 0:
            continue
        mask = np.zeros(thresh_blurred.shape, dtype="uint8")
        mask[labels == label] = 255
        contours, _ = cv2.findContours(mask, cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_SIMPLE)
        
        if contours:
            contour = max(contours, key=cv2.contourArea)
            x, y, w, h = cv2.boundingRect(contour)
            
            # Vẽ tất cả contour lên ảnh để kiểm tra
            cv2.rectangle(all_contour_img, (x, y), (x + w, y + h), (0, 0, 255), 1)
            
            # Tính các tỷ lệ
            width_ratio = w / STANDARD_WIDTH
            height_ratio = h / standard_height
            aspect_ratio = w / float(h)
            solidity = cv2.contourArea(contour) / float(w * h)
            
            # Lưu thông tin contour
            all_contours.append({
                'contour': contour,
                'x': x, 'y': y, 'w': w, 'h': h,
                'width_ratio': width_ratio,
                'height_ratio': height_ratio,
                'aspect_ratio': aspect_ratio,
                'solidity': solidity
            })
    
    # Hiển thị tất cả contour tìm thấy
    plt.figure(figsize=(12, 8))
    plt.imshow(cv2.cvtColor(all_contour_img, cv2.COLOR_BGR2RGB))
    plt.title("Tất cả contour tìm thấy")
    plt.axis('off')
    plt.show()
    
    # Lọc contour theo các tiêu chí
    valid_contour_img = license_plate_resized.copy()
    valid_mask = np.zeros_like(thresh_blurred)
    
    for contour_info in all_contours:
        x, y, w, h = contour_info['x'], contour_info['y'], contour_info['w'], contour_info['h']
        width_ratio = contour_info['width_ratio']
        height_ratio = contour_info['height_ratio']
        aspect_ratio = contour_info['aspect_ratio']
        solidity = contour_info['solidity']
        # if True:
        if (True 
            and
            # min_width_ratio < width_ratio < max_width_ratio and
            # min_height_ratio < height_ratio < max_height_ratio and
            0.1 < aspect_ratio < 2.5 and solidity > 0.1
            ):
            
            # Vẽ hình chữ nhật quanh ký tự hợp lệ
            cv2.rectangle(valid_contour_img, (x, y), (x + w, y + h), (0, 255, 0), 2)
            
            # Vẽ mask cho ký tự hợp lệ
            mask_roi = valid_mask[y:y+h, x:x+w]
            cv2.drawContours(mask_roi, [contour_info['contour'] - np.array([x, y])], 0, 255, -1)
            
            # Cắt ký tự từ ảnh đã resize
            char_resized = license_plate_resized[y:y + h, x:x + w]
            square_candidate = convert_to_square(char_resized)
            square_candidate = cv2.resize(square_candidate, (28, 28))
            
            # Giữ cấu trúc tuple như cũ
            candidates.append((square_candidate, (y, x)))
    
    # Hiển thị contour hợp lệ và mask
    valid_images = [valid_contour_img, valid_mask]
    valid_titles = ["Các ký tự hợp lệ", "Mask các ký tự"]
    plot_images(valid_images, valid_titles, figsize=(15, 7), rows=1)

    # Hiển thị các ký tự được phát hiện
    plt.figure(figsize=(12, 8))
    plt.imshow(cv2.cvtColor(valid_contour_img, cv2.COLOR_BGR2RGB))
    plt.title("Các ký tự được phát hiện")
    plt.axis('off')
    plt.show()
    
    if not candidates:
        print("Không tìm thấy ký tự nào! Thử với các tham số khác...")
        # Thử lại với các tham số khác nếu không tìm thấy ký tự nào
        for contour_info in all_contours:
            x, y, w, h = contour_info['x'], contour_info['y'], contour_info['w'], contour_info['h']
            width_ratio = contour_info['width_ratio']
            height_ratio = contour_info['height_ratio']
            aspect_ratio = contour_info['aspect_ratio']
            solidity = contour_info['solidity']
            
            # Các tham số lọc rộng hơn
            if ((0.02 < width_ratio < 0.2) and 
                (0.25 < height_ratio < 0.85) and 
                (0.05 < aspect_ratio < 1.2) and 
                (solidity > 0.08)):
                
                # Vẽ hình chữ nhật quanh ký tự hợp lệ mở rộng
                cv2.rectangle(candidate_rects, (x, y), (x + w, y + h), (255, 0, 0), 2)
                
                # Cắt ký tự từ ảnh đã resize
                char_resized = license_plate_resized[y:y + h, x:x + w]
                square_candidate = convert_to_square(char_resized)
                square_candidate = cv2.resize(square_candidate, (28, 28))
                
                # Giữ cấu trúc tuple như cũ
                candidates.append((square_candidate, (y, x)))
                
        # Hiển thị các ký tự được phát hiện sau khi nới lỏng tham số
        plt.figure(figsize=(12, 8))
        plt.imshow(cv2.cvtColor(candidate_rects, cv2.COLOR_BGR2RGB))
        plt.title("Các ký tự được phát hiện (sau khi nới lỏng tham số)")
        plt.axis('off')
        plt.show()

    # Sắp xếp các ký tự
    # Tạo ảnh hiển thị quá trình sắp xếp
    sorting_img = license_plate_resized.copy()
    
    # Sắp xếp trước theo y (hàng)
    candidates = sorted(candidates, key=lambda x: x[1][0])
    
    # Tìm điểm trung bình y để phân biệt hàng trên và hàng dưới
    if candidates:
        mid_y = np.median([pos[0] for _, pos in candidates])
        
        # Vẽ đường phân cách giữa hàng trên và hàng dưới
        cv2.line(sorting_img, (0, int(mid_y)), (STANDARD_WIDTH, int(mid_y)), (0, 255, 255), 2)
        
        # Chia thành hàng trên và hàng dưới
        top_row = [c for c in candidates if c[1][0] < mid_y]
        bottom_row = [c for c in candidates if c[1][0] >= mid_y]

        # Sắp xếp theo x (từ trái sang phải) trong mỗi hàng
        top_row = sorted(top_row, key=lambda x: x[1][1])
        bottom_row = sorted(bottom_row, key=lambda x: x[1][1])
        
        # Vẽ số thứ tự cho hàng trên
        for i, (_, (y, x)) in enumerate(top_row):
            cv2.putText(sorting_img, str(i), (x, y), cv2.FONT_HERSHEY_SIMPLEX, 0.7, (255, 0, 0), 2)
        
        # Vẽ số thứ tự cho hàng dưới
        for i, (_, (y, x)) in enumerate(bottom_row):
            cv2.putText(sorting_img, str(i), (x, y), cv2.FONT_HERSHEY_SIMPLEX, 0.7, (0, 0, 255), 2)
        
        # Hiển thị quá trình sắp xếp
        plt.figure(figsize=(12, 8))
        plt.imshow(cv2.cvtColor(sorting_img, cv2.COLOR_BGR2RGB))
        plt.title("Quá trình sắp xếp ký tự (đỏ: hàng dưới, xanh: hàng trên)")
        plt.axis('off')
        plt.show()
        
        return top_row + bottom_row
    else:
        print("Không phát hiện được ký tự nào sau tất cả các bước xử lý!")
        return []

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



# Load YOLO model for license plate detection
yolo_model = YOLO("./License-plate-detection.pt")

# Directory paths
input_folder = "./output_chars"
output_dir = r"./Cropped"
# Create output directories if they don't exist
os.makedirs(output_dir, exist_ok=True)

# Danh sách các tệp ảnh trong thư mục
image_folder = r"./Dataset"
image_files = sorted([f for f in os.listdir(image_folder) if f.endswith(('.png', '.jpg'))])

# Xử lý từng ảnh
for image_file in image_files:
    image_path = os.path.join(image_folder, image_file) 
    process_image_for_license_plate(image_path)
    
    # Thêm dòng này để tạm dừng sau mỗi ảnh
    if input("Nhấn Enter để tiếp tục, nhập 'q' để thoát: ").lower() == 'q':
        break