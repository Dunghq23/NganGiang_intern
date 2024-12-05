import sys
import os
import time
import json
import win32serviceutil
import win32service
import win32event
import win32api
from flask import Flask, request, jsonify
import cv2
import numpy as np
from ultralytics import YOLO
from threading import Thread

# Tải mô hình YOLO
model = YOLO('./Model/yolov8n.pt')

# Tạo ứng dụng Flask
app = Flask(__name__)

# Hàm phát hiện đối tượng
def detect_objects(image_path):
    image = cv2.imread(image_path)
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

# API endpoint nhận đường dẫn ảnh và trả về kết quả
@app.route('/detect', methods=['POST'])
def detect():
    try:
        # Lấy đường dẫn ảnh từ yêu cầu
        image_path = request.json.get('image_path')
        if not image_path:
            return jsonify({"error": "No image path provided"}), 400

        # Kiểm tra ảnh tồn tại
        if not os.path.exists(image_path):
            return jsonify({"error": "Image path does not exist"}), 400

        # Phát hiện đối tượng
        detections = detect_objects(image_path)

        # Trả về kết quả dưới dạng JSON
        return jsonify(detections)

    except Exception as e:
        return jsonify({"error": str(e)}), 500

# Service class để chạy Flask API dưới dạng Windows Service
class YoloService(win32serviceutil.ServiceFramework):
    _svc_name_ = "YoloService"
    _svc_display_name_ = "YOLO Object Detection Service"
    _svc_description_ = "A service to run YOLO object detection using Flask API"
    
    def __init__(self, args):
        win32serviceutil.ServiceFramework.__init__(self, args)
        self._stop_event = win32event.CreateEvent(None, 0, 0, None)
    
    def SvcStop(self):
        self.ReportServiceStatus(win32service.SERVICE_STOP_PENDING)
        win32event.SetEvent(self._stop_event)

    def SvcDoRun(self):
        # Khởi tạo Flask API trong một thread riêng để tránh blocking
        api_thread = Thread(target=self.start_flask)
        api_thread.start()
        win32event.WaitForSingleObject(self._stop_event, win32event.INFINITE)

    def start_flask(self):
        app.run(host='0.0.0.0', port=5000)

# Chạy service
if __name__ == "__main__":
    win32serviceutil.HandleCommandLine(YoloService)
