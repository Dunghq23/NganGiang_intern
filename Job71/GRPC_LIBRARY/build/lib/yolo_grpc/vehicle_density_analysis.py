import cv2
import numpy as np
from ultralytics import YOLO

def load_and_process_road_mask(processed_image_path):
    processed_image = cv2.imread(processed_image_path)
    if processed_image is None:
        raise FileNotFoundError(f"Không thể đọc ảnh: {processed_image_path}")

    gray_road_mask = cv2.cvtColor(processed_image, cv2.COLOR_BGR2GRAY)
    _, road_mask = cv2.threshold(gray_road_mask, 200, 255, cv2.THRESH_BINARY)

    return np.count_nonzero(road_mask == 255)

def detect_vehicles(original_image_path, model_path):
    model = YOLO(model_path)
    original_image = cv2.imread(original_image_path)
    if original_image is None:
        raise FileNotFoundError(f"Không thể đọc ảnh: {original_image_path}")

    results = model(original_image)

    vehicle_list = ["car", "truck", "motorcycle", "bicycle", "bus", "person"]
    sum_vehicle_area = 0

    for i in range(len(results[0].masks)):
        class_id = int(results[0].boxes.cls[i])
        if model.names[class_id] in vehicle_list:
            mask = results[0].masks.data[i]
            sum_vehicle_area += np.count_nonzero(mask)

    return sum_vehicle_area

def calculate_occupancy(road_area, vehicle_area):
    return (vehicle_area / road_area) * 100 if road_area > 0 else 0

def process_image_data(model_path, pathImageProcessed, pathImage):
    road_area = load_and_process_road_mask(pathImageProcessed)
    vehicle_area = detect_vehicles(pathImage, model_path)
    return calculate_occupancy(road_area, vehicle_area)
