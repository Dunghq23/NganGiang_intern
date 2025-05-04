import os
import shutil
import tkinter as tk
from PIL import Image, ImageTk

# Đường dẫn thư mục chứa ảnh
image_folder = r"./nf9"  # Đổi lại tên thư mục phù hợp
labeled_folder = r"./labeled_images"  # Thư mục chứa ảnh đã gán nhãn

# Danh sách các tệp ảnh trong thư mục
image_files = sorted([f for f in os.listdir(image_folder) if f.endswith(('.png', '.jpg'))])
print(f"Tìm thấy {len(image_files)} tệp ảnh trong thư mục {image_folder}")

# Khởi tạo cửa sổ Tkinter
root = tk.Tk()
root.title("Image Annotation Tool")

# Đặt kích thước cửa sổ đủ lớn để chứa ảnh
root.geometry("1000x1000")  # Điều chỉnh kích thước cửa sổ

# Hiển thị ảnh và ô nhập ký tự
image_label = tk.Label(root)
image_label.pack()

# Tạo ô nhập ký tự và làm nó lớn hơn
entry = tk.Entry(root, font=("Arial", 40))  # Kích thước font lớn hơn
entry.pack(pady=20, anchor='center')  # Căn giữa ô nhập liệu
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
    
    # Mở và hiển thị ảnh
    img = Image.open(image_path)
    
    # Resize ảnh cho phù hợp với cửa sổ
    img = img.resize((100, 100))  # Resize ảnh theo kích thước mong muốn, ví dụ 500x500
    img_tk = ImageTk.PhotoImage(img)
    image_label.config(image=img_tk)
    image_label.image = img_tk
    
    # Tạo hàm lưu nhãn và di chuyển ảnh
    def save_label(event=None):
        label = entry.get().strip()
        
        if label:
            # Tạo thư mục nhãn nếu chưa tồn tại
            label_folder_path = os.path.join(labeled_folder, label)
            if not os.path.exists(label_folder_path):
                os.makedirs(label_folder_path)
            
            # Di chuyển ảnh vào thư mục nhãn
            new_image_path = os.path.join(label_folder_path, image_file)
            shutil.move(image_path, new_image_path)
        else:
            # Nếu không nhập ký tự, xóa ảnh
            os.remove(image_path)
            print(f"Đã xóa ảnh {image_path} do không có nhãn.")

        # Xóa nội dung ô nhập ký tự sau khi lưu nhãn
        entry.delete(0, tk.END)
        
        # Sau khi xử lý xong, chuyển sang ảnh tiếp theo
        show_next_image()

    # Khi nhấn Enter, lưu nhãn hoặc xoá ảnh
    entry.bind("<Return>", save_label)

# Bắt đầu hiển thị ảnh đầu tiên
show_next_image()

# Chạy vòng lặp Tkinter
root.mainloop()
