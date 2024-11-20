import socket
import cv2
import numpy as np
import json

# Hàm phát hiện đối tượng giả lập
def detect_objects(image):
    # Ví dụ trả về bounding box giả lập: [(x, y, w, h, label)]
    return [{"x": 50, "y": 50, "w": 100, "h": 100, "label": "Object1"}]

# Khởi tạo socket server
def start_server(host='localhost', port=5000):
    server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    server_socket.bind((host, port))
    server_socket.listen(1)
    print("Server đang chạy...")
    
    while True:
        conn, addr = server_socket.accept()
        print(f"Kết nối từ: {addr}")
        
        # Nhận kích thước ảnh
        img_size = int(conn.recv(16).decode())
        print(f"Kích thước ảnh: {img_size}")
        
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
        print("Ảnh đã nhận thành công.")
        
        # Phát hiện đối tượng
        bounding_boxes = detect_objects(image)
        
        # Gửi bounding box lại C#
        result = json.dumps(bounding_boxes).encode()
        conn.sendall(result)
        print("Kết quả đã gửi.")
        conn.close()

if __name__ == "__main__":
    start_server()
