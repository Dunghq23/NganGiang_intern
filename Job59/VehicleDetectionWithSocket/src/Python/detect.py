import os
import sys
import json
import time
import cv2
from collections import Counter
from ultralytics import YOLO

def detect_vehicles(image_path, model_path, output_path):
    # Tải mô hình YOLO
    model = YOLO(model_path)
    
    # Đọc ảnh
    image = cv2.imread(image_path)
    if image is None:
        print("Không thể mở ảnh")
        return None

    # Thiết lập kích thước mới cho ảnh
    new_width = 640
    new_height = 480
    resized_image = cv2.resize(image, (new_width, new_height))
    
    # Bắt đầu đo thời gian
    start_time = time.time()
    
    # Dự đoán bằng YOLO
    results = model(resized_image, verbose=False)
    result = results[0]
    
    # Đếm từng loại phương tiện
    vehicle_counts = Counter()
    for box in result.boxes:
        class_id = int(box.cls)
        class_name = result.names[class_id]
        vehicle_counts[class_name] += 1

    # Vẽ bounding box và số lượng phương tiện
    annotated_image = result.plot()
    cv2.putText(annotated_image, f'Total Vehicles: {sum(vehicle_counts.values())}', (10, 40),
                cv2.FONT_HERSHEY_SIMPLEX, 1, (0, 0, 255), 2)

    # Lưu ảnh đầu ra
    cv2.imwrite(output_path, annotated_image)
    
    # Tính tổng thời gian xử lý
    end_time = time.time()
    total_time = end_time - start_time

    # Chuẩn bị dữ liệu JSON
    data = {
        "total_time": total_time,
        "total_vehicles": sum(vehicle_counts.values()),
        "vehicle_counts": dict(vehicle_counts)
    }

    # Trả về JSON dưới dạng chuỗi để in ra cho C#
    return json.dumps(data, ensure_ascii=False)

if __name__ == "__main__":
    # Nhận các đối số từ dòng lệnh
    if len(sys.argv) < 2:
        print("Vui lòng cung cấp đường dẫn tới ảnh.")
        sys.exit(1)

    image_path = sys.argv[1]
    model_path = sys.argv[2]
    output_path = sys.argv[3]

    # Gọi hàm detect_vehicles và trả về kết quả JSON
    result_json = detect_vehicles(image_path, model_path, output_path)
    print(result_json)  # In JSON để C# có thể nhận được output
