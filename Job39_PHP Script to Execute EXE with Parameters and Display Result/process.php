<?php
if ($_SERVER["REQUEST_METHOD"] == "POST") {
    $num1 = isset($_POST['num1']) ? intval($_POST['num1']) : 0;
    $num2 = isset($_POST['num2']) ? intval($_POST['num2']) : 0;

    // OPTION 1: Dùng đường dẫn tương đối
    // $exe_relative_path = 'C++\Sum.exe'; // Đường dẫn tương đối tới file thực thi 
    $exe_relative_path = 'SumForm\bin\Debug\SumForm.exe';
    $result_relative_path = 'result.txt';

    // OPTION 2: Dùng bằng biến siêu toàn cục trong PHP chứa đường dẫn tới thư mục gốc của tài liệu trên máy chủ web.
    $exe_path = $_SERVER['DOCUMENT_ROOT'] . '/Job39/' . $exe_relative_path;
    
    // Kiểm tra xem file .exe có tồn tại và có quyền chạy
    if (file_exists($exe_path) && is_executable($exe_path)) {
        // Chạy file .exe với 2 tham số
        $command = escapeshellcmd("$exe_path $num1 $num2");
        exec($command, $output, $return_var);

        if ($return_var === 0) {
            // Đường dẫn đến file kết quả
            $result_file = $_SERVER['DOCUMENT_ROOT'] . '/Job39/' . $result_relative_path; // Đảm bảo file kết quả ở nơi bạn mong muốn

            // Đọc nội dung file kết quả và echo ra ngoài trình duyệt
            if (file_exists($result_file)) {
                $result = file_get_contents($result_file);

                if ($result !== false) {
                    echo "<div class='alert alert-info'>The sum of " . htmlspecialchars($num1) . " and " . htmlspecialchars($num2) . " is: " . htmlspecialchars(trim($result)) . "</div>";
                } else {
                    echo "<div class='alert alert-danger'>Failed to read result.txt.</div>";
                }
            } else {
                echo "<div class='alert alert-danger'>result.txt not found.</div>";
            }
        } else {
            echo "<div class='alert alert-danger'>Failed to execute sum.exe with return code $return_var.</div>";
        }
    } else {
        echo "<div class='alert alert-danger'>Executable file not found or permission issue.</div>";
    }
}
?>