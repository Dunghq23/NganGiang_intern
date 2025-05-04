# -*- coding: utf-8 -*-
"""ContinueTrainingImbalanced.py

Mã nguồn để tiếp tục huấn luyện mô hình CNN với dữ liệu mất cân bằng
"""

import os
import numpy as np
import tensorflow as tf
from tensorflow.keras.preprocessing.image import ImageDataGenerator
from tensorflow.keras.models import load_model
from tensorflow.keras.callbacks import ModelCheckpoint, EarlyStopping, ReduceLROnPlateau
from sklearn.model_selection import train_test_split
from sklearn.utils import class_weight
import cv2
import matplotlib.pyplot as plt
import seaborn as sns
from pathlib import Path
from sklearn.metrics import classification_report, confusion_matrix
import pandas as pd
from collections import Counter

# Đường dẫn đến thư mục chứa dữ liệu mới và mô hình đã lưu
data_dir = "./ket_qua"  # Cập nhật đường dẫn tới thư mục dữ liệu mới
model_path = "/content/drive/MyDrive/NGANGIANGINTERNSHIP/Job74/DL/model_nhan_dang_bien_so_final_2.h5"  # Đường dẫn đến mô hình đã huấn luyện

# Tham số cho mô hình
img_width, img_height = 64, 64
batch_size = 32
epochs = 30
grayscale = True  # Phải giữ nhất quán với mô hình đã huấn luyện trước đó
learning_rate = 0.0001  # Giảm learning rate khi fine-tuning
min_samples_per_class = 100  # Số lượng tối thiểu mẫu sau khi augment cho các lớp thiểu số

# Hàm kiểm tra phân bố dữ liệu
def analyze_data_distribution(data_dir):
    class_counts = {}
    class_dirs = sorted(os.listdir(data_dir))
    
    for class_name in class_dirs:
        class_path = os.path.join(data_dir, class_name)
        if os.path.isdir(class_path):
            count = len([f for f in os.listdir(class_path) if os.path.isfile(os.path.join(class_path, f))])
            class_counts[class_name] = count
    
    # Hiển thị phân bố
    df = pd.DataFrame(list(class_counts.items()), columns=['Class', 'Count'])
    df = df.sort_values('Count')
    
    plt.figure(figsize=(12, 8))
    plt.barh(df['Class'], df['Count'])
    plt.title('Phân bố số lượng mẫu theo lớp')
    plt.xlabel('Số lượng mẫu')
    plt.ylabel('Lớp')
    plt.tight_layout()
    plt.savefig("data_distribution.png")
    plt.show()
    
    return class_counts

# Load dữ liệu với xử lý đặc biệt cho các lớp thiểu số
def load_data(data_dir):
    images = []
    labels = []
    class_dirs = sorted(os.listdir(data_dir))
    label_to_idx = {class_name: idx for idx, class_name in enumerate(class_dirs)}
    
    # Phân tích phân bố dữ liệu
    class_counts = analyze_data_distribution(data_dir)
    print("Phân bố dữ liệu ban đầu:")
    for class_name, count in class_counts.items():
        print(f"Lớp {class_name}: {count} mẫu")
    
    for class_name in class_dirs:
        class_path = os.path.join(data_dir, class_name)
        if not os.path.isdir(class_path):
            continue
        
        class_images = []
        for img_file in os.listdir(class_path):
            img_path = os.path.join(class_path, img_file)
            try:
                if grayscale:
                    img = cv2.imread(img_path, cv2.IMREAD_GRAYSCALE)
                    img = cv2.resize(img, (img_width, img_height))
                    img = np.expand_dims(img, axis=-1)
                else:
                    img = cv2.imread(img_path)
                    img = cv2.cvtColor(img, cv2.COLOR_BGR2RGB)
                    img = cv2.resize(img, (img_width, img_height))

                if img is not None:
                    class_images.append(img)
            except Exception as e:
                print(f"Lỗi khi đọc ảnh {img_path}: {e}")
        
        # Thêm tất cả ảnh của lớp vào tập dữ liệu
        images.extend(class_images)
        labels.extend([label_to_idx[class_name]] * len(class_images))
        
    return np.array(images), np.array(labels), label_to_idx

# Tải dữ liệu
print("Đang tải dữ liệu...")
images, labels, label_to_idx = load_data(data_dir)
idx_to_label = {v: k for k, v in label_to_idx.items()}
num_classes = len(label_to_idx)

print(f"Số lượng ảnh: {len(images)}")
print(f"Số lượng nhãn: {num_classes}")

