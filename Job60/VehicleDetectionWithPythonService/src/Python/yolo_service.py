import socket
import cv2
import numpy as np
import json
import win32serviceutil
import win32service
import win32event
import servicemanager
import threading
from ultralytics import YOLO

# Tải mô hình YOLO
model = YOLO('./Model/yolov8n.pt')

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

# Socket server xử lý ảnh
def socket_server(host='localhost', port=5000):
    server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    server_socket.bind((host, port))
    server_socket.listen(1)
    print(f"Service is running on {host}:{port}...")

    while True:
        conn, addr = server_socket.accept()
        try:
            img_size = int(conn.recv(16).decode())
            img_data = b''
            while len(img_data) < img_size:
                packet = conn.recv(4096)
                if not packet:
                    break
                img_data += packet

            np_data = np.frombuffer(img_data, np.uint8)
            image = cv2.imdecode(np_data, cv2.IMREAD_COLOR)
            if image is None:
                raise ValueError("Cannot decode the received image.")

            bounding_boxes = detect_objects(image)
            result = json.dumps(bounding_boxes).encode()
            conn.sendall(result)
        except Exception as e:
            print(f"Error: {e}")
        finally:
            conn.close()
# Windows Service class
class YOLOWindowsService(win32serviceutil.ServiceFramework):
    _svc_name_ = "YOLOService"
    _svc_display_name_ = "YOLO Object Detection Service"
    _svc_description_ = "Một dịch vụ dựa trên Python để phát hiện đối tượng bằng cách sử dụng YOLO và giao tiếp socket."
    
    def __init__(self, args):
        win32serviceutil.ServiceFramework.__init__(self, args)
        self.hWaitStop = win32event.CreateEvent(None, 0, 0, None)
        self.server_thread = threading.Thread(target=socket_server)

    def SvcDoRun(self):
        servicemanager.LogMsg(servicemanager.EVENTLOG_INFORMATION_TYPE,
                              servicemanager.PYS_SERVICE_STARTED,
                              (self._svc_name_, ""))
        self.server_thread.start()
        win32event.WaitForSingleObject(self.hWaitStop, win32event.INFINITE)

    def SvcStop(self):
        self.ReportServiceStatus(win32service.SERVICE_STOP_PENDING)
        win32event.SetEvent(self.hWaitStop)
        self.server_thread.join()
        servicemanager.LogMsg(servicemanager.EVENTLOG_INFORMATION_TYPE,
                              servicemanager.PYS_SERVICE_STOPPED,
                              (self._svc_name_, ""))

if __name__ == "__main__":
    win32serviceutil.HandleCommandLine(YOLOWindowsService)
