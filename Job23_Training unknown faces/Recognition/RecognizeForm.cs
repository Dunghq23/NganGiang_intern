using AForge.Video.DirectShow;
using AForge.Video;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

using AForge.Video.DirectShow;
using AForge.Video;
using System.Threading;
using System.Xml.Linq;

namespace Recognition
{
    public partial class RecognizeForm : Form
    {
        private string selectedFilePathTrain = null;
        private string selectedFilePathRecognize = null;
        private string pythonExePath = @"C:/Users/Admin/AppData/Local/Microsoft/WindowsApps/python3.12.exe";
        string pythonScriptPath = Path.Combine("..", "..", "python", "main.py");
        string currentDirectory = Environment.CurrentDirectory;
        private string tempFolderPath = Path.Combine(Environment.CurrentDirectory, "temp");
        private bool isCameraRunning = false;
        private bool isRecognizing = false;
        private FilterInfoCollection videoDevices;
        private VideoCaptureDevice videoSource;

        public RecognizeForm()
        {
            InitializeComponent();
        }


        /////////////////////////////////// Nhóm hàm liên quan đến việc đóng mở form, hiển thị, lõi python ///////////////////////////////////

        private void RecognizeForm_Load(object sender, EventArgs e)
        {
            // Tạo một FilterInfoCollection để lưu danh sách các thiết bị đầu vào video
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            // Kiểm tra nếu không có camera nào được tìm thấy
            if (videoDevices.Count == 0)
            {
                // Hiển thị thông báo và tắt nút Camera nếu không có camera
                MessageBox.Show("Không tìm thấy camera.");
                btnCamera.Enabled = false; // Tắt nút nếu không tìm thấy camera
                return; // Thoát khỏi sự kiện nếu không có camera
            }

            // Kiểm tra xem thư mục tạm có tồn tại không
            if (!Directory.Exists(tempFolderPath))
            {
                // Tạo thư mục tạm nếu không tồn tại
                Directory.CreateDirectory(tempFolderPath);
            }
        }

        private void RecognizeForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Dừng camera khi đóng form
            if (videoSource != null && videoSource.IsRunning)
            {
                videoSource.SignalToStop();
                videoSource.WaitForStop();
            }
            // Dừng tiến trình nhận diện nếu đang chạy
            isRecognizing = false;

        }

