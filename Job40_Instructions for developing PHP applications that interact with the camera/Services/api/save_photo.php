<?php
if (isset($_POST['imageBase64'])) {
    $imageData = $_POST['imageBase64'];

    // Chuẩn bị dữ liệu ảnh để giải mã
    $imageData = str_replace('data:image/png;base64,', '', $imageData); // Loại bỏ phần header của base64
    $imageData = str_replace(' ', '+', $imageData); // Thay thế các khoảng trắng

    // Giải mã dữ liệu base64 thành dữ liệu nhị phân của ảnh
    $imageBinary = base64_decode($imageData);

    $uploadPath = dirname(__FILE__, 2) . "/Images/";

    // Tạo thư mục nếu chưa tồn tại
    if (!file_exists($uploadPath)) {
        mkdir($uploadPath, 0777, true);
    }

    $filename = 'photo_' . date('Y-m-d-H-i-s') . '.png'; // Định dạng tên file
    $filepath = $uploadPath . $filename;

    // Lưu ảnh vào thư mục
    if (file_put_contents($filepath, $imageBinary) !== false) {
        echo $filepath;
    } else {
        echo 'Lỗi khi lưu file.'; 
    }
} else {
    echo 'Không có dữ liệu ảnh được gửi lên.';
}
?>