using Emgu.CV;
using Microsoft.ML.OnnxRuntime;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using VehicleDetection.src.CSharp.Models;
using VehicleDetection.src.CSharp.Services;

namespace VehicleDetection_8._0_
{
    public partial class Form1 : Form
    {
        #region Thuộc tính và Khởi tạo
        private readonly ImageExtractor _imageExtractor;
        private readonly string _rootDir;
        private readonly string _extractImageFolder;
        private string _videoPath;
        private int _frameSkipQuantity = 1;

        public Form1()
        {
            InitializeComponent();
            _rootDir = Path.GetFullPath(Path.Combine("..", "..", ".."));
            _extractImageFolder = Path.Combine(_rootDir, "resources", "Image", "ExtractFromVideo");
            _imageExtractor = new ImageExtractor(_extractImageFolder);
        }
        #endregion

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

            try
            {
                Helper.DeleteImagesInDirectory(_extractImageFolder);
                _imageExtractor.ExtractImages(_videoPath, _frameSkipQuantity);
                MessageBox.Show("Hình ảnh đã được trích xuất thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Có lỗi xảy ra: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void nmrframeSkip_ValueChanged(object sender, EventArgs e)
        {
            _frameSkipQuantity = (int)nmrframeSkip.Value;
        }
        #endregion

        #region Phát hiện Bằng Python
        private async void runPython_Click(object sender, EventArgs e)
        {
            string scriptPath = Path.Combine(_rootDir, "src", "Python", "detect.py");
            string modelPath = Path.Combine(_rootDir, "model", "yolov8n.pt");
            string outputPath = Path.Combine(_rootDir, "resources", "Image", "OutputDetection", "vehicle_image_detected.jpg");

            var pythonExecutor = new PythonExecutor("python", scriptPath);
            string[] imageFiles = Directory.GetFiles(_extractImageFolder, "*.jpg");
            int count = 0;
            lbProcessed.Text = $"Đang xử lý: {count}/{imageFiles.Length}";

            await Task.Run(() =>
            {
                foreach (var imageFile in imageFiles)
                {
                    string args = $"{imageFile} {modelPath} {outputPath}";
                    var stopwatch = Stopwatch.StartNew();
                    string result = pythonExecutor.Execute(args);
                    stopwatch.Stop();
                    double elapsedTimeMilliseconds = stopwatch.Elapsed.TotalSeconds;

                    if (string.IsNullOrWhiteSpace(result))
                    {
                        MessageBox.Show("Không thể phân tích kết quả từ Python.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    try
                    {
                        var detectionResult = JsonSerializer.Deserialize<DetectionResult>(result);
                        if (detectionResult != null)
                        {
                            Invoke(new Action(() =>
                            {
                                DisplayDetectionResult(elapsedTimeMilliseconds.ToString(), detectionResult.TotalVehicles, detectionResult.VehicleCounts);
                                DisplayProcessedImage(outputPath);
                            }));
                        }
                    }
                    catch (JsonException)
                    {
                        MessageBox.Show("Dữ liệu JSON không hợp lệ từ script Python.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    count++;
                    Invoke(new Action(() =>
                    {
                        lbProcessed.Text = $"Đã xử lý: {count}/{imageFiles.Length}";
                    }));
                }
            });

            MessageBox.Show("Hoàn thành xử lý tất cả các ảnh!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        #endregion

        #region Phát hiện Bằng ONNX
        private async void btnONNXDetection_Click(object sender, EventArgs e)
        {
            string modelPath = Path.Combine(_rootDir, "model", "yolov8n.onnx");
            string imageFolder = Path.Combine(_rootDir, "resources", "Image", "ExtractFromVideo");
            string outputFolder = Path.Combine(_rootDir, "resources", "Image", "OutputDetection");
            string yamlFilePath = Path.Combine(_rootDir, "model", "coco.yaml");

            var predictor = new YoloV8(modelPath, yamlFilePath);
            string[] imageFiles = Directory.GetFiles(imageFolder, "*.jpg");

            await ProcessImagesAsync(imageFiles, predictor, outputFolder);
        }

        private async Task ProcessImagesAsync(string[] imageFiles, YoloV8 predictor, string outputFolder)
        {
            await Task.Run(() =>
            {
                foreach (var imageFile in imageFiles)
                {
                    string outputImagePath = Path.Combine(outputFolder, Path.GetFileName(imageFile));
                    var stopwatch = Stopwatch.StartNew();
                    var results = predictor.Detect(imageFile, outputImagePath);
                    stopwatch.Stop();
                    double elapsedTimeMilliseconds = stopwatch.Elapsed.TotalSeconds;

                    UpdateDataGridView(results);
                    DisplayProcessedImage(outputImagePath);
                    Invoke(new Action(() =>
                    {
                        DisplayDetectionResult(elapsedTimeMilliseconds.ToString(), results.Values.Sum(), results);
                    }));
                }
            });
        }
        #endregion

        #region Cập nhật Giao diện Người dùng (UI)
        private void DisplayDetectionResult(string TotalTime, int TotalVehicles, Dictionary<string, int> results)
        {
            if (results == null) return;

            lbTotalTime.Text = $"Tổng thời gian thực hiện: {TotalTime} giây";
            lbTotalVehicles.Text = $"Tổng số phương tiện: {TotalVehicles}";

            dataGridView1.Columns.Clear();
            dataGridView1.Columns.Add("VehicleType", "Loại Phương Tiện");
            dataGridView1.Columns.Add("Count", "Số Lượng");

            dataGridView1.Rows.Clear();
            foreach (var result in results)
            {
                dataGridView1.Rows.Add(result.Key, result.Value);
            }
        }

        private void UpdateDataGridView(Dictionary<string, int> results)
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

        private void DisplayProcessedImage(string outputImagePath)
        {
            if (File.Exists(outputImagePath))
            {
                Invoke((Action)(() =>
                {
                    using (var img = Image.FromFile(outputImagePath))
                    {
                        pictureBox1.Image = new Bitmap(img);
                        pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                    }
                }));
            }
        }
        #endregion
    }
}