# Kiểm tra sự mất cân bằng
label_counts = Counter(labels)
print("Phân bố lớp sau khi tải:")
for label_idx, count in label_counts.items():
    print(f"Lớp {idx_to_label[label_idx]}: {count} mẫu")

# Chuẩn hóa dữ liệu
images = images.astype('float32') / 255.0
print(f"Kích thước tập dữ liệu: {images.shape}")

# Chia tập dữ liệu với stratify để đảm bảo phân bố lớp nhất quán
X_train, X_temp, y_train, y_temp = train_test_split(images, labels, test_size=0.3, stratify=labels, random_state=42)
X_val, X_test, y_val, y_test = train_test_split(X_temp, y_temp, test_size=0.5, stratify=y_temp, random_state=42)

y_train_onehot = tf.keras.utils.to_categorical(y_train, num_classes)
y_val_onehot = tf.keras.utils.to_categorical(y_val, num_classes)
y_test_onehot = tf.keras.utils.to_categorical(y_test, num_classes)

print(f"Kích thước tập huấn luyện: {X_train.shape}")
print(f"Kích thước tập kiểm định: {X_val.shape}")
print(f"Kích thước tập kiểm thử: {X_test.shape}")

# Tăng cường dữ liệu cho các lớp thiểu số
# Tạo bộ tăng cường dữ liệu cơ bản
datagen_basic = ImageDataGenerator(
    rotation_range=15,
    width_shift_range=0.1,
    height_shift_range=0.1,
    shear_range=0.1,
    zoom_range=0.1,
    fill_mode='nearest'
)

# Tạo bộ tăng cường dữ liệu mạnh mẽ hơn cho các lớp thiểu số
datagen_strong = ImageDataGenerator(
    rotation_range=30,
    width_shift_range=0.2,
    height_shift_range=0.2,
    shear_range=0.2,
    zoom_range=0.2,
    brightness_range=[0.8, 1.2],
    horizontal_flip=False,  # Tránh flip với các ký tự
    fill_mode='nearest'
)

# Tính toán class weights cho quá trình huấn luyện
y_train_np = np.array(y_train)
class_weights = class_weight.compute_class_weight(
    class_weight='balanced',
    classes=np.unique(y_train_np),
    y=y_train_np
)
class_weights_dict = {i: class_weights[i] for i in range(len(class_weights))}
print(f"Class weights: {class_weights_dict}")

# Load mô hình đã huấn luyện trước đó
print(f"Đang tải mô hình từ {model_path}...")
model = load_model(model_path)

# Hiển thị cấu trúc mô hình
model.summary()

# Biên dịch lại mô hình với learning rate mới (thấp hơn)
model.compile(
    optimizer=tf.keras.optimizers.Adam(learning_rate=learning_rate),
    loss='categorical_crossentropy',
    metrics=['accuracy']
)

# Callbacks
checkpoint = ModelCheckpoint(
    "fine_tuned_model.h5",
    monitor='val_accuracy',
    save_best_only=True,
    mode='max'
)
early_stop = EarlyStopping(
    monitor='val_loss',
    patience=15,
    restore_best_weights=True
)
reduce_lr = ReduceLROnPlateau(
    monitor='val_loss',
    factor=0.5,
    patience=5,
    min_lr=1e-7
)

callbacks = [checkpoint, early_stop, reduce_lr]

