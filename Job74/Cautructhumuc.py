import os

def print_folder_tree(directory, prefix=""):
    # Lấy danh sách thư mục con, bỏ qua thư mục 'frontend'
    entries = [entry for entry in os.listdir(directory)
               if os.path.isdir(os.path.join(directory, entry)) and entry != 'frontend'  and entry != '.git']
    entries.sort()

    for index, entry in enumerate(entries):
        path = os.path.join(directory, entry)
        is_last = index == len(entries) - 1
        connector = "└── " if is_last else "├── "

        print(prefix + connector + entry)

        # Đệ quy cho thư mục con
        extension = "    " if is_last else "│   "
        print_folder_tree(path, prefix + extension)

# Gọi hàm với đường dẫn cần in cấu trúc
folder_path = "D:\Documents\THUYLOIUNIVERSITY\Semester8\GraduationProject\HRM"
print(folder_path)
print_folder_tree(folder_path)
