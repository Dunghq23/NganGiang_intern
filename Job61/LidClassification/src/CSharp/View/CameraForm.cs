using Emgu.CV;
using Emgu.CV.CvEnum;
using System.Diagnostics;

using Grpc.Net.Client;
using Newtonsoft.Json;
using VehicleDetection.src.CSharp.Services;

namespace LidClassification.View
{
    public partial class CameraForm : Form
    {
        #region Thuộc tính và khởi tạo
        // Xử lý camera
        private VideoCapture _capture;
        private Thread _captureThread;
        private bool _isCapturing;
        // Đường dẫn
        private string _rootDir;
        private string _resourceDir;
        private string _modelDir;
        private string _capturedImagesDir;
        private string _modelBoxLidPath;
        // Localhost & Port
        private string grpcHost = "http://localhost:50051";
        public CameraForm()
        {
            InitializeComponent();
            InitializePath();
            InitializeServerGrpc();
            _capture = new VideoCapture(1);
            if (!_capture.IsOpened)
            {
                _capture = new VideoCapture(0);
            }
            _capture.ImageGrabbed += ProcessFrame;
        }
        #endregion

        #region Nhóm hàm phụ trợ khởi tạo
        private void InitializePath()
        {
            _rootDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..");
            _resourceDir = Path.Combine(_rootDir, "Resource");
            _modelDir = Path.Combine(_rootDir, "Model");
            _capturedImagesDir = Path.Combine(_resourceDir, "CapturedImages");
            _modelBoxLidPath = Path.Combine(_modelDir, "box_lid_model_2.pt");
        }

        private void InitializeServerGrpc()
        {
            string pythonScriptPath = Path.Combine(_rootDir, "src", "Python", "GRPCServer.py");
            PythonExecutor pythonExecutor = new PythonExecutor("python", pythonScriptPath);
            pythonExecutor.Execute("");
        }
        #endregion

        #region Nhóm hàm sự kiện
        private void CameraForm_Load(object sender, EventArgs e)
        {
            if (!_capture.IsOpened)
            {
                MessageBox.Show("Không thể mở camera.");
            }
            else
            {
                // Lấy độ phân giải mặc định của camera
                int width = _capture.Width;
                int height = _capture.Height;

                _capture.Start();
            }
        }
        private void CameraForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _capture.Stop();
            _isCapturing = false;
            _captureThread?.Join();
            _capture?.Dispose();
        }
        private void btnStart_Click(object sender, EventArgs e)
        {
            if (!_isCapturing)
            {
                _isCapturing = true;
                _captureThread = new Thread(CaptureImages);
                _captureThread.Start();
                btnStart.Text = "Dừng chụp";
            }
            else
            {
                _isCapturing = false;
                _captureThread?.Join();
                btnStart.Text = "Bắt đầu chụp";
            }
        }
        private async void btnDetect_Click(object sender, EventArgs e)
        {
            string[] allFiles = Directory.GetFiles(_capturedImagesDir);
            int count = 0;
            Stopwatch stopwatch = new Stopwatch();

            foreach (var file in allFiles)
            {
                stopwatch = Stopwatch.StartNew();
                stopwatch.Start();
                var detections = await gRpc(file);

                // Vẽ bounding box lên ảnh
                Bitmap bitmap = Helper.DrawBoundingBoxes(file, detections);

                stopwatch.Stop();

                // Hiển thị ảnh
                pictureBox2.Image = bitmap;
                pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage; // Hoặc AutoSize

                count++;
                lbProcessed.Text = $"Đã xử lý: {count}/{allFiles.Length}";
                lbTimeProcess.Text = $"Thời gian xử lý: {stopwatch.Elapsed.TotalSeconds} ms";

                if (isWithLid(detections))
                {
                    txbCheckLid.Text = "Hộp có nắp";
                } else
                {
                    txbCheckLid.Text = "Hộp không có nắp";
                }
            }
            MessageBox.Show("Đã xử lý xong");
            //Helper.DeleteImagesInDirectory(_capturedImagesDir);
        }
        #endregion

        #region Nhóm hàm xử lý khung hình camera
        private void ProcessFrame(object sender, EventArgs e)
        {
            Mat frame = new Mat();
            _capture.Retrieve(frame);

            // Lật ảnh theo trục Y (ngang)
            Mat flippedFrame = new Mat();
            CvInvoke.Flip(frame, flippedFrame, FlipType.Horizontal); // Lật ngang theo trục Y

            pictureBox1.Image = frame.ToBitmap();
        }
        private void CaptureImages()
        {
            int count = 0;
            string outputFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "Resource", "CapturedImages");
            Directory.CreateDirectory(outputFolder);

            while (_isCapturing)
            {
                Mat frame = new Mat();
                _capture.Retrieve(frame);

                if (!frame.IsEmpty)
                {
                    string filePath = Path.Combine(outputFolder, $"Image_{count++:D4}.jpg");
                    frame.Save(filePath);
                }

                Thread.Sleep(100); // Chụp ảnh mỗi 100ms
            }
        }

        #endregion

        #region Nhóm hàm hỗ trợ
        private async Task<List<dynamic>> gRpc(string imagePath)
        {
            try
            {
                // Địa chỉ server gRPC
                using var channel = GrpcChannel.ForAddress("http://localhost:50051");

                // Tạo client
                var client = new ImageTransfer.ImageTransferClient(channel);

                // Gửi yêu cầu tới server
                var request = new ImageRequest { Path = imagePath, ModelPath = _modelBoxLidPath };
                var response = await client.SendImageAsync(request);

                // Xử lý phản hồi
                if (response.Success)
                {
                    Console.WriteLine("Message: " + response.Message);

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
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return new List<dynamic>(); // Trả về danh sách rỗng nếu có ngoại lệ
            }
        }
        private bool isWithLid(dynamic boundingBoxes)
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
        #endregion
    }
}
