import os

# Thư mục chứa ảnh cần rename
folder_path = 'cropped'

# Lấy danh sách file trong thư mục (lọc ra file ảnh thôi)
image_files = [f for f in os.listdir(folder_path) if f.lower().endswith(('.jpg', '.jpeg', '.png'))]

# Sắp xếp danh sách file (nếu muốn thứ tự ổn định)
image_files.sort()

# Lặp và đổi tên từng file
for idx, filename in enumerate(image_files, start=1):
    # Tách phần mở rộng
    _, ext = os.path.splitext(filename)

    # Tạo tên mới: ví dụ 0001.jpg, 0002.jpg,...
    new_name = f"{idx:04d}{ext}"

    # Đường dẫn cũ và mới
    old_path = os.path.join(folder_path, filename)
    new_path = os.path.join(folder_path, new_name)

    # Đổi tên
    os.rename(old_path, new_path)
    print(f"Renamed: {filename} -> {new_name}")
