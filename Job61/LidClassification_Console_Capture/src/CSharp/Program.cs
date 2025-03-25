using Grpc.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;
using VehicleDetection.src.CSharp.Services;
using System.Threading;

namespace LidClassification_Console
{
    class Program
    {
        private static string _rootDir;
        private static string _resourceDir;
        private static string _modelDir;
        private static string _capturedImagesDir;
        private static string _modelBoxLidPath;
        private static string grpcHost = "localhost:50051"; // Địa chỉ server gRPC
        private static string tempImagePath;

        static async Task Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            InitializePath();
            InitializeServerGrpc();

            // Chụp ảnh từ webcam
            tempImagePath = Path.Combine(_capturedImagesDir, "TempImage.jpg");
            CaptureImage(tempImagePath);

            Console.WriteLine("Đang xử lý ảnh vừa chụp:");
            await btnDetect(tempImagePath);

            Console.WriteLine("Nhấn Enter để thoát...");
            Console.ReadLine();
        }

        private static void InitializePath()
        {
            _rootDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..");
            _resourceDir = Path.Combine(_rootDir, "Resource");
            _modelDir = Path.Combine(_rootDir, "Model");
            _capturedImagesDir = Path.Combine(_resourceDir, "CapturedImages");
            _modelBoxLidPath = Path.Combine(_modelDir, "box_lid_model_2.pt");

            // Đảm bảo thư mục tồn tại
            Directory.CreateDirectory(_capturedImagesDir);
        }

        private static void InitializeServerGrpc()
        {
            string pythonScriptPath = Path.Combine(_rootDir, "src", "Python", "GRPCServer.py");
            PythonExecutor pythonExecutor = new PythonExecutor("python", pythonScriptPath);
            pythonExecutor.Execute("");
        }

        private static void CaptureImage(string imagePath)
        {
            try
            {
                using (var capture = new VideoCapture(0)) 
                {
                    if (!capture.IsOpened)
                    {
                        Console.WriteLine("Không thể mở webcam.");
                        return;
                    }

                    using (var frame = capture.QueryFrame())
                    {
                        if (frame != null)
                        {
                            frame.Save(imagePath);
                            Console.WriteLine($"Ảnh đã được chụp và lưu tại: {imagePath}");
                        }
                        else
                        {
                            Console.WriteLine("Không thể chụp ảnh từ webcam.");
                        }
                    }
                }
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi chụp ảnh: {ex.Message}");
            }
        }

        private static async Task btnDetect(string imagePath)
        {
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

            if (isWithLid(detections))
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
                var channel = new Channel(grpcHost, ChannelCredentials.Insecure);
                var client = new ImageTransfer.ImageTransferClient(channel);
                var request = new ImageRequest { Path = imagePath, ModelPath = _modelBoxLidPath };

                using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30)))
                {
                    var response = await client.SendImageAsync(request, cancellationToken: cts.Token);
                    if (response.Success)
                    {
                        List<dynamic> detections = JsonConvert.DeserializeObject<List<dynamic>>(response.Data);
                        return detections;
                    }
                    else
                    {
                        Console.WriteLine("Error: " + response.Message);
                        return new List<dynamic>();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return new List<dynamic>();
            }
        }

        private static bool isWithLid(dynamic boundingBoxes)
        {
            foreach (var box in boundingBoxes)
            {
                if (box.label.ToString() == "Lid")
                {
                    return true;
                }
            }
            return false;
        }
    }
}