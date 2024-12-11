using Emgu.CV;
using VehicleDetection.src.CSharp.Models;
using VehicleDetection.src.CSharp.Services;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IO.Pipes;
using Grpc.Net.Client;
using System.CodeDom.Compiler;
using VehicleDetection_8._0_.src.CSharp.Services;


namespace VehicleDetection_8._0_
{
    public partial class MainForm : Form
    {
        #region Thuộc tính và Khởi tạo
        private readonly ImageExtractor _imageExtractor;
        private readonly string _rootDir;
        private readonly string _extractImageFolder;
        private string _videoPath;
        private int _frameSkipQuantity;
        private Dictionary<string, DetectionResult> frameTimeExecute = new Dictionary<string, DetectionResult>();
        private string host = "127.0.0.1";
        private int port = 5000;
        private Stopwatch stopwatch = new Stopwatch();
        private string _logFilePath;
        PythonExecutor pythonExecutor;
        private string _modelPath;

        public MainForm()
        {
            InitializeComponent();
            _rootDir = Path.GetFullPath(Path.Combine("..", "..", ".."));
            _extractImageFolder = Path.Combine(_rootDir, "resources", "Image", "ExtractFromVideo");
            _logFilePath = Path.Combine(_rootDir, "resources", "Logs", "Log.txt");
            _modelPath = Path.Combine(_rootDir, "model", "yolov8n.pt");
            _imageExtractor = new ImageExtractor(_extractImageFolder);
            _frameSkipQuantity = (int)nmrframeSkip.Value;
        }
        #endregion

        private async void Form1_Load(object sender, EventArgs e)
        {
            string scriptPath = Path.Combine(_rootDir, "src", "Python", "GRPCServer.py");
            string modelPath = Path.Combine(_rootDir, "model", "yolov8n.pt");
            string outputPath = Path.Combine(_rootDir, "resources", "Image", "OutputDetection", "vehicle_image_detected.jpg");

            //var pythonExecutor = new PythonExecutor("python", scriptPath);
            //string arr = "";
            //var result = pythonExecutor.Execute(arr);

            pythonExecutor = new PythonExecutor("python", scriptPath);
            pythonExecutor.Execute("");
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //PortKiller.KillAllProcessesAndConnectionsOnPort(50051);
        }

        #region Chọn File Video
        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog
            {
                Title = "Chọn File Video",
                Filter = "Video Files|*.mp4;*.avi;*.mov;*.mkv|All Files|*.*"
            })
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    _videoPath = openFileDialog.FileName;
                    wmpVideo.URL = _videoPath;
                    wmpVideo.Ctlcontrols.play();
                }
            }
        }
        #endregion

        #region Trích xuất Hình ảnh
        private void btnExtractImages_Click(object sender, EventArgs e)
        {
            if (_videoPath == null)
            {
                MessageBox.Show("Vui lòng chọn Video trước khi trích xuất!");
                return;
            }

            stopwatch = Stopwatch.StartNew();
            Task.Factory.StartNew(() => ProcessImage());
        }

        private async Task ProcessImage()
        {
            ImageExtractor imageExtractor = new ImageExtractor(_extractImageFolder);
            imageExtractor.ExtractImages(_videoPath, _frameSkipQuantity);

            // Các phần mở rộng của file ảnh mà bạn muốn lấy
            string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".bmp", ".gif", ".tiff" };

            try
            {
                DetectionResult detectionResult = new DetectionResult();
                // Lấy danh sách file từ thư mục
                string[] allFiles = Directory.GetFiles(_extractImageFolder);

                // Lọc file ảnh dựa trên phần mở rộng
                foreach (string file in allFiles)
                {
                    stopwatch = Stopwatch.StartNew();
                    stopwatch.Start();

                    //var results = await SendImagePathToApi(file);
                    var results = await gRpc(file);

                    stopwatch.Stop();

                    // Cập nhật kết quả vào dictionary
                    //foreach (var box in results.boundingBoxes) // Sử dụng flask
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

                    Invoke(new Action(() => DisplayImageWithBoundingBoxes(file, results)));
                    Invoke(new Action(() => pictureBox1.Refresh()));


                    detectionResult.TotalTime = (double)stopwatch.Elapsed.TotalSeconds;
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

        // Gửi yêu cầu POST đến Flask API
        private async Task<(List<dynamic> boundingBoxes, string receiveTime, double processTime)> SendImagePathToApi(string imagePath)
        {
            string apiUrl = "http://localhost:5000/detect";
            Stopwatch test = new Stopwatch();
            test = Stopwatch.StartNew();
            using (var client = new HttpClient())
            {
                test.Start();
                var requestData = new { image_path = imagePath };
                string json = JsonConvert.SerializeObject(requestData);
                test.Stop();
                double time1 = (double)test.Elapsed.TotalSeconds;

                DateTime sendTime = DateTime.UtcNow; // Thời điểm gửi yêu cầu (UTC)
                Console.WriteLine(sendTime.ToString());

                test = Stopwatch.StartNew();
                _logFilePath = Path.Combine(_rootDir, "resources", "Logs", "Log.txt");
                WriteLog(_logFilePath, $"[CSharp] Send Time (UTC): {sendTime:o}");

                test.Start();
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                test.Stop();
                double time2 = (double)test.Elapsed.TotalSeconds;

                if (response.IsSuccessStatusCode)
                {
                    string responseJson = await response.Content.ReadAsStringAsync();

                    // Trả về danh sách dynamic để xử lý trực tiếp các trường
                    //return JsonConvert.DeserializeObject<List<dynamic>>(responseJson);


                    dynamic result = JsonConvert.DeserializeObject<dynamic>(responseJson);

                    // Lấy boundingBoxes, receive_time và process_time từ phản hồi
                    var boundingBoxes = JsonConvert.DeserializeObject<List<dynamic>>(result["detections"].ToString());
                    string receiveTime = result["receive_time"];
                    double processTime = result["process_time"];

                    WriteLog(_logFilePath, $"[Python] Receive Time (UTC): {receiveTime}");
                    WriteLog(_logFilePath, $"[Python] Process Time (UTC): {processTime}");
                    WriteLog(_logFilePath, $"[CSharp] Response received successfully from {apiUrl}. Status: {response.StatusCode}");

                    return (boundingBoxes, receiveTime, processTime);
                }
                else
                {
                    throw new Exception($"API Error: {response.StatusCode}");
                }
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
        private void DisplayImageWithBoundingBoxes(string filePath, dynamic boundingBoxes)
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
                            g.DrawString(box.label.ToString(), new Font("Arial", 12), new SolidBrush(ColorTranslator.FromHtml("#33FF66")), x, y - 24);
                        }
                    }
                }
                pictureBox1.Image = bitmap;
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }

        private void nmrframeSkip_ValueChanged(object sender, EventArgs e)
        {
            _frameSkipQuantity = (int)nmrframeSkip.Value;
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
        #region Ghi Log
        private void WriteLog(string filePath, string message)
        {
            try
            {
                // Kiểm tra và tạo file nếu chưa tồn tại
                if (!File.Exists(filePath))
                {
                    using (var fileStream = File.Create(filePath))
                    {
                        // Đảm bảo file được tạo trước khi đóng
                    }
                }

                // Ghi log với timestamp
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] {message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi ghi log: {ex.Message}");
            }
        }
        #endregion
    }
}
