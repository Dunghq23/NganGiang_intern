import os
import shutil
import tkinter as tk
from PIL import Image, ImageTk

# Đường dẫn thư mục chứa ảnh
image_folder = r"./output_chars"
output_file = "annotations.txt"
labeled_folder = r"./labeled_images"  # Thư mục chứa ảnh đã gán nhãn

# Danh sách các tệp ảnh trong thư mục
image_files = sorted([f for f in os.listdir(image_folder) if f.endswith(('.png', '.jpg'))])

# Khởi tạo cửa sổ Tkinter
root = tk.Tk()
root.title("Image Annotation Tool")

# Hiển thị ảnh và ô nhập ký tự
image_label = tk.Label(root)
image_label.pack()

# Tạo ô nhập ký tự
entry = tk.Entry(root)
entry.pack()
entry.focus()

# Chức năng để hiển thị ảnh và cho phép nhập ký tự
def show_next_image():
    if not image_files:
        print("Tất cả ảnh đã được xử lý!")
        root.quit()
        return
    
    # Lấy tên file ảnh tiếp theo
    image_file = image_files.pop(0)
    image_path = os.path.join(image_folder, image_file)
    image_path_labeled = os.path.join(labeled_folder, image_file)
    
    # Mở và hiển thị ảnh
    img = Image.open(image_path)
    img.thumbnail((500, 500))  # Resize ảnh để vừa với cửa sổ
    img_tk = ImageTk.PhotoImage(img)
    image_label.config(image=img_tk)
    image_label.image = img_tk
    
    # Tạo hàm lưu nhãn và di chuyển ảnh
    def save_label(event=None):
        label = entry.get().strip()
        if label:
            # Lưu nhãn vào file txt
            with open(output_file, "a") as f:
                f.write(f"{image_path} {label}\n")
            print(f"Lưu nhãn: {image_path_labeled} cho ảnh {image_path}")
            
            # Tạo thư mục nhãn nếu chưa tồn tại
            label_folder_path = os.path.join(labeled_folder, label)
            if not os.path.exists(label_folder_path):
                os.makedirs(label_folder_path)
            
            # Di chuyển ảnh vào thư mục nhãn
            new_image_path = os.path.join(label_folder_path, image_file)
            shutil.move(image_path, new_image_path)
            print(f"Đã di chuyển ảnh {image_file} vào thư mục {label_folder_path}")
        
        # Xóa nội dung ô nhập ký tự sau khi lưu nhãn
        entry.delete(0, tk.END)
        
        # Sau khi lưu nhãn, chuyển sang ảnh tiếp theo
        show_next_image()

    # Khi nhấn Enter, lưu nhãn và chuyển ảnh tiếp theo
    entry.bind("<Return>", save_label)

# Bắt đầu hiển thị ảnh đầu tiên
show_next_image()

# Chạy vòng lặp Tkinter
root.mainloop()
