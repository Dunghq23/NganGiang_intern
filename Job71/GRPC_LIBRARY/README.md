# Object Detection Service

## Giới thiệu
Thư viện cung cấp dịch vụ phát hiện đối tượng sử dụng YOLO qua gRPC.

## Cài đặt
```bash
pip install object_detection_service
```

## Sử dụng cơ bản
```python
from yolo-grpc import ObjectDetector
import cv2

# Khởi tạo detector
detector = ObjectDetector('path_to_model.pt')

# Đọc ảnh
image = cv2.imread('image.jpg')

# Phát hiện đối tượng
results = detector.detect_objects(image)
print(results)
```

## Chạy Server gRPC
```python
from object_detection_service import serve
```

### Khởi động server
```python
serve('path_to_model.pt', port=50051)
```