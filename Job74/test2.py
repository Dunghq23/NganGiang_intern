import torch
import torch.nn as nn
import torchvision.transforms as transforms
from torch.utils.data import Dataset, DataLoader
import cv2
import numpy as np

# Mô hình CRNN đơn giản
class CRNN(nn.Module):
    def __init__(self, num_classes):
        super(CRNN, self).__init__()
        self.cnn = nn.Sequential(
            nn.Conv2d(1, 64, kernel_size=3, stride=1, padding=1),
            nn.ReLU(),
            nn.MaxPool2d(2, 2),
            nn.Conv2d(64, 128, kernel_size=3, stride=1, padding=1),
            nn.ReLU(),
            nn.MaxPool2d(2, 2),
        )
        self.rnn = nn.LSTM(128*8, 256, bidirectional=True, batch_first=True)
        self.fc = nn.Linear(512, num_classes)

    def forward(self, x):
        x = self.cnn(x)
        x = x.view(x.size(0), x.size(2), -1)
        x, _ = self.rnn(x)
        x = self.fc(x)
        return x

# Khởi tạo mô hình
num_classes = 36  # 10 chữ số + 26 chữ cái
model = CRNN(num_classes)

# Load ảnh và tiền xử lý
def preprocess_image(image_path):
    image = cv2.imread(image_path, cv2.IMREAD_GRAYSCALE)
    image = cv2.resize(image, (128, 32))
    image = image.astype(np.float32) / 255.0
    image = torch.tensor(image).unsqueeze(0).unsqueeze(0)  # Thêm batch và channel dimension
    return image

# Nhận dạng ký tự
image_path = r"D:\Documents\Work\NganGiang\HAQUANGDUNG\Job74\Cropped\license_plate_0_0.jpg"
image = preprocess_image(image_path)
output = model(image)

print("Ký tự nhận dạng:", output)
