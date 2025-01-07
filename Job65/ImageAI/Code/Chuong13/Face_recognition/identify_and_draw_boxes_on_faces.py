from PIL import Image, ImageDraw, ImageFont
import face_recognition
import numpy as np

# Tải hình ảnh mẫu và học cách nhận diện nó.
PSJ_image = face_recognition.load_image_file("../data/Park_Seo_Joon.png")
PSJ_face_encoding = face_recognition.face_encodings(PSJ_image)[0]

# Tải hình ảnh mẫu thứ hai và học cách nhận diện nó.
PMY_image = face_recognition.load_image_file("../data/Park_Min_Young.png")
PMY_face_encoding = face_recognition.face_encodings(PMY_image)[0]

# Tạo mảng mã hóa khuôn mặt đã biết và tên của họ
known_face_encodings = [
    PSJ_face_encoding,
    PMY_face_encoding
]
known_face_names = [
    "Park Seo Joon",
    "Park Min Young"
]

# Tải một ảnh với khuôn mặt không xác định
unknown_image = face_recognition.load_image_file("../data/PSJ.jpg")

# Tìm tất cả khuôn mặt và mã hóa khuôn mặt trong ảnh không xác định
face_locations = face_recognition.face_locations(unknown_image)
face_encodings = face_recognition.face_encodings(unknown_image, face_locations)

# Chuyển đổi ảnh thành ảnh định dạng PIL để chúng ta có thể vẽ lên đó với thư viện Pillow
pil_image = Image.fromarray(unknown_image)
# Tạo một đối tượng vẽ Draw của Pillow để vẽ
draw = ImageDraw.Draw(pil_image)

# Lặp qua mỗi khuôn mặt được tìm thấy trong ảnh không xác định
for (top, right, bottom, left), face_encoding in zip(face_locations, face_encodings):
    # Xem xem khuôn mặt có trùng khớp với khuôn mặt đã biết không
    matches = face_recognition.compare_faces(known_face_encodings, face_encoding)

    name = "Unknown"

    # Sử dụng khoảng cách nhỏ nhất để nhận diện khuôn mặt
    face_distances = face_recognition.face_distance(known_face_encodings, face_encoding)
    best_match_index = np.argmin(face_distances)

    name = known_face_names[best_match_index] if matches[best_match_index] else "Unknown"

    # Vẽ một hộp xung quanh khuôn mặt sử dụng module Pillow
    draw.rectangle(((left, top), (right, bottom)), outline=(0, 0, 255))
    
    # Vẽ một nhãn với tên dưới khuôn mặt (chữ to hơn)
    text = f"{name} ({round(min(face_distances), 2)})"
    font = ImageFont.load_default()

    # Sử dụng textsize thay vì textbbox
    text_width, text_height = draw.textsize(text, font=font)

    # Vẽ hộp xung quanh vùng chữ với padding
    box_padding = 10
    draw.rectangle(((left, bottom - text_height - 10), (right, bottom + box_padding)), fill=(0, 0, 255), outline=(0, 0, 255))

    # Vẽ văn bản
    draw.text((left + 6, bottom - text_height - 5), text, fill=(255, 255, 255, 255))

# Xóa thư viện vẽ khỏi bộ nhớ theo tài liệu của Pillow
del draw

# Hiển thị ảnh kết quả

# Bạn cũng có thể lưu một bản sao của ảnh mới vào đĩa nếu muốn bằng cách bỏ chú thích dòng này
pil_image.save("image_with_boxes.jpg")

pil_image.show()