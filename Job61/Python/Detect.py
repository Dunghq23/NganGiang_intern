import os
import shutil
from ultralytics import YOLO

# Đường dẫn đến mô hình và thư mục chứa ảnh
model_path = r'D:\Documents\Work\NganGiang\HAQUANGDUNG\Job61\Python\Box_Lid_Model_2.pt'  
# images_dir = r'D:\Documents\Work\NganGiang\HAQUANGDUNG\Job61\LidClassification\Resource\CapturedImages'
# output_dir = r'D:\Documents\Work\NganGiang\HAQUANGDUNG\Job61\Python\Output'
# runs_dir = r'runs'  # Thư mục mặc định YOLO tạo ra
# prediction_dir = r'runs\detect\predict'  # Thư mục chứa ảnh đã dự đoán

# Load mô hình
model = YOLO(model_path)
model.overrides['verbose'] = False


# # Tạo thư mục lưu kết quả nếu chưa tồn tại
# os.makedirs(output_dir, exist_ok=True)

# # Duyệt qua tất cả các ảnh trong thư mục
# for image_name in os.listdir(images_dir):
#     image_path = os.path.join(images_dir, image_name)
#     if os.path.isfile(image_path):  # Chỉ xử lý file, không xử lý thư mục con
#         # Dự đoán 
#         results = model(image_path, save=True)

# # Sao chép toàn bộ ảnh từ prediction sang output
# if os.path.exists(prediction_dir):
#     for file_name in os.listdir(prediction_dir):
#         src_file = os.path.join(prediction_dir, file_name)
#         dest_file = os.path.join(output_dir, file_name)
#         shutil.copy(src_file, dest_file)
#     print(f"Tất cả ảnh đã được sao chép sang {output_dir}")
# else:
#     print(f"Thư mục {prediction_dir} không tồn tại")

# # Xóa thư mục runs nếu cần
# try:
#     shutil.rmtree(runs_dir)
#     print(f"Đã xóa thư mục {runs_dir}")
# except Exception as e:
#     print(f"Lỗi khi xóa thư mục {runs_dir}: {e}")


# Hàm phát hiện đối tượng
def detect_objects(image):
    results = model(image)
    detections = []
    for result in results:
        for box in result.boxes:
            x1, y1, x2, y2 = box.xyxy[0].cpu().numpy()
            confidence = float(box.conf[0].cpu().numpy())
            class_id = int(box.cls[0].cpu().numpy())
            label = model.names[class_id]

            detections.append({
                "x": int(x1),
                "y": int(y1),
                "w": int(x2 - x1),
                "h": int(y2 - y1),
                "label": label,
                "confidence": round(confidence, 2)
            })
    return detections


if "__main__":
    print(detect_objects("Data\With_Lid\Image_0000.jpg"))