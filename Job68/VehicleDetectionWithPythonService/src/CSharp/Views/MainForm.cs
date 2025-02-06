using VehicleDetection.src.CSharp.Models;
using VehicleDetection.src.CSharp.Services;
using System.Text;
using Newtonsoft.Json;
using System.Diagnostics;
using Grpc.Net.Client;
using VehicleDetection_8._0_.src.CSharp.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        private Dictionary<string, (double X, double Y)> _previousPositions = new();


        // Đường dẫn thư mục và file
        private string _rootDir;
        private string _extractImageFolder;
        private string _logFilePath;
        private string _modelPath;
        private string _videoPath;

        // Thuộc tính liên quan tính vận tốc
        private double _laneWidthMeters = 3.5;
        private Dictionary<int, VehicleTrackingInfo> _vehicleTracking;
        private bool _calibrationDone;
        private double? _metersPerPixel;
        private int _nextId;

        #endregion

        #region Nhóm hàm khởi tạo
        public MainForm()
        {
            InitializeComponent();
            InitializePaths();
            _vehicleTracking = new Dictionary<int, VehicleTrackingInfo>();
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

                        //// Tính toán vận tốc
                        //CalculateSpeed(box);
                    }

                    Invoke(new Action(() =>
                            {
                                pictureBox1.Image = DrawBoundingBoxes(file, results);
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
        private Bitmap DrawBoundingBoxes(string filePath, dynamic boundingBoxes)
        {
            using (Image img = Image.FromFile(filePath))
            {
                Bitmap bitmap = new Bitmap(img);
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    Color customColor = ColorTranslator.FromHtml("#33FF66");
                    using (Pen pen = new Pen(customColor, 2)) // Độ dày là 2
                    {
                        foreach (var box in boundingBoxes)
                        {
                            int x = box.x, y = box.y, w = box.w, h = box.h;
                            g.DrawRectangle(pen, x, y, w, h);
                            g.DrawString(box.label.ToString(), new Font("Arial", 10), new SolidBrush(ColorTranslator.FromHtml("#33FF66")), x, y - 24);
                            g.DrawString($"{CalculateSpeed(box)} km/h", new Font("Arial", 10), new SolidBrush(ColorTranslator.FromHtml("#ff2020")), x+ 40, y - 24);
                        }
                    }
                }
                return bitmap;
            }
        }

        private double CalculateSpeed(dynamic box)
        {
            if (!_calibrationDone)
            {
                // Hiệu chuẩn nếu chưa thực hiện
                _metersPerPixel = _laneWidthMeters / 243;
                _calibrationDone = true;
            }
            // Tính tọa độ trung tâm của bounding box
            float x_center = box.x + box.w / 2;
            float y_center = box.y + box.h / 2;
            var currentPos = new PointF(x_center, y_center);


            int matchedId = -1;
            double minDist = double.MaxValue;

            // Tìm phương tiện phù hợp dựa trên khoảng cách
            foreach (var objId in _vehicleTracking.Keys)
            {
                var track = _vehicleTracking[objId];
                if (track.Positions.Any())
                {
                    var lastPos = track.Positions.Last();
                    double dist = Math.Sqrt(Math.Pow(lastPos.X - currentPos.X, 2) + Math.Pow(lastPos.Y - currentPos.Y, 2));
                    if (dist < minDist && dist < 100) // Giới hạn khoảng cách tối đa để khớp
                    {
                        minDist = dist;
                        matchedId = objId;
                    }
                }
            }

            // Nếu không tìm thấy phương tiện phù hợp, tạo ID mới
            if (matchedId == -1)
            {
                matchedId = _nextId++;
                _vehicleTracking[matchedId] = new VehicleTrackingInfo
                {
                    Type = box.Label,
                    Positions = new List<PointF>(),
                    Timestamps = new List<double>(),
                    Velocities = new List<double>()
                };
            }

            // Cập nhật thông tin theo dõi
            var vehicleTrack = _vehicleTracking[matchedId];
            vehicleTrack.Positions.Add(currentPos);
            vehicleTrack.Timestamps.Add(_stopwatch.Elapsed.TotalSeconds);

            double speed = 0.0;
            // Tính vận tốc nếu có đủ dữ liệu
            if (vehicleTrack.Positions.Count >= 2)
            {
                var prevPos = vehicleTrack.Positions[vehicleTrack.Positions.Count - 2];
                var prevTime = vehicleTrack.Timestamps[vehicleTrack.Timestamps.Count - 2];
                var currentTime = vehicleTrack.Timestamps.Last();
                //var timeDiff = currentTime - prevTime;
                double fps = 29.0; // Nếu bạn biết FPS của video
                var timeDiff = (_frameSkipQuantity + 1) / fps;


                speed = CalculateSpeed(prevPos, currentPos, timeDiff);
                vehicleTrack.Velocities.Add(speed);

                var speedText = $"Vehicle ID: {matchedId}, Type: {box.label}, Speed: {speed} km/h";
            }
            return Math.Round(speed, 2);
        }
        private double CalculateSpeed(PointF pos1, PointF pos2, double timeDiff)
        {
            if (timeDiff < 0.001)
                return 0.0;

            double dx = pos2.X - pos1.X;
            double dy = pos2.Y - pos1.Y;
            double distancePixels = Math.Sqrt(dx * dx + dy * dy);
            double distanceMeters = distancePixels * _metersPerPixel.Value;
            double speedMps = distanceMeters / timeDiff;
            double speedKmh = Math.Min(Math.Max(speedMps * 3.6, 0), 200);

            return speedKmh;
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

                    // Phân tích JSON thành danh sách BoundingBox
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
