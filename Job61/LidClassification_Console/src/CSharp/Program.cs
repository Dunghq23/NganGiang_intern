using Grpc.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using VehicleDetection.src.CSharp.Services;

namespace LidClassification_Console
{
    class Program
    {
        private static string _rootDir;
        private static string _resourceDir;
        private static string _modelDir;
        private static string _capturedImagesDir;
        private static string _modelBoxLidPath;
        private static string grpcHost = "localhost:50051"; // Địa chỉ server gRPC (không cần http://)

        static async Task Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            InitializePath();
            InitializeServerGrpc();

            string folderPath = @"D:\Documents\Work\NganGiang\HAQUANGDUNG\Job72\Image"; // Đường dẫn thư mục chứa ảnh

            // Lấy danh sách tất cả các tệp .jpg
            string[] imageFiles = Directory.GetFiles(folderPath, "*.jpg");

            // Duyệt qua từng file và in đường dẫn
            int count = 0;
            foreach (string file in imageFiles)
            {
                string imagePath = Path.Combine(file);
                Console.WriteLine($"Đang xử lý ảnh {++count}: {imagePath}"); await btnDetect(imagePath);
            }




            //string imagePath = Path.Combine(_capturedImagesDir, "Image_0001.jpg");
            //Console.WriteLine("Đang xử lý ảnh 1:"); await btnDetect(imagePath);

            //string imagePath2 = Path.Combine(_capturedImagesDir, "Image_0002.jpg");
            //Console.WriteLine("Đang xử lý ảnh 2:"); await btnDetect(imagePath2);

            //string imagePath3 = Path.Combine(_capturedImagesDir, "Image_0002.jpg");
            //Console.WriteLine("Đang xử lý ảnh 3:"); await btnDetect(imagePath3);


            Console.WriteLine("Nhấn Enter để thoát...");
            Console.ReadLine();
        }

        private static void InitializePath()
        {
            _rootDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..");
            _resourceDir = Path.Combine(_rootDir, "Resource");
            _modelDir = Path.Combine(_rootDir, "Model");
            _capturedImagesDir = Path.Combine(_resourceDir, "CapturedImages");
            //_modelBoxLidPath = Path.Combine(_modelDir, "box_lid_model_2.pt");
            //_modelBoxLidPath = Path.Combine("D:\\Documents\\Work\\NganGiang\\HAQUANGDUNG\\Job72\\Box_Lid_Model_Circle.pt");
            _modelBoxLidPath = Path.Combine("D:\\Documents\\Work\\NganGiang\\HAQUANGDUNG\\Job72\\Model_Circle.pt");

        }

        private static void InitializeServerGrpc()
        {
            string pythonScriptPath = Path.Combine(_rootDir, "src", "Python", "GRPCServer.py");
            PythonExecutor pythonExecutor = new PythonExecutor("python", pythonScriptPath);
            pythonExecutor.Execute("");
        }

        private static async Task btnDetect(string imagePath)
        {
            // Kiểm tra sự tồn tại của ảnh
            if (!File.Exists(imagePath))
            {
                Console.WriteLine("Ảnh không tồn tại: " + imagePath);
                return;
            }

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            var detections = await gRpc(imagePath);

            stopwatch.Stop();

            Console.WriteLine($"Thời gian xử lý: {stopwatch.Elapsed.TotalSeconds} giây");

            //if (isWithLid(detections))
            //{
            //    Console.WriteLine("Hộp có nắp bị lỗi");
            //}
            //else
            //{
            //    Console.WriteLine("Hộp không có nắp");
            //}

            if (isLidError(detections) == 2)
            {
                Console.WriteLine("Hộp có nắp bị lỗi");
            }
            else if (isLidError(detections) == 1)
            {
                Console.WriteLine("Hộp có nắp");
            }
            else
            {
                Console.WriteLine("Hộp không có nắp");
            }

        }

        private static async Task<List<dynamic>> gRpc(string imagePath)
        {
            try
            {
                // Địa chỉ server gRPC
                var channel = new Channel(grpcHost, ChannelCredentials.Insecure); // Tạo channel với Grpc.Core

                // Tạo client gRPC
                var client = new ImageTransfer.ImageTransferClient(channel);

                // Gửi yêu cầu tới server
                var request = new ImageRequest { Path = imagePath, ModelPath = _modelBoxLidPath };

                // Gửi yêu cầu với timeout
                using (var cts = new System.Threading.CancellationTokenSource(TimeSpan.FromSeconds(30))) // Thiết lập timeout
                {
                    var response = await client.SendImageAsync(request, cancellationToken: cts.Token);

                    // Xử lý phản hồi
                    if (response.Success)
                    {
                        //Console.WriteLine("Message: " + response.Message);

                        // Phân tích JSON thành danh sách dynamic
                        List<dynamic> detections = JsonConvert.DeserializeObject<List<dynamic>>(response.Data);

                        // Trả về danh sách phát hiện
                        return detections;
                    }
                    else
                    {
                        Console.WriteLine("Error: " + response.Message);
                        return new List<dynamic>(); // Trả về danh sách rỗng nếu có lỗi
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return new List<dynamic>(); // Trả về danh sách rỗng nếu có ngoại lệ
            }
        }

        private static bool isWithLid(dynamic boundingBoxes)
        {
            foreach (var box in boundingBoxes)
            {
                if (box.label.ToString() == "Lid_error")
                {
                    return true;
                }
            }
            return false;
        }

        private static int isLidError(dynamic boundingBoxes)
        {
            foreach (var box in boundingBoxes)
            {
                if (box.label.ToString() == "Lid_error")
                {
                    return 2;
                }
                else if (box.label.ToString() == "Lid")
                {
                    return 1;
                }
            }
            return 0;
        }
    }
}
