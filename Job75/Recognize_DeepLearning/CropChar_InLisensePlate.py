import os
import cv2
import numpy as np
from skimage.filters import threshold_local
from skimage import measure
import imutils

# Paths
INPUT_FOLDER = "./Cropped"  # Thư mục chứa ảnh biển số đã cắt
CHAR_OUTPUT_DIR = "./output_chars"
PLATE_OUTPUT_DIR = "./Cropped"
os.makedirs(PLATE_OUTPUT_DIR, exist_ok=True)
os.makedirs(CHAR_OUTPUT_DIR, exist_ok=True)

def save_characters(candidates, original_filename, save_dir=CHAR_OUTPUT_DIR):
    """Save sorted character images to output directory."""
    file_prefix = os.path.splitext(os.path.basename(original_filename))[0]
    for idx, (char_img, _) in enumerate(candidates):
        filename = os.path.join(save_dir, f"{file_prefix}_char_{idx:02d}.png")
        cv2.imwrite(filename, char_img)
        print(f"Saved {filename}")


def convert_to_square(image, pad_color=255):
    """Pad image to make it square."""
    h, w = image.shape[:2]
    if h == w:
        return image

    size_diff = abs(h - w)
    if len(image.shape) == 2:  # grayscale
        pad_shape = (size_diff // 2, size_diff - size_diff // 2)
        pad = [(pad_shape[0], pad_shape[1]), (0, 0)] if h < w else [(0, 0), (pad_shape[0], pad_shape[1])]
    else:  # color
        pad = [(0, 0), (0, 0), (0, 0)]
        if h < w:
            pad[0] = (size_diff // 2, size_diff - size_diff // 2)
        else:
            pad[1] = (size_diff // 2, size_diff - size_diff // 2)
    return np.pad(image, pad, mode='constant', constant_values=pad_color)


def preprocess_license_plate(plate_img):
    """Convert plate image to binary using adaptive thresholding."""
    hsv = cv2.cvtColor(plate_img, cv2.COLOR_BGR2HSV)
    _, _, v = cv2.split(hsv)
    T = threshold_local(v, 15, offset=10, method="gaussian")
    return ((v > T) * 255).astype("uint8")


def extract_and_sort_characters(thresh, plate_img):
    """Extract character candidates from binary image."""
    inverted = cv2.bitwise_not(thresh)
    resized = imutils.resize(inverted, width=400)
    blurred = cv2.medianBlur(resized, 5)
    labels = measure.label(blurred, connectivity=2, background=0)

    candidates = []
    scale_x = plate_img.shape[1] / resized.shape[1]
    scale_y = plate_img.shape[0] / resized.shape[0]

    for label in np.unique(labels):
        if label == 0:
            continue
        mask = (labels == label).astype("uint8") * 255
        contours, _ = cv2.findContours(mask, cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_SIMPLE)
        if not contours:
            continue

        contour = max(contours, key=cv2.contourArea)
        x, y, w, h = cv2.boundingRect(contour)
        aspect_ratio = w / float(h)
        solidity = cv2.contourArea(contour) / float(w * h)
        height_ratio = h / float(plate_img.shape[0])

        if 0.1 < aspect_ratio < 1.0 and solidity > 0.1 and 0.35 < height_ratio < 2.0:
            x_orig, y_orig = int(x * scale_x), int(y * scale_y)
            w_orig, h_orig = int(w * scale_x), int(h * scale_y)
            char_img = plate_img[y_orig:y_orig + h_orig, x_orig:x_orig + w_orig]
            square_char = convert_to_square(char_img)
            resized_char = cv2.resize(square_char, (28, 28), cv2.INTER_AREA)
            candidates.append((resized_char, (y, x)))

    # Sort by rows then columns
    candidates = sorted(candidates, key=lambda x: x[1][0])
    mid_y = np.median([pos[0] for _, pos in candidates])
    top = sorted([c for c in candidates if c[1][0] < mid_y], key=lambda x: x[1][1])
    bottom = sorted([c for c in candidates if c[1][0] >= mid_y], key=lambda x: x[1][1])
    return top + bottom


def process_image(image_path):
    """Process cropped license plate image (no detection needed)."""
    img = cv2.imread(image_path)
    if img is None:
        print(f"⚠️ Cannot read image: {image_path}")
        return

    print(f"Processing: {image_path}")

    # Since image is already cropped, no need to detect license plate
    thresh = preprocess_license_plate(img)
    candidates = extract_and_sort_characters(thresh, img)
    save_characters(candidates, image_path)


def main():
    image_files = sorted([f for f in os.listdir(INPUT_FOLDER) if f.lower().endswith(('.jpg', '.jpeg', '.png'))])
    for image_file in image_files:
        image_path = os.path.join(INPUT_FOLDER, image_file)
        process_image(image_path)


if __name__ == "__main__":
    main()
