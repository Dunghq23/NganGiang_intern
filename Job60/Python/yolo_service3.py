import grpc
import os
import json
from concurrent import futures
import image_service_pb2
import image_service_pb2_grpc
import cv2
from ultralytics import YOLO
import time
import win32serviceutil
import win32service
import win32event
import win32api
from threading import Thread

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

class ImageTransferServicer(image_service_pb2_grpc.ImageTransferServicer):
    def SendImage(self, request, context):
        try:
            image_path = request.path
            
            # Kiểm tra file tồn tại
            if not os.path.exists(image_path):
                return image_service_pb2.ImageResponse(
                    success=False, 
                    message=f"File not found: {image_path}"
                )
            
            # Đọc ảnh
            image = cv2.imread(image_path)
            
            if image is None:
                return image_service_pb2.ImageResponse(
                    success=False, 
                    message=f"Cannot read image: {image_path}"
                )
            
            # Phát hiện đối tượng
            detections = detect_objects(image)
            
            # Chuyển danh sách phát hiện đối tượng thành chuỗi JSON
            detections_json = json.dumps(detections)
            
            return image_service_pb2.ImageResponse(
                success=True, 
                message="Detection successful",
                data=detections_json  # Trả về dữ liệu JSON
            )
        
        except Exception as e:
            return image_service_pb2.ImageResponse(
                success=False, 
                message=str(e)
            )

class GRPCServerService(win32serviceutil.ServiceFramework):
    _svc_name_ = "Yolo service - gRPCImageDetectionService"
    _svc_display_name_ = "Yolo service - gRPC Image Detection Service"
    _svc_description_ = "Service that runs a gRPC server for image detection using YOLO"

    def __init__(self, args):
        win32serviceutil.ServiceFramework.__init__(self, args)
        self._stop_event = win32event.CreateEvent(None, 0, 0, None)

    def SvcStop(self):
        self.ReportServiceStatus(win32service.SERVICE_STOP_PENDING)
        win32event.SetEvent(self._stop_event)

    def SvcDoRun(self):
        # Khởi tạo gRPC server trong một thread riêng để tránh blocking
        grpc_thread = Thread(target=self._run_grpc_server)
        grpc_thread.start()
        win32event.WaitForSingleObject(self._stop_event, win32event.INFINITE)

    def _run_grpc_server(self):
        # Tạo server gRPC
        server = grpc.server(futures.ThreadPoolExecutor(max_workers=10))
        
        # Đăng ký service
        image_service_pb2_grpc.add_ImageTransferServicer_to_server(
            ImageTransferServicer(), server)
        
        # Lắng nghe trên port 50051
        server.add_insecure_port('[::]:50051')
        server.start()
        
        print("gRPC server started, listening on port 50051")
        
        while not self._stop_event.is_set():
            time.sleep(1)

        server.stop(0)
        print("gRPC server stopped")

if __name__ == '__main__':
    win32serviceutil.HandleCommandLine(GRPCServerService)