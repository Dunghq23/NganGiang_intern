# train_model.py
from levatas_ai_library import Trainer

# Khởi tạo đối tượng huấn luyện
trainer = Trainer(config_path='DetectImage\config.json')

# Huấn luyện mô hình
trainer.train()
