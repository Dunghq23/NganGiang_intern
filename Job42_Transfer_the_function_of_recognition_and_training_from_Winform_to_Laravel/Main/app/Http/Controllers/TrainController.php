<?php

namespace App\Http\Controllers;

use Illuminate\Http\Request;
use Illuminate\Support\Facades\Storage;
use Illuminate\Support\Facades\File;


class TrainController extends Controller
{

    public function savePhoto(Request $request)
    {
        if ($request->ajax()) {
            if ($request->has('imageBase64')) {
                $imageData = $request->input('imageBase64');
                $count = $request->input('count');

                // Chuẩn bị dữ liệu ảnh để giải mã
                $imageData = str_replace('data:image/png;base64,', '', $imageData); // Loại bỏ phần header của base64
                $imageData = str_replace(' ', '+', $imageData); // Thay thế các khoảng trắng

                // Giải mã dữ liệu base64 thành dữ liệu nhị phân của ảnh
                $imageBinary = base64_decode($imageData);

                // Tạo tên file duy nhất
                $filename = 'photo_' . $count . '.png';

                // Đường dẫn tới thư mục public
                $publicPath = public_path('imageTrain');

                // Tạo thư mục nếu chưa tồn tại
                if (!file_exists($publicPath)) {
                    mkdir($publicPath, 0777, true);
                }

                // Đường dẫn đầy đủ của file
                $filePath = $publicPath . '/' . $filename;

                // Lưu ảnh vào thư mục public
                file_put_contents($filePath, $imageBinary);

                // Trả về URL của file đã lưu
                $fileUrl = asset('imageTrain/' . $filename);

                return response()->json(['fileUrl' => $fileUrl]);
            } else {
                return response()->json(['error' => 'Không có dữ liệu ảnh được gửi lên.'], 400);
            }
        }
    }


    public function TrainAllFace(Request $request)
    {
        ini_set('max_execution_time', 300);
        if ($request->ajax()) {
            $username = $request->input('username');

            // Kiểm tra và lấy đường dẫn chính xác của các file và script Python
            $pythonScriptPath = storage_path('app/python/FaceRecognition.py');
            $encodingPath = storage_path('app/models/encodings.txt');
            $publicPath = public_path('imageTrain');

            $commands = [];
            $output = [];
            $returnVars = [];

            for ($i = 1; $i <= 50; $i++) {
                $imagePath = $publicPath . '/photo_' . $i . '.png';
                $command = escapeshellcmd("python $pythonScriptPath encode_images $imagePath $encodingPath $username");

                // Lưu lệnh vào mảng để trả về sau
                $commands[] = $command;

                // Thực hiện lệnh và lưu kết quả
                exec($command, $output[], $returnVars[]);
            }

            return response()->json([
                'status' => 'success',
                'commands' => $commands,
                'output' => $output,
                'returnVars' => $returnVars,
                'encodingPath' => $encodingPath,
                'pythonScriptPath' => $pythonScriptPath,
            ]);
        }
    }

    public function TrainFace(Request $request)
    {
        if ($request->ajax()) {
            $username = $request->input('username');
            $filePath = $request->input('filePath');
            $directory = public_path('Storage/ImageUnknown');
            $imagePath = str_replace(asset(''), $directory, url($filePath));
            $imagePath = str_replace('ImageUnknownStorage', '', $imagePath);


            // Kiểm tra và lấy đường dẫn chính xác của các file và script Python
            $pythonScriptPath = storage_path('app/python/FaceRecognition.py');
            $encodingPath = storage_path('app/models/encodings.txt');

            $command = "python $pythonScriptPath encode_images $imagePath $encodingPath $username";
            exec($command, $output, $returnVars);

            return response()->json([
                'commands' => $command,
                'imagePath' => $imagePath,
            ]);
        }
    }

    public function UnknownList()
    {
        // Lấy danh sách các tệp tin từ thư mục public/Storage/ImageUnknown
        $directory = public_path('Storage/ImageUnknown');
        $files = scandir($directory);

        // Khởi tạo mảng để lưu danh sách nhận dạng chưa biết
        $unknownRecognitions = [];

        // Lặp qua các tệp tin để lấy thông tin
        foreach ($files as $index => $file) {
            // Bỏ qua các thư mục và tệp tin ẩn
            if ($file == '.' || $file == '..' || is_dir($directory . '/' . $file)) {
                continue;
            }

            // Tạo đường dẫn tuyệt đối đến tệp tin
            $filePath = $directory . '/' . $file;

            // Tạo một mẫu dữ liệu để thêm vào danh sách
            $recognition = [
                'id' => $index - 1, // Tạo ID dựa trên chỉ số của mảng
                'image' => asset('Storage/ImageUnknown/' . $file), // Lấy URL tuyệt đối của ảnh từ thư mục public
                'date' => date('Y-m-d', filemtime($filePath)) // Lấy ngày sửa đổi của tệp tin
            ];

            // Thêm mẫu dữ liệu vào danh sách nhận dạng chưa biết
            $unknownRecognitions[] = $recognition;
        }

        // Trả về view và truyền dữ liệu vào view
        return view('auth.unknownList', compact('unknownRecognitions'));
    }

    public function deleteImages()
    {
        $imageTrainPath = public_path('imageTrain');

        if (File::isDirectory($imageTrainPath)) {
            File::cleanDirectory($imageTrainPath);
        }

        return response()->json(['message' => 'Deleted all images successfully']);
    }

    public function deleteImage(Request $request)
    {
        if ($request->ajax()) {
            if ($request->has('filePath')) {
                $filePath = $request->input('filePath');

                // Chuyển URL thành đường dẫn tệp
                $relativeFilePath = str_replace(asset(''), '', $filePath);
                $relativeFilePath = ltrim($relativeFilePath, '/');

                // Kiểm tra và xóa file
                if (File::exists(public_path($relativeFilePath))) {
                    File::delete(public_path($relativeFilePath));
                    return response()->json(['success' => 'File đã được xóa.']);
                } else {
                    return response()->json(['error' => 'File không tồn tại.'], 404);
                }
            } else {
                return response()->json(['error' => 'Không có đường dẫn file được gửi lên.'], 400);
            }
        } else {
            abort(403, 'Unauthorized action.');
        }
    }


}