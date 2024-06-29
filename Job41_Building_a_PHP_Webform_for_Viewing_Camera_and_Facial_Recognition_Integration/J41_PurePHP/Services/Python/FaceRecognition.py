import face_recognition
import sys

def load_encoding_file(file_path):
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


def recognize_faces(image_path, names, encodings, output_file):
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

        results.append({"top": top, "right": right, "bottom": bottom, "left": left, "name": name})

    if results:
        recognized_name = results[0]['name']
        print(recognized_name)
        with open(output_file, 'w') as file:
            file.write(recognized_name)


if __name__ == "__main__":
    if len(sys.argv) < 2:
        print("Usage: python main.py command [options]")
        sys.exit(1)

    command = sys.argv[1]

    if command == "recognize_faces":
        image_path = sys.argv[2]
        encodings_path = sys.argv[3]
        output_file = sys.argv[4]
        names, encodings = load_encoding_file(encodings_path)
        recognize_faces(image_path, names, encodings, output_file)
    else:
        print("Unknown command")
        sys.exit(1)
