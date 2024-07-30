<?php

namespace App\Http\Controllers;

use App\Models\User;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\DB;
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
        $username = $request->input('username');
        $password = $request->input('password');

        // Sử dụng DB::table để truy vấn dữ liệu từ bảng 'dbo.User' và áp dụng điều kiện where
        $user = DB::table('dbo.User')
            ->where('UserName', $username)
            ->first();

        if ($user) {
            // So sánh mật khẩu
            if ($password === $user->Password) {
                return redirect('/')->with(['message' => 'Đăng nhập thành công', 'type' => 'success']);

            } else {
                return back()->with([
                    'message' => 'Mật khẩu không chính xác.',
                    'type' => 'danger'
                ]);
            }
        } else {
            return back()->with([
                'message' => 'Tên đăng nhập không chính xác.',
                'type' => 'danger'
            ]);
        }
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

                $command = escapeshellcmd("python $pythonScriptPath recognize_faces $imagePath $encodingPath $outputPath");

                // Thực hiện lệnh và lưu kết quả
                exec($command, $output[], $returnVars[]);

                // Đọc kết quả từ file output
                if (file_exists($outputPath)) {
                    $recognizedName = trim(file_get_contents($outputPath));

                    if ($recognizedName === 'Unknown') {
                        // Nếu nhận dạng là "Unknown", di chuyển ảnh vào thư mục public/Storage/ImageUnknown
                        $newImagePath = public_path('Storage/ImageUnknown') . '/' . basename($imagePath);
                        copy($imagePath, $newImagePath);
                    }

                    return response()->json([
                        'command' => $command,
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

    public function loginWithFace(Request $request)
    {
        $username = $request->input('username');
        $user = DB::table('dbo.User')
            ->where('UserName', $username)
            ->first();

        if ($user) {
            return redirect('/')->with(['message' => 'Đăng nhập thành công', 'type' => 'success']);
        }
    }
}