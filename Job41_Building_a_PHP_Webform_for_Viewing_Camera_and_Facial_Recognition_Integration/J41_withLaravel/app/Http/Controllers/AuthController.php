<?php

namespace App\Http\Controllers;

use Illuminate\Http\Request;
use Illuminate\Support\Facades\Storage;

class AuthController extends Controller
{
    // Hiển thị form đăng nhập
    public function showLoginForm()
    {
        return view('auth.login');
    }

    // Xử lý đăng nhập
    public function login(Request $request)
    {
        // Logic xử lý đăng nhập ở đây
        // Ví dụ: kiểm tra thông tin đăng nhập

        // Nếu đăng nhập thành công
        return redirect('/welcome'); // Thay thế '/dashboard' bằng route bạn muốn chuyển hướng đến khi đăng nhập thành công

        // Nếu đăng nhập thất bại
        // return back()->withInput()->withErrors(['loginError' => 'Đăng nhập không thành công']);
    }

    public function savePhoto(Request $request)
    {
        if ($request->ajax()) {
            if ($request->has('imageBase64')) {
                $imageData = $request->input('imageBase64');

                // Chuẩn bị dữ liệu ảnh để giải mã
                $imageData = str_replace('data:image/png;base64,', '', $imageData); // Loại bỏ phần header của base64
                $imageData = str_replace(' ', '+', $imageData); // Thay thế các khoảng trắng

                // Giải mã dữ liệu base64 thành dữ liệu nhị phân của ảnh
                $imageBinary = base64_decode($imageData);

                // Đường dẫn tới thư mục trong storage/app
                $uploadPath = 'ImageRecognize/';

                // Tạo thư mục nếu chưa tồn tại
                if (!Storage::exists($uploadPath)) {
                    Storage::makeDirectory($uploadPath, 0777, true, true);
                }

                // Tạo tên file duy nhất
                $filename = 'photo_' . date('Y-m-d-H-i-s') . '.png';

                // Lưu ảnh vào thư mục trong storage
                Storage::put($uploadPath . $filename, $imageBinary);

                // Lấy đường dẫn tuyệt đối của file đã lưu
                $filePath = Storage::path($uploadPath . $filename);

                // Trả về đường dẫn tuyệt đối
                return response()->json(['filepath' => realpath($filePath)]);
            } else {
                return response()->json(['error' => 'Không có dữ liệu ảnh được gửi lên.'], 400);
            }
        }
    }

    public function recognizeFace(Request $request)
    {
        if ($request->ajax()) {
            if ($request->has('imagePath')) {
                $imagePath = $request->input('imagePath');
    
                // Kiểm tra và lấy đường dẫn chính xác của các file và script Python
                $pythonScriptPath = storage_path('app/python/FaceRecognition.py');
                $encodingPath = storage_path('app/models/encodings.txt');
                $outputPath = storage_path('app/data/output.txt');
    
                // Tạo lệnh để gọi script Python
                $command = escapeshellcmd("python $pythonScriptPath recognize_faces $imagePath $encodingPath $outputPath");
    
                // Thực thi lệnh và đọc kết quả từ file output
                exec($command, $output, $return_var);
    
                if ($return_var !== 0) {
                    return response()->json(['error' => 'Có lỗi xảy ra khi thực thi script Python.']);
                }
    
                // Đọc kết quả từ file output
                if (file_exists($outputPath)) {
                    $recognizedName = file_get_contents($outputPath);
                    return response()->json([
                        'recognizedName' => $recognizedName,
                        'imagePath' => $imagePath,
                        'encodingPath' => $encodingPath,
                        'outputPath' => $outputPath,
                        'pythonScriptPath' => $pythonScriptPath,
                    ]);
                } else {
                    return response()->json(['error' => 'Không tìm thấy file kết quả.'], 404);
                }
            } else {
                return response()->json(['error' => 'Không có dữ liệu imagePath được gửi lên.'], 400);
            }
        }
    }
}