from ultralytics import YOLO
import socket
import cv2
import numpy as np
import json

# Tải mô hình YOLO (tải một lần để tiết kiệm tài nguyên)
model = YOLO('./Model/yolov8n.pt')

# Hàm phát hiện đối tượng
def detect_objects(image):
    """
    Phát hiện đối tượng trong ảnh sử dụng YOLO.

    Args:
        image (numpy.ndarray): Ảnh đầu vào.

    Returns:
        List[Dict]: Danh sách đối tượng được phát hiện, mỗi đối tượng có định dạng:
            {"x": int, "y": int, "w": int, "h": int, "label": str, "confidence": float}.
    """
    # Thực hiện dự đoán
    results = model(image)

    # Lưu các kết quả phát hiện
    detections = []

    for result in results:
        for box in result.boxes:
            x1, y1, x2, y2 = box.xyxy[0].cpu().numpy()  # Góc trên trái và dưới phải của bounding box
            confidence = float(box.conf[0].cpu().numpy())  # Độ chính xác của dự đoán
            class_id = int(box.cls[0].cpu().numpy())  # ID của lớp
            label = model.names[class_id]  # Tên lớp đối tượng

            # Tính toán width và height từ bounding box
            w = int(x2 - x1)
            h = int(y2 - y1)

            # Lưu thông tin bounding box vào danh sách kết quả
            detections.append({
                "x": int(x1),
                "y": int(y1),
                "w": w,
                "h": h,
                "label": label,
                "confidence": round(confidence, 2)
            })

    return detections

# Khởi tạo socket server
def start_server(host='localhost', port=5000):
    """
    Khởi động server để nhận ảnh qua socket và trả về bounding box.

    Args:
        host (str): Địa chỉ host. Mặc định là 'localhost'.
        port (int): Cổng của server. Mặc định là 5000.
    """
    server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    server_socket.bind((host, port))
    server_socket.listen(1)
    print(f"Server đang chạy trên {host}:{port}...")

    while True:
        conn, addr = server_socket.accept()
        print(f"Kết nối từ: {addr}")

        try:
            # Nhận kích thước ảnh
            img_size = int(conn.recv(16).decode())
            print(f"Kích thước ảnh nhận được: {img_size}")

            # Nhận dữ liệu ảnh
            img_data = b''
            while len(img_data) < img_size:
                packet = conn.recv(4096)
                if not packet:
                    break
                img_data += packet

            # Decode ảnh
            np_data = np.frombuffer(img_data, np.uint8)
            image = cv2.imdecode(np_data, cv2.IMREAD_COLOR)
            if image is None:
                raise ValueError("Không thể decode ảnh nhận được.")

            print("Ảnh đã nhận thành công.")

            # Phát hiện đối tượng
            bounding_boxes = detect_objects(image)

            # Gửi bounding box lại dưới dạng JSON
            result = json.dumps(bounding_boxes).encode()
            conn.sendall(result)
            print("Kết quả đã gửi.")
        except Exception as e:
            print(f"Lỗi: {e}")
        finally:
            conn.close()

if __name__ == "__main__":
    try:
        start_server()
    except KeyboardInterrupt:
        print("Server đã dừng.")