        private string RunPythonScript(string cmd, string args)
        {
            ProcessStartInfo start = new ProcessStartInfo
            {
                FileName = cmd,
                Arguments = args,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true // Không hiển thị cửa sổ dòng lệnh
            };

            // Khởi chạy tiến trình
            using (Process process = Process.Start(start))
            {
                // Đọc đầu ra tiêu chuẩn của tiến trình
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    // Hiển thị đầu ra trên màn hình console
                    Console.WriteLine(result);
                    return result;
                }
            }
        }

        private void StopCamera()
        {
            if (isCameraRunning)
            {
                if (btnCamera.InvokeRequired)
                {
                    // Gọi lại phương thức StopCamera trên UI thread
                    btnCamera.Invoke(new Action(StopCamera));
                }
                else
                {
                    if (videoSource != null && videoSource.IsRunning)
                    {
                        videoSource.Stop();
                        picBoxRecognize.Image = null; // Clear the PictureBox
                    }
                    isCameraRunning = false;
                    btnCamera.Text = "Mở Camera"; // Change button text
                }
            }
        }


        private void DisplayImageInPictureBox(string file_path, PictureBox PicBox)
        {
            // Đọc ảnh từ tệp
            Image originalImage = Image.FromFile(file_path);

            // Kích thước của picBoxTrain
            int picBoxWidth = PicBox.Width;
            int picBoxHeight = PicBox.Height;

            // Tính toán chiều rộng và chiều cao mới dựa trên kích thước của picBoxTrain
            int newWidth = originalImage.Width;
            int newHeight = originalImage.Height;

            if (newWidth > picBoxWidth)
            {
                newWidth = picBoxWidth;
                newHeight = (int)((double)newWidth / originalImage.Width * originalImage.Height);
            }

            if (newHeight > picBoxHeight)
            {
                newHeight = picBoxHeight;
                newWidth = (int)((double)newHeight / originalImage.Height * originalImage.Width);
            }

            // Tạo ảnh mới với kích thước mới
            Image resizedImage = ResizeImage(originalImage, newWidth, newHeight);

            // Tính toán vị trí để căn giữa ảnh trong picBoxTrain
            int x = (picBoxWidth - newWidth) / 2;
            int y = (picBoxHeight - newHeight) / 2;

            // Tạo một Bitmap mới với kích thước của picBoxTrain
            Bitmap centeredBitmap = new Bitmap(picBoxWidth, picBoxHeight);

            // Tạo một đối tượng Graphics để vẽ ảnh mới vào vị trí giữa
            using (Graphics g = Graphics.FromImage(centeredBitmap))
            {
                g.DrawImage(resizedImage, x, y, newWidth, newHeight);
            }

            // Hiển thị ảnh trong PictureBox
            PicBox.Image = centeredBitmap;
            originalImage.Dispose();
            resizedImage.Dispose();
        }

        //////////////////////////////////////////// Nhóm hàm liên quan đến việc nhận diện ////////////////////////////////////////////
        private void videoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            // Lấy khung hình từ camera
            Bitmap frame = (Bitmap)eventArgs.Frame.Clone();

            // Lật ngang khung hình
            frame.RotateFlip(RotateFlipType.RotateNoneFlipX);

            // Hiển thị khung hình đã lật ngang trên PictureBox với chế độ Zoom
            picBoxRecognize.SizeMode = PictureBoxSizeMode.Zoom;
            picBoxRecognize.Image = frame;
        }
        private Image ResizeImage(Image image, int maxWidth, int maxHeight)
        {
            float aspectRatio = (float)image.Width / image.Height;

            // Tính toán kích thước mới dựa trên tỷ lệ và giới hạn kích thước
            int calculatedWidth = Math.Min(maxWidth, (int)(maxHeight * aspectRatio));
            int calculatedHeight = Math.Min(maxHeight, (int)(maxWidth / aspectRatio));

            // Kiểm tra nếu kích thước mới vượt quá kích thước ban đầu
            if (calculatedWidth > image.Width || calculatedHeight > image.Height)
            {
                calculatedWidth = image.Width;
                calculatedHeight = image.Height;
            }

            Bitmap resizedBitmap = new Bitmap(calculatedWidth, calculatedHeight);

            using (Graphics g = Graphics.FromImage(resizedBitmap))
            {
                g.DrawImage(image, 0, 0, calculatedWidth, calculatedHeight);
            }
            return resizedBitmap;
        }
        private void UpdateRecognitionTextBox(string text)
        {
            if (txbRecognize.InvokeRequired)
            {
                txbRecognize.Invoke(new Action<string>(UpdateRecognitionTextBox), text);
            }
            else
            {
                txbRecognize.Text = text;
            }
        }

        private void RunRecognition(Image imgPath)
        {
            string fullPathPythonScript = Path.Combine(currentDirectory, pythonScriptPath).Replace('\\', '/');
            string encodingFilePath = Path.Combine("..", "..", "Models", "encodings.txt").Replace('\\', '/');

            string ordinImage = Path.Combine("temp", "original_picture.jpg");
            

            string text = RunPythonScript("python", $"\"{fullPathPythonScript}\" Recognize \"{ordinImage}\" \"{encodingFilePath}\"");

            isRecognizing = false;

            if (text.Trim() == "Unknown")
            {
                string fileName = DateTime.Now.ToString("yyyy.MM.dd_HH.mm.ss.fff") + ".jpg";
                string saveFolderPath = Path.Combine("WrongIdentification");
                string saveFilePath = Path.Combine(saveFolderPath, fileName);

                File.Copy(ordinImage, saveFilePath);
            }

            // Sử dụng Invoke để cập nhật txbRecognize.Text trên UI thread
            UpdateRecognitionTextBox(text);

            StopCamera();
            DisplayImageInPictureBox(ordinImage, picBoxRecognize);
        }

        //////////////////////////////////////////// Nhóm hàm xử lý sự kiện click ////////////////////////////////////////////

        private void btnCamera_Click(object sender, EventArgs e)
        {
            if (isCameraRunning)
            {
                StopCamera();
            }
            else
            {
                // Start the camera
                if (videoDevices == null || videoDevices.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy camera hoặc videoDevices chưa được khởi tạo.");
                    return;
                }

                // Select the first camera
                videoSource = new VideoCaptureDevice(videoDevices[0].MonikerString);
                videoSource.NewFrame += new NewFrameEventHandler(videoSource_NewFrame);

                // Start displaying the camera
                videoSource.Start();
                isCameraRunning = true;
                btnCamera.Text = "Tắt Camera"; // Change button text
                txbRecognize.Text = "";
            }
        }
        private void btnOpenImageRecognize_Click(object sender, EventArgs e)
        {
            StopCamera();

            txbRecognize.Text = null;
            ofdRecognize.Title = "Chọn ảnh đào tạo";
            ofdRecognize.Filter = "Ảnh|*.jpg;*.jpeg;*.png;*.gif";

            // Hiển thị hộp thoại mở tệp và xác nhận rằng người dùng đã chọn một tệp
            if (ofdRecognize.ShowDialog() == DialogResult.OK)
            {
                // Lấy đường dẫn đầy đủ của tệp đã chọn
                selectedFilePathRecognize = ofdRecognize.FileName;
                try
                {
                    string destinationPath = Path.Combine("temp", "original_picture.jpg");
                    File.Copy(selectedFilePathRecognize, destinationPath);
                    DisplayImageInPictureBox(selectedFilePathRecognize, picBoxRecognize);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Không thể mở ảnh: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void btnRecognize_Click(object sender, EventArgs e)
        {
            if (isCameraRunning)
            {
                // Bắt đầu quá trình chụp và nhận diện liên tục
                isRecognizing = true;

                // Tạo một luồng mới để thực hiện chụp ảnh và nhận diện liên tục
                Task.Run(() =>
                {
                    while (isRecognizing && isCameraRunning)
                    {
                        string fileName = DateTime.Now.ToString("yyyy.MM.dd_HH.mm.ss.fff") + ".jpg";
                        string dateTempPath = Path.Combine("temp", fileName);

                        // Chụp ảnh từ camera và lưu vào thư mục temp
                        if (videoSource != null && videoSource.IsRunning)
                        {
                            // Lấy frame từ camera và tạo một bản sao
                            Bitmap frame = null;

                            if (picBoxRecognize.InvokeRequired)
                            {
                                picBoxRecognize.Invoke(new MethodInvoker(() =>
                                {
                                    frame = (Bitmap)picBoxRecognize.Image.Clone();
                                }));
                            }
                            else
                            {
                                frame = (Bitmap)picBoxRecognize.Image.Clone();
                            }

                            // Lưu frame vào thư mục temp
                            if (frame != null)
                            {
                                frame.Save(dateTempPath, ImageFormat.Jpeg);
                                frame.Save(Path.Combine("temp", "original_picture.jpg"), ImageFormat.Jpeg);
                                // Giải phóng frame sau khi sử dụng
                                frame.Dispose();
                            }
                        }

                        // Nhận diện từ ảnh vừa chụp
                        if (!string.IsNullOrEmpty(dateTempPath))
                        {
                            Image img = Image.FromFile(dateTempPath);
                            RunRecognition(img);
                        }

                        // Thêm thời gian nghỉ để tránh quá tải hệ thống (có thể điều chỉnh)
                        Thread.Sleep(1000);
                    }
                });
            }
            else if (selectedFilePathRecognize != null)
            {
                Image img = Image.FromFile(selectedFilePathRecognize);
                
                RunRecognition(img);
            }
            else
            {
                MessageBox.Show("Vui lòng mở camera hoặc chọn ảnh trước khi nhận diện.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Dừng quá trình nhận diện liên tục khi camera đóng
                isRecognizing = false;
            }
        }

    }
}
