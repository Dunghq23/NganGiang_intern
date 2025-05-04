import os
os.environ['TF_CPP_MIN_LOG_LEVEL'] = '3'  # Chỉ giữ lại log lỗi nghiêm trọng

import logging
logging.getLogger('tensorflow').setLevel(logging.FATAL)

import absl.logging
absl.logging.set_verbosity(absl.logging.ERROR)

import shutil
import cv2
import numpy as np
import tensorflow as tf
from tensorflow.keras.models import load_model


def nhan_dien_ky_tu(duong_dan_anh, model_path, img_width=64, img_height=64, grayscale=True):
    model = load_model(model_path)

    if grayscale:
        img = cv2.imread(duong_dan_anh, cv2.IMREAD_GRAYSCALE)
        img = cv2.resize(img, (img_width, img_height))
        img = np.expand_dims(img, axis=-1)
    else:
        img = cv2.imread(duong_dan_anh)
        img = cv2.cvtColor(img, cv2.COLOR_BGR2RGB)
        img = cv2.resize(img, (img_width, img_height))

    img = img.astype('float32') / 255.0
    img = np.expand_dims(img, axis=0)

    prediction = model.predict(img, verbose=0)
    predicted_class = np.argmax(prediction, axis=1)[0]
    probability = np.max(prediction)

    idx_to_label = {
        0: '0', 1: '1', 2: '2', 3: '3', 4: '4', 5: '5', 6: '6', 7: '7', 8: '8', 9: '9',
        10: 'A', 11: 'B', 12: 'C', 13: 'D', 14: 'E', 15: 'F', 16: 'G', 17: 'H',
        18: 'K', 19: 'L', 20: 'M', 21: 'N', 22: 'P', 23: 'R', 24: 'S', 25: 'T',
        26: 'U', 27: 'V', 28: 'X', 29: 'Y', 30: 'Z'
    }

    predicted_label = idx_to_label[predicted_class]

    return predicted_label, probability


def nhan_dien_va_phan_loai(thu_muc_anh, model_path, thu_muc_dich):
    for subdir, dirs, files in os.walk(thu_muc_anh):
        for file in files:
            if file.lower().endswith(('.png', '.jpg', '.jpeg', '.bmp')):
                full_path = os.path.join(subdir, file)
                ky_tu, do_tin_cay = nhan_dien_ky_tu(full_path, model_path)
                print(f"Ký tự: {ky_tu}, Độ tin cậy: {do_tin_cay*100:.2f}%")

                # if do_tin_cay < 0.7:
                #     thu_muc_nhohon70 = os.path.join(thu_muc_dich, "nhohon70")
                #     os.makedirs(thu_muc_nhohon70, exist_ok=True)
                #     file_moi = os.path.join(thu_muc_nhohon70, os.path.basename(full_path))
                #     shutil.move(full_path, file_moi)
                #     print(f"    --> Đã di chuyển vào {thu_muc_nhohon70}")
                #     continue
                # elif do_tin_cay < 0.9:
                #     thu_muc_nhohon90 = os.path.join(thu_muc_dich, "nhohon90")
                #     os.makedirs(thu_muc_nhohon90, exist_ok=True)
                #     file_moi = os.path.join(thu_muc_nhohon90, os.path.basename(full_path))
                #     shutil.move(full_path, file_moi)
                #     print(f"--> Đã di chuyển vào {thu_muc_nhohon90}")
                #     continue

                # Nếu ký tự là số từ 0 đến 9
                if ky_tu.isdigit() or ky_tu.isalpha():
                    thu_muc_con = os.path.join(thu_muc_dich, ky_tu)
                    os.makedirs(thu_muc_con, exist_ok=True)
                    file_moi = os.path.join(thu_muc_con, os.path.basename(full_path))
                    shutil.move(full_path, file_moi)
                    print(f"--> Đã di chuyển vào {thu_muc_con}")


model_path = "./model/model_nhan_dang_bien_so_final.h5"
output_folder = "./ket_qua_low"  # Nơi chứa các thư mục kết quả


base_path = "./ket_qua/nhohon70"
nhan_dien_va_phan_loai(base_path, model_path, output_folder)
base_path = "./ket_qua/nhohon90"
nhan_dien_va_phan_loai(base_path, model_path, output_folder)