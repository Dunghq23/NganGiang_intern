import sys
import face_recognition
import os
from PIL import Image
import time

def LoadEncodeFile(file_path):
    """
    Đọc tệp mã hóa khuôn mặt và trả về danh sách tên và mã hóa tương ứng.
    
    :param file_path: Đường dẫn đến tệp chứa mã hóa khuôn mặt.
    :return: Tuple (names, encodings) nơi names là danh sách tên và encodings là danh sách các mã hóa khuôn mặt.
    """
    names = []
    encodings = []

    with open(file_path, 'r') as file:
        lines = file.readlines()

    current_name = None
    current_encoding = []

    for line in lines:
        parts = line.strip().split(': ')

        if len(parts) == 2:
            if current_name is not None:
                names.append(current_name)
                encodings.append(current_encoding)

            current_name = parts[0]
            current_encoding = [float(value) for value in parts[1][1:-1].replace(']', '').split()]
        elif current_name is not None:
            current_encoding.extend([float(value) for value in line.strip().replace(']', '').split()])

    if current_name is not None:
        names.append(current_name)
        encodings.append(current_encoding)

    return names, encodings

def Encode(image_path, encoding_file, username):
    """
    Mã hóa khuôn mặt từ ảnh và lưu vào tệp mã hóa.

    :param image_path: Đường dẫn đến ảnh cần mã hóa.
    :param encoding_file: Đường dẫn đến tệp nơi lưu mã hóa khuôn mặt.
    :param username: Tên người dùng để gán cho mã hóa khuôn mặt.
    """
    if not os.path.isfile(encoding_file):
        with open(encoding_file, 'w', encoding='utf-8') as file:
            file.write('')

    with open(encoding_file, 'a', encoding='utf-8') as encoding_file:

        image = face_recognition.load_image_file(image_path)
        encoding = face_recognition.face_encodings(image)

        if len(encoding) > 0:
            encoding_file.write(f"{username}: {encoding[0]}\n")
            # os.remove(image_path)
        print("Đã xử lý xong")

def Recognize(image_path, names, encodings, resultPath):
    """
    Nhận diện khuôn mặt từ ảnh và ghi kết quả vào tệp.

    :param image_path: Đường dẫn đến ảnh cần nhận diện.
    :param names: Danh sách tên người đã mã hóa.
    :param encodings: Danh sách mã hóa khuôn mặt tương ứng với tên.
    :param resultPath: Đường dẫn đến tệp nơi lưu kết quả nhận diện.
    """
    image = face_recognition.load_image_file(image_path)
    face_locations = face_recognition.face_locations(image)
    face_encodings = face_recognition.face_encodings(image, face_locations)

    results = []
    for (top, right, bottom, left), face_encoding in zip(face_locations, face_encodings):
        matches = face_recognition.compare_faces(encodings, face_encoding, tolerance=0.3)
        name = "Unknown"

        if True in matches:
            first_match_index = matches.index(True)
            name = names[first_match_index]

        results.append({
            "top": top,
            "right": right,
            "bottom": bottom,
            "left": left,
            "name": name
        })
    try:
        # Tạo hoặc mở file resultPath và ghi dữ liệu vào đó
        with open(resultPath, "w", encoding="UTF-8") as file:
            if len(results) != 0:
                try:
                    if len(results) > 1:
                        recognized_names = [result['name'] for result in results]
                        file.write("Phát hiện 2 khuôn mặt, vui lòng thử lại!")
                    else:
                        file.write(f"{results[0]['name']} ({results[0]['top']}, {results[0]['right']}, {results[0]['bottom']}, {results[0]['left']})")
                except Exception as e:
                    file.write(f"Lỗi khi thực thi script Python: {e}")
            else:
                file.write("Không có khuôn mặt được tìm thấy\n")
    except Exception as e:
        # Nếu xảy ra lỗi, ghi lỗi vào file
        with open(resultPath, "w", encoding="UTF-8") as file:
            file.write(f"Lỗi khi thực thi script Python: {e}\n")

def Recognize2(image_path, names, encodings):
    """
    Nhận diện khuôn mặt từ ảnh và in kết quả ra màn hình.

    :param image_path: Đường dẫn đến ảnh cần nhận diện.
    :param names: Danh sách tên người đã mã hóa.
    :param encodings: Danh sách mã hóa khuôn mặt tương ứng với tên.
    """
    image = face_recognition.load_image_file(image_path)
    face_locations = face_recognition.face_locations(image)
    face_encodings = face_recognition.face_encodings(image, face_locations)

    results = []
    for (top, right, bottom, left), face_encoding in zip(face_locations, face_encodings):
        matches = face_recognition.compare_faces(encodings, face_encoding, tolerance=0.3)
        name = "Unknown"


        if True in matches:
            first_match_index = matches.index(True)
            name = names[first_match_index]

        # Thêm thông tin khuôn mặt vào dictionary và thêm vào danh sách results
        results.append({
            "top": top,
            "right": right,
            "bottom": bottom,
            "left": left,
            "name": name
        })

    if results:
        try:
            if len(results) > 1:
                recognized_names = [result['name'] for result in results]
                print("Phát hiện 2 khuôn mặt, vui lòng thử lại!")
            else:
                print(results)
        except Exception as e:
            print(f"Lỗi khi thực thi script Python: {e}")
    else:
        print("Không có khuôn mặt được tìm thấy")

if __name__ == "__main__":
    """
    Điểm vào chính của chương trình. Xử lý các lệnh và tham số đầu vào từ dòng lệnh để gọi các hàm tương ứng.
    """
    if len(sys.argv) < 2:
        print("Usage: python RecognizeFace.py command [options]")
        sys.exit(1)

    command = sys.argv[1]

    if command == "Recognize":
        image_path = sys.argv[2]
        encodings_path = sys.argv[3]
        result_path = sys.argv[4]
        names, encodings = LoadEncodeFile(encodings_path)
        Recognize(image_path, names, encodings, result_path)
    elif command == "Recognition2":
        image_path = sys.argv[2]
        encodings_path = sys.argv[3]
        names, encodings = LoadEncodeFile(encodings_path)
        Recognize2(image_path, names, encodings)
    elif command == "Encode":
        image_path = sys.argv[2]
        encoding_file = sys.argv[3]
        username = sys.argv[4]
        Encode(image_path, encoding_file, username)
    else:
        print("Unknown command")
        sys.exit(1)