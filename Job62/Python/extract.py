import cv2
import os
from PIL import Image

def extract_first_frame(video_path, output_folder):
    """
    Trích xuất khung hình đầu tiên từ video và lưu dưới dạng ảnh.
    
    :param video_path: Đường dẫn đến video.
    :param output_folder: Thư mục lưu khung hình.
    :return: Đường dẫn ảnh được lưu hoặc None nếu không thành công.
    """
    # Tạo thư mục lưu ảnh nếu chưa tồn tại
    if not os.path.exists(output_folder):
        os.makedirs(output_folder)
    
    # Mở video
    cap = cv2.VideoCapture(video_path)
    if not cap.isOpened():
        print("Không thể mở video.")
        return None

    ret, frame = cap.read()  # Lấy khung hình đầu tiên
    if ret:
        frame_name = "frame_00000.jpg"
        frame_path = os.path.join(output_folder, frame_name)
        cv2.imwrite(frame_path, frame)  # Lưu ảnh
        print(f"Lưu: {frame_path}")
        cap.release()
        return frame_path
    else:
        print("Không thể đọc khung hình đầu tiên từ video.")
        cap.release()
        return None


def get_image_properties(image_path):
    """
    Lấy thông tin width, height và DPI của ảnh.
    
    :param image_path: Đường dẫn tới ảnh.
    :return: Dictionary chứa width, height, DPI (horizontal và vertical).
    """
    try:
        with Image.open(image_path) as img:
            # Lấy kích thước (width, height)
            width, height = img.size

            # Lấy DPI (nếu có)
            dpi = img.info.get("dpi", (72, 72))  # Trả về (96, 96) nếu không có DPI
            horizontal_dpi, vertical_dpi = dpi

            print(f"Width: {width} pixels")
            print(f"Height: {height} pixels")
            print(f"DPI (Horizontal, Vertical): {horizontal_dpi}, {vertical_dpi}")

            return {
                "width": width,
                "height": height,
                "horizontal_dpi": horizontal_dpi,
                "vertical_dpi": vertical_dpi
            }
    except Exception as e:
        print(f"Không thể đọc thông tin từ ảnh: {e}")
        return None


def calculate_r_from_dpi(width_px, height_px, dpi_x, dpi_y):
    """
    Tính R (m/pixel) từ kích thước pixel và DPI của ảnh.
    
    :param width_px: Chiều ngang (pixels).
    :param height_px: Chiều dọc (pixels).
    :param dpi_x: DPI ngang.
    :param dpi_y: DPI dọc.
    :return: R (m/pixel) dựa trên DPI.
    """
    # Chuyển đổi pixel sang inch
    width_inch = width_px / dpi_x
    height_inch = height_px / dpi_y

    # Chuyển đổi inch sang mét
    width_m = width_inch * 0.0254
    height_m = height_inch * 0.0254

    # Tính Pixel Per Meter
    ppm_x = width_px / width_m
    ppm_y = height_px / height_m

    # Tính R (sử dụng chiều ngang hoặc chiều dọc)
    r_x = 1 / ppm_x
    r_y = 1 / ppm_y

    return r_x, r_y


# Sử dụng
video_path = 'b.mp4'  # Đường dẫn video
output_folder = 'frames_output'  # Thư mục lưu ảnh

# Trích xuất khung hình đầu tiên
first_frame_path = extract_first_frame(video_path, output_folder)

# Nếu trích xuất thành công, lấy thông tin và tính R
if first_frame_path:
    image_properties = get_image_properties(first_frame_path)

    if image_properties:
        width_px = image_properties['width']
        height_px = image_properties['height']
        dpi_x = image_properties['horizontal_dpi']
        dpi_y = image_properties['vertical_dpi']

        # Tính R từ thông tin thực tế của ảnh
        r_x, r_y = calculate_r_from_dpi(width_px, height_px, dpi_x, dpi_y)
        print(f"R (Horizontal) = {r_x} m/pixel")
        print(f"R (Vertical) = {r_y} m/pixel")
    else:
        print("Không thể lấy thông tin từ ảnh.")
else:
    print("Không thể trích xuất khung hình đầu tiên từ video.")
