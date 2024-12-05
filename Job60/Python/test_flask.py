import sys
import os
import time
import json
from flask import Flask, request, jsonify
import cv2
import numpy as np
from ultralytics import YOLO
from threading import Thread

# Load YOLO model
model = YOLO('./Model/yolov8n.pt')

# Create Flask app
app = Flask(__name__)

# Object detection function
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

# API endpoint for object detection
@app.route('/detect', methods=['POST'])
def detect():
    try:
        # Lấy thời gian nhận yêu cầu
        receive_time = time.time()  # Thời gian nhận (tính bằng giây từ epoch)

        # Lấy đường dẫn ảnh từ yêu cầu
        image_path = request.json.get('image_path')
        if not image_path:
            return jsonify({"error": "No image path provided"}), 400

        # Kiểm tra ảnh tồn tại
        if not os.path.exists(image_path):
            return jsonify({"error": "Image path does not exist"}), 400

        # Bắt đầu đo thời gian xử lý
        start_time = time.time()

        # Phát hiện đối tượng
        detections = detect_objects(image_path)

        # Kết thúc đo thời gian xử lý
        end_time = time.time()

        # Tính toán các mốc thời gian
        process_time = end_time - start_time  # Thời gian xử lý
        receive_timestamp = time.strftime('%Y-%m-%d %H:%M:%S', time.gmtime(receive_time))
        receive_timestamp += f".{int((receive_time - int(receive_time)) * 1000):03d}"  # Thêm mili giây

        # Trả về kết quả dưới dạng JSON
        response = {
            "detections": detections,
            "receive_time": receive_timestamp,
            "process_time": process_time  # Không làm tròn
        }
        return jsonify(response)

    except Exception as e:
        return jsonify({"error": str(e)}), 500

# Run Flask app
if __name__ == "__main__":
    app.run(host='0.0.0.0', port=5000)
