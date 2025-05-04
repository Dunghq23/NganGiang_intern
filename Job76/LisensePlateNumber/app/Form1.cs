using System.Diagnostics;

namespace LisensePlateNumber
{
    public partial class Form1 : Form
    {
        string excutePath = AppDomain.CurrentDomain.BaseDirectory;
        string ROOTDIR;
        string pythonExe;
        string scriptPath;
        string selectedImagePath = "";

        public Form1()
        {
            InitializeComponent();
            ROOTDIR = Path.GetFullPath(Path.Combine(excutePath, @"..\..\.."));  // Gốc dự án
            pythonExe = Path.Combine(ROOTDIR, "tf_env", "Scripts", "python.exe");
            scriptPath = Path.Combine(ROOTDIR, "python", "main.py");
        }

        private void btnChooseImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Chọn hình ảnh biển số";
                openFileDialog.Filter = "Image files (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp";
                txbLPN.Text = ""; 

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    selectedImagePath = openFileDialog.FileName;
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                    pictureBox1.Image = new Bitmap(selectedImagePath);
                }
            }
        }

        private async void btnRecognize_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(selectedImagePath))
            {
                string result = await RunPythonScriptAsync(scriptPath, selectedImagePath);
                txbLPN.Text = result;
            }
            else
            {
                MessageBox.Show("Vui lòng chọn ảnh trước khi nhận diện!");
            }
        }

        // Chạy Python script một cách bất đồng bộ
        private async Task<string> RunPythonScriptAsync(string scriptPath, string imagePath)
        {
            try
            {
                // Đường dẫn các file và thư mục
                //string modelPath = Path.Combine(ROOTDIR, "python", "models", "model_nhan_dang_bien_so_final.h5");
                string modelPath = Path.Combine(ROOTDIR, "python", "models", "best_model3.h5");
                string yoloPath = Path.Combine(ROOTDIR, "python", "models", "License-plate-detection.pt");
                string croppedDir = Path.Combine(ROOTDIR, "app", "Resources", "Images", "cropped");
                string charOutputDir = Path.Combine(ROOTDIR, "app", "Resources", "Images", "chars");
                string lisenseImageSave = Path.Combine(ROOTDIR, "app", "Resources", "Images", "Lisense");

                EnsureDirectoriesExist(croppedDir, charOutputDir, lisenseImageSave);
                string arguments = BuildPythonArguments(scriptPath, imagePath, modelPath, yoloPath, croppedDir, charOutputDir, lisenseImageSave);
                string output = await RunProcessAsync(pythonExe, arguments);

                if (!string.IsNullOrWhiteSpace(output))
                {
                    return output;
                }
                else
                {
                    return "⚠️ Không có kết quả nhận diện từ Python.";
                }
            }
            catch (Exception ex)
            {
                return "❌ Lỗi khi chạy script Python: " + ex.Message;
            }
        }

        // Đảm bảo các thư mục cần thiết tồn tại
        private void EnsureDirectoriesExist(params string[] directories)
        {
            foreach (var dir in directories)
            {
                Directory.CreateDirectory(dir);
            }
        }

        // Xây dựng chuỗi tham số cho Python script
        private string BuildPythonArguments(string scriptPath, string imagePath, string modelPath, string yoloPath, string croppedDir, string charOutputDir, string lisenseImageSave)
        {
            return $"\"{scriptPath}\" \"{imagePath}\" " +
                   $"--model_path \"{modelPath}\" " +
                   $"--yolo_path \"{yoloPath}\" " +
                   $"--cropped_dir \"{croppedDir}\" " +
                   $"--char_output_dir \"{charOutputDir}\" " +
                   $"--output_image_dir \"{lisenseImageSave}\"";
        }

        // Chạy một tiến trình Python không đồng bộ
        private async Task<string> RunProcessAsync(string fileName, string arguments)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            using (var process = new Process { StartInfo = startInfo })
            {
                process.Start();

                // Đọc kết quả từ StandardOutput và StandardError
                string output = await process.StandardOutput.ReadToEndAsync();
                string error = await process.StandardError.ReadToEndAsync();

                await process.WaitForExitAsync();

                if (!string.IsNullOrWhiteSpace(error))
                {
                    MessageBox.Show("⚠️ Lỗi từ Python:\n" + error);
                }

                return output;
            }
        }
    }
}
