using VehicleDetection.src.CSharp.Models;
using VehicleDetection.src.CSharp.Services;
using System.Text;
using Newtonsoft.Json;
using System.Diagnostics;
using Grpc.Net.Client;

namespace VehicleDetection_8._0_
{
    public partial class MainForm : Form
    {
        #region Thuộc tính
        private PythonExecutor _pythonExecutor;
        private ImageExtractor _imageExtractor;
        private int _frameSkipQuantity;
        private Dictionary<string, DetectionResult> _frameTimeExecute = new();
        private Stopwatch _stopwatch = new();

        // Đường dẫn thư mục và file
        private string _rootDir;
        private string _extractImageFolder;
        private string _logFilePath;
        private string _modelPath;
        private string _videoPath;
        #endregion

        #region Nhóm hàm khởi tạo
        public MainForm()
        {
            InitializeComponent();
            InitializePaths();
        }
        private void InitializePaths()
        {
            _rootDir = Path.GetFullPath(Path.Combine("..", "..", ".."));
            _extractImageFolder = Path.Combine(_rootDir, "resources", "Image", "ExtractFromVideo");
            _logFilePath = Path.Combine(_rootDir, "resources", "Logs", "Log.txt");
            _modelPath = Path.Combine(_rootDir, "model", "yolov8n.pt");
            _imageExtractor = new ImageExtractor(_extractImageFolder);
            _frameSkipQuantity = (int)nmrframeSkip.Value;
        }
        #endregion

        #region Nhóm hàm sự kiện
        private async void Form1_Load(object sender, EventArgs e)
        {
            StartPythonGRPCServer();
        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //PortKiller.KillAllProcessesAndConnectionsOnPort(50051);
        }
        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            SelectVideoFile();
        }
        private void btnExtractImages_Click(object sender, EventArgs e)
        {
            if (_videoPath == null)
            {
                MessageBox.Show("Vui lòng chọn Video trước khi trích xuất!");
                return;
            }

            _stopwatch = Stopwatch.StartNew();
            Task.Factory.StartNew(() => ProcessImage());
        }
        private void nmrframeSkip_ValueChanged(object sender, EventArgs e)
        {
            _frameSkipQuantity = (int)nmrframeSkip.Value;
        }

        #endregion

        #region Nhóm hàm xử lý chính
        private void SelectVideoFile()
        {
            using var openFileDialog = new OpenFileDialog
            {
                Title = "Chọn File Video",
                Filter = "Video Files|*.mp4;*.avi;*.mov;*.mkv|All Files|*.*"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                _videoPath = openFileDialog.FileName;
                wmpVideo.URL = _videoPath;
                wmpVideo.Ctlcontrols.play();
            }
        }
        private void StartPythonGRPCServer()
        {
            string scriptPath = Path.Combine(_rootDir, "src", "Python", "GRPCServer.py");

            _pythonExecutor = new PythonExecutor("python", scriptPath);
            _pythonExecutor.Execute(string.Empty);
        }
        #endregion

        #region Trích xuất Hình ảnh
        private async Task ProcessImage()
        {
            ImageExtractor imageExtractor = new ImageExtractor(_extractImageFolder);
            imageExtractor.ExtractImages(_videoPath, _frameSkipQuantity);
            try
            {
                DetectionResult detectionResult = new DetectionResult();
                string[] allFiles = Directory.GetFiles(_extractImageFolder);

                foreach (string file in allFiles)
                {
                    _stopwatch = Stopwatch.StartNew();
                    _stopwatch.Start();

                    var results = await gRpc(file);

                    _stopwatch.Stop();

                    foreach (var box in results)
                    {
                        string label = box.label.ToString();
                        if (detectionResult.VehicleCounts.ContainsKey(label))
                        {
                            detectionResult.VehicleCounts[label]++;
                        }
                        else
                        {
                            detectionResult.VehicleCounts[label] = 1;
                        }
                    }

                    Invoke(new Action(() =>
                            {
                                pictureBox1.Image = Helper.DrawBoundingBoxes(file, results);
                                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                                pictureBox1.Refresh();
                            }
                        )
                    );

                    detectionResult.TotalTime = (double)_stopwatch.Elapsed.TotalSeconds;
                    detectionResult.TotalVehicles = (int)detectionResult.VehicleCounts.Values.Sum();
                    UpdateDataGridView(detectionResult.VehicleCounts);
                    File.Delete(file);
                    Invoke(new Action(() => DisplayDetectionResult(detectionResult)));
                    detectionResult.VehicleCounts.Clear();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Có lỗi xảy ra: {ex.Message}");
            }


        }

        private async Task<List<dynamic>> gRpc(string imagePath)
        {
            try
            {
                // Địa chỉ server gRPC
                using var channel = GrpcChannel.ForAddress("http://localhost:50051");

                // Tạo client
                var client = new ImageTransfer.ImageTransferClient(channel);

                // Gửi yêu cầu tới server
                var request = new ImageRequest { Path = imagePath, ModelPath = _modelPath };
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

        #endregion

        #region Cập nhật Giao diện Người dùng (UI)
        private void DisplayDetectionResult(DetectionResult detectionResult)
        {
            if (detectionResult.VehicleCounts == null) return;

            lbTotalTime.Text = $"Tổng thời gian thực hiện: {detectionResult.TotalTime} giây";
            lbTotalVehicles.Text = $"Tổng số phương tiện: {detectionResult.TotalVehicles}";

            dataGridView1.Columns.Clear();
            dataGridView1.Columns.Add("VehicleType", "Loại Phương Tiện");
            dataGridView1.Columns.Add("Count", "Số Lượng");

            dataGridView1.Rows.Clear();
            foreach (var result in detectionResult.VehicleCounts)
            {
                dataGridView1.Rows.Add(result.Key, result.Value);
            }
        }
        private async void UpdateDataGridView(Dictionary<string, int> results)
        {
            Invoke((Action)(() =>
            {
                dataGridView1.Columns.Clear();
                dataGridView1.Columns.Add("VehicleType", "Loại Phương Tiện");
                dataGridView1.Columns.Add("Count", "Số Lượng");

                dataGridView1.Rows.Clear();
                foreach (var result in results)
                {
                    dataGridView1.Rows.Add(result.Key, result.Value);
                }
            }));
        }
        #endregion
    }
}
