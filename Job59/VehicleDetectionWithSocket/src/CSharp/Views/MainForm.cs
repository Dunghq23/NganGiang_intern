using Emgu.CV;
using VehicleDetection.src.CSharp.Models;
using VehicleDetection.src.CSharp.Services;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
using System.Diagnostics;

namespace VehicleDetection_8._0_
{
    public partial class MainForm : Form
    {
        #region Thuộc tính và Khởi tạo
        private readonly ImageExtractor _imageExtractor;
        private readonly string _rootDir;
        private readonly string _extractImageFolder;
        private string _videoPath;
        private int _frameSkipQuantity = 1;
        private Dictionary<string, DetectionResult> frameTimeExecute = new Dictionary<string, DetectionResult>();
        private string host = "127.0.0.1";
        private int port = 5000;
        private Stopwatch stopwatch = new Stopwatch();

        public MainForm()
        {
            InitializeComponent();
            _rootDir = Path.GetFullPath(Path.Combine("..", "..", ".."));
            _extractImageFolder = Path.Combine(_rootDir, "resources", "Image", "ExtractFromVideo");
            _imageExtractor = new ImageExtractor(_extractImageFolder);
        }
        #endregion

        private async void Form1_Load(object sender, EventArgs e)
        {
            string scriptPath = Path.Combine(_rootDir, "src", "Python", "SocketServer.py");
            string modelPath = Path.Combine(_rootDir, "model", "yolov8n.pt");
            string outputPath = Path.Combine(_rootDir, "resources", "Image", "OutputDetection", "vehicle_image_detected.jpg");

            var pythonExecutor = new PythonExecutor("python", scriptPath);
            string arr = "";
            var result = pythonExecutor.Execute(arr);

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

        private void ProcessImage()
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
                    if (Array.Exists(imageExtensions, ext => ext.Equals(Path.GetExtension(file), StringComparison.OrdinalIgnoreCase)))
                    {
                        // Kết nối đến Python server
                        using (TcpClient client = new TcpClient(host, port))
                        using (NetworkStream stream = client.GetStream())
                        {
                            // Đọc ảnh và chuyển sang byte[]
                            byte[] imageBytes = File.ReadAllBytes(file);
                            byte[] sizeBytes = Encoding.UTF8.GetBytes(imageBytes.Length.ToString().PadLeft(16, '0'));

                            // Gửi kích thước ảnh
                            stream.Write(sizeBytes, 0, sizeBytes.Length);

                            // Gửi ảnh
                            stream.Write(imageBytes, 0, imageBytes.Length);

                            // Nhận kết quả bounding box
                            using (MemoryStream ms = new MemoryStream())
                            {
                                byte[] buffer = new byte[4096];
                                int bytesRead;
                                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                                {
                                    ms.Write(buffer, 0, bytesRead);
                                }
                                string response = Encoding.UTF8.GetString(ms.ToArray());
                                var boundingBoxes = JsonConvert.DeserializeObject<dynamic>(response);

                                // Cập nhật kết quả vào dictionary
                                foreach (var box in boundingBoxes)
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
                                Invoke(new Action(() => DisplayImageWithBoundingBoxes(file, boundingBoxes)));
                            }
                            Invoke(new Action(() => pictureBox1.Refresh()));
                        }
                    }
                    stopwatch.Stop();
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
    }
}
