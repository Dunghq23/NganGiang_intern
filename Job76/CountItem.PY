import os

def count_images_in_subdirectories(directory_path):
    """Count the number of subdirectories and the number of images within each subdirectory."""
    # Kiểm tra nếu thư mục không tồn tại
    if not os.path.exists(directory_path):
        print(f"⚠️ Thư mục {directory_path} không tồn tại.")
        return

    # Đếm số thư mục con và số ảnh trong từng thư mục con
    for subdir, dirs, files in os.walk(directory_path):
        # Nếu thư mục con không có tệp ảnh thì bỏ qua
        if subdir == directory_path:
            continue  # Bỏ qua thư mục gốc

        image_files = [file for file in files if file.lower().endswith(('.jpg', '.jpeg', '.png'))]
        print(f"Thư mục {subdir}: {len(image_files)}")

# Thư mục bạn muốn đếm ảnh trong các thư mục con
directory_path = "./ket_qua"  # Thay đổi đường dẫn tới thư mục bạn cần đếm

count_images_in_subdirectories(directory_path)


def count_images(directory_path):
    """Count the number of images within a directory and its subdirectories."""
    image_files = []
    
    for subdir, dirs, files in os.walk(directory_path):
        # Lọc ra các tệp ảnh (jpg, jpeg, png)
        image_files += [file for file in files if file.lower().endswith(('.jpg', '.jpeg', '.png'))]

    return len(image_files)


def calculate_completion_rate(directory_a, directory_b):
    """Calculate the completion rate as total images in A divided by total images in B."""
    # Đếm số ảnh trong thư mục A và B
    total_images_a = count_images(directory_a)
    total_images_b = count_images(directory_b)
    
    # In số lượng ảnh trong A và B
    print(f"Số lượng ảnh đã gán: {total_images_a}")
    print(f"Số lượng ảnh còn lại: {total_images_b}")
    
    # Tính tỷ lệ hoàn thành
    if total_images_b == 0:
        print("⚠️ Không có ảnh trong thư mục B, không thể tính tỷ lệ hoàn thành.")
        return
    
    completion_rate = (total_images_a / (total_images_a + total_images_b)) * 100
    print(f"Tỷ lệ hoàn thành: {completion_rate:.2f}%")
    return completion_rate


# Đường dẫn đến thư mục A và B
directory_a = "./labeled_images"  # Thư mục A
directory_b = "./output_chars"  # Thư mục B

# Tính tỷ lệ hoàn thành
calculate_completion_rate(directory_a, directory_b)