# Sử dụng Class-aware data generator 
def generate_balanced_batches(X_train, y_train, batch_size=32):
    num_classes = len(np.unique(y_train))
    
    # Tạo các chỉ mục cho mỗi lớp
    class_indices = [np.where(y_train == i)[0] for i in range(num_classes)]
    
    # Số lượng mẫu tối thiểu cho mỗi lớp trong mỗi batch
    min_samples_per_class_in_batch = max(1, batch_size // num_classes)
    
    while True:
        # Khởi tạo batch
        batch_X = []
        batch_y = []
        
        # Đảm bảo mỗi lớp đều có ít nhất một vài mẫu trong batch
        for i in range(num_classes):
            # Chọn ngẫu nhiên các chỉ mục từ lớp hiện tại
            if len(class_indices[i]) > 0:
                # Số lượng mẫu cho lớp này trong batch hiện tại
                if len(class_indices[i]) < min_samples_per_class_in_batch:
                    # Nếu lớp có ít mẫu, lặp lại các mẫu
                    indices = np.random.choice(class_indices[i], size=min_samples_per_class_in_batch, replace=True)
                else: 
                    indices = np.random.choice(class_indices[i], size=min_samples_per_class_in_batch, replace=False)
                
                # Thêm vào batch
                for idx in indices:
                    x = X_train[idx]
                    y = y_train[idx]
                    
                    # Áp dụng augmentation cho lớp thiểu số
                    if len(class_indices[i]) < 100:  # Ngưỡng định nghĩa lớp thiểu số
                        x = datagen_strong.random_transform(x)
                    else:
                        x = datagen_basic.random_transform(x)
                    
                    batch_X.append(x)
                    batch_y.append(y)
        
        # Bổ sung thêm mẫu ngẫu nhiên nếu batch chưa đủ kích thước
        remaining = batch_size - len(batch_X)
        if remaining > 0:
            all_indices = np.arange(len(X_train))
            extra_indices = np.random.choice(all_indices, size=remaining, replace=False)
            for idx in extra_indices:
                x = X_train[idx]
                y = y_train[idx]
                x = datagen_basic.random_transform(x)
                batch_X.append(x)
                batch_y.append(y)
        
        # Chuyển đổi sang mảng numpy và chuyển đổi one-hot
        batch_X = np.array(batch_X)
        batch_y = tf.keras.utils.to_categorical(np.array(batch_y), num_classes)
        
        yield batch_X, batch_y

# Tiếp tục huấn luyện mô hình với bộ phát batch cân bằng
print("Bắt đầu tiếp tục huấn luyện...")
history = model.fit(
    generate_balanced_batches(X_train, y_train, batch_size),
    steps_per_epoch=len(X_train) // batch_size,
    epochs=epochs,
    validation_data=(X_val, y_val_onehot),
    class_weight=class_weights_dict,
    callbacks=callbacks,
    initial_epoch=0  # Bắt đầu từ epoch 0 của quá trình fine-tuning
)

# Đánh giá mô hình
loss, accuracy = model.evaluate(X_test, y_test_onehot)
print(f"Độ chính xác trên tập kiểm tra: {accuracy*100:.2f}%")
print(f"Tổn thất trên tập kiểm tra: {loss:.4f}")

# Lưu mô hình đã fine-tuned
fine_tuned_model_path = "model_fine_tuned_balanced.h5"
model.save(fine_tuned_model_path)
print(f"Đã lưu mô hình fine-tuned tại: {fine_tuned_model_path}")

# Vẽ đồ thị kết quả
plt.figure(figsize=(12, 4))
plt.subplot(1, 2, 1)
plt.plot(history.history['accuracy'], label='Độ chính xác huấn luyện')
plt.plot(history.history['val_accuracy'], label='Độ chính xác kiểm định')
plt.legend()
plt.title('Độ chính xác qua các epoch')

plt.subplot(1, 2, 2)
plt.plot(history.history['loss'], label='Mất mát huấn luyện')
plt.plot(history.history['val_loss'], label='Mất mát kiểm định')
plt.legend()
plt.title('Mất mát qua các epoch')
plt.savefig("fine_tuning_history.png")
plt.show()

# Confusion Matrix
y_pred = np.argmax(model.predict(X_test), axis=1)
y_true = np.argmax(y_test_onehot, axis=1)
cm = confusion_matrix(y_true, y_pred)

# Vẽ confusion matrix với nhãn lớp
plt.figure(figsize=(16, 14))
sns.heatmap(cm, annot=True, fmt='d', cmap='Blues', 
            xticklabels=[idx_to_label[i] for i in range(num_classes)],
            yticklabels=[idx_to_label[i] for i in range(num_classes)])
plt.xlabel('Dự đoán')
plt.ylabel('Thực tế')
plt.title('Confusion Matrix')
plt.savefig("fine_tuning_confusion_matrix.png")
plt.show()

# Báo cáo phân loại chi tiết
print("Báo cáo phân loại:")
class_names = [idx_to_label[i] for i in range(num_classes)]
print(classification_report(y_true, y_pred, target_names=class_names))

# Báo cáo độ chính xác của từng lớp
per_class_accuracy = {}
for i in range(num_classes):
    class_indices = np.where(y_true == i)[0]
    if len(class_indices) > 0:
        class_acc = np.sum(y_pred[class_indices] == i) / len(class_indices)
        per_class_accuracy[idx_to_label[i]] = class_acc

# Hiển thị độ chính xác của từng lớp
df_acc = pd.DataFrame(list(per_class_accuracy.items()), columns=['Lớp', 'Độ chính xác'])
df_acc = df_acc.sort_values('Độ chính xác')

plt.figure(figsize=(12, 8))
plt.barh(df_acc['Lớp'], df_acc['Độ chính xác'])
plt.title('Độ chính xác theo từng lớp')
plt.xlabel('Độ chính xác')
plt.ylabel('Lớp')
plt.xlim(0, 1)
plt.tight_layout()
plt.savefig("per_class_accuracy.png")
plt.show()