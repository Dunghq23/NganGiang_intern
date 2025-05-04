import os

def print_folder_tree(directory, prefix=""):
    entries = os.listdir(directory)
    entries = [entry for entry in entries if entry != 'tf_env' and entry != '.vs' and entry != 'obj' and entry != '.vs']
    entries.sort()

    for index, entry in enumerate(entries):
        path = os.path.join(directory, entry)
        is_last = index == len(entries) - 1
        connector = "└── " if is_last else "├── "

        print(prefix + connector + entry)

        if os.path.isdir(path):
            extension = "    " if is_last else "│   "
            print_folder_tree(path, prefix + extension)

# Gọi hàm với đường dẫn cần in cấu trúc
folder_path = r"D:\Documents\Work\NganGiang\HAQUANGDUNG\Job76\LisensePlateNumber"
# folder_path = r"D:\Documents\THUYLOIUNIVERSITY\Semester8\GraduationProject\HRM\Backend\src\main\java\tlu\finalproject\hrmanagement"
print(folder_path)
print_folder_tree(folder_path)
