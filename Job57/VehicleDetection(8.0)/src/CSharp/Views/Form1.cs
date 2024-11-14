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
using Emgu.CV.CvEnum;
using System.Linq;

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
        Dictionary<string, DetectionResult> frameTimeExcute = new Dictionary<string, DetectionResult>();

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

            // Run the image extraction and detection in a background thread
            Task.Run(() => ExtractAndDetectImages(_videoPath));
        }

        private void btnStopExtraction_Click(object sender, EventArgs e)
        {

        }

        private void ExtractAndDetectImages(string videoPath)
        {
            try
            {
                // Clear previous extracted images
                Helper.DeleteImagesInDirectory(_extractImageFolder);
                frameTimeExcute.Clear();

                // Initialize paths and YOLO model
                string modelPath = Path.Combine(_rootDir, "model", "yolov8n.onnx");
                string yamlFilePath = Path.Combine(_rootDir, "model", "coco.yaml");
                var predictor = new YoloV8(modelPath, yamlFilePath);

                // Open the video file
                using (var capture = new VideoCapture(videoPath))
                {
                    int totalFrames = (int)capture.Get(CapProp.FrameCount);
                    int frameCount = 0;

                    // Iterate over each frame of the video
                    while (frameCount < totalFrames)
                    {
                        using (Mat frame = new Mat())
                        {
                            capture.Read(frame);
                            if (frame.IsEmpty) break; // End of video

                            // Process frame if it matches the frame skip condition
                            if (frameCount % _frameSkipQuantity == 0)
                            {
                                var results = predictor.Detect(frame);
                                frameTimeExcute.Add($"frame_{frameCount}", results);
                                UpdatePictureBox(results.image);               // Display the detected Bitmap image
                                Invoke(new Action(() =>
                                {
                                    DisplayDetectionResult(results.TotalTime.ToString(), results.VehicleCounts.Values.Sum(), results.VehicleCounts);
                                }));
                            }
                        }
                        frameCount++;
                    }
                }

                ShowCompletionMessage("Hình ảnh đã được trích xuất thành công!");
                ExportDictionaryToFile(frameTimeExcute, @"C:\Users\HQD\Downloads\TimeExcute.txt");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Có lỗi xảy ra: {ex.Message}");
            }
        }

        static void ExportDictionaryToFile(Dictionary<string, DetectionResult> dictionary, string filePath)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    // Ghi tiêu đề cho file nếu cần
                    writer.WriteLine("Frame |   Tiền xử lý  |   Nhận diện   |   Vẽ boudingbox   |   Tổng thời gian");
                    writer.WriteLine("--------------------------------------------------");

                    // Lặp qua Dictionary và ghi từng phần tử vào file
                    foreach (var entry in dictionary)
                    {
                        writer.WriteLine($"{entry.Key}  |   {entry.Value.PreprocessTime}   |   {entry.Value.DetectTime} |   {entry.Value.DrawBoxTime}   |   {entry.Value.TotalTime}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi xuất file: {ex.Message}");
            }
        }

        // Method to update the picture box with the detected image (Bitmap) on the UI thread
        private void UpdatePictureBox(Bitmap detectedBitmap)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action(() => SetPictureBoxImage(detectedBitmap)));
            }
            else
            {
                SetPictureBoxImage(detectedBitmap);
            }
        }

        // Helper method to safely set the Bitmap image in pictureBox1
        private void SetPictureBoxImage(Bitmap newBitmap)
        {
            // Dispose of the old image to free memory
            if (pictureBox1.Image != null)
            {
                pictureBox1.Image.Dispose();
            }

            // Set the new Bitmap image
            pictureBox1.Image = new Bitmap(newBitmap);  // Clone to avoid disposal issues
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
        }

        // Method to display completion message on the UI thread
        private void ShowCompletionMessage(string message)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action(() => MessageBox.Show(message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information)));
            }
            else
            {
                MessageBox.Show(message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // Method to show error message on the UI thread
        private void ShowErrorMessage(string message)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action(() => MessageBox.Show(message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)));
            }
            else
            {
                MessageBox.Show(message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            string modelPath = Path.Combine(_rootDir, "model", "yolov8m.pt");
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
            string modelPath = Path.Combine(_rootDir, "model", "yolov8m.onnx");
            string imageFolder = Path.Combine(_rootDir, "resources", "Image", "ExtractFromVideo");
            string outputFolder = Path.Combine(_rootDir, "resources", "Image", "OutputDetection");
            string yamlFilePath = Path.Combine(_rootDir, "model", "coco.yaml");

            var predictor = new YoloV8(modelPath, yamlFilePath);
            string[] imageFiles = Directory.GetFiles(imageFolder, "*.jpg");

            //await ProcessImagesAsync(imageFiles, predictor, outputFolder);
        }

        //private async Task ProcessImagesAsync(string[] imageFiles, YoloV8 predictor, string outputFolder)
        //{
        //    await Task.Run(() =>
        //    {
        //        foreach (var imageFile in imageFiles)
        //        {
        //            string outputImagePath = Path.Combine(outputFolder, Path.GetFileName(imageFile));
        //            var stopwatch = Stopwatch.StartNew();
        //            var results = predictor.Detect(imageFile, outputImagePath);
        //            stopwatch.Stop();
        //            double elapsedTimeMilliseconds = stopwatch.Elapsed.TotalSeconds;

        //            UpdateDataGridView(results.VehicleCounts);
        //            DisplayProcessedImage(outputImagePath);
        //            Invoke(new Action(() =>
        //            {
        //                DisplayDetectionResult(elapsedTimeMilliseconds.ToString(), results.Values.Sum(), results);
        //            }));
        //        }
        //    });
        //}
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
