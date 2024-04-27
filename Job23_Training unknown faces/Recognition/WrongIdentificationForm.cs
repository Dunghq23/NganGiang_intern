using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Recognition
{
    public partial class WrongIdentificationForm : Form
    {

        private List<string> imagePath = new List<string>();
        private string pythonExePath = @"C:/Users/Admin/AppData/Local/Microsoft/WindowsApps/python3.12.exe";
        private string pythonScriptPath = Path.Combine("..", "..", "Python", "main.py");

        public WrongIdentificationForm()
        {
            InitializeComponent();

            // Xử lý sự kiện khi ngày được chọn thay đổi
            dtpk.ValueChanged += new EventHandler(dtpkDate_ValueChanged);

            // Hiển thị dữ liệu ban đầu với ngày được chọn mặc định là ngày hiện tại
            imagePath = DisplayDataForSelectedDate(DateTime.Today);
        }

        // Hàm xử lý sự kiện khi ngày được chọn thay đổi
        private void dtpkDate_ValueChanged(object sender, EventArgs e)
        {
            // Lấy ngày được chọn từ DateTimePicker
            DateTime selectedDate = dtpk.Value;

            // Hiển thị dữ liệu cho ngày được chọn
            imagePath = DisplayDataForSelectedDate(selectedDate);
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
        private void AddFilesToDataGridView(string[] files)
        {
            int stt = 1;
            foreach (string filePath in files)
            {
                // Thêm dòng vào DataGridView
                Image image = Image.FromFile(filePath);

                dtgvReport.Rows.Add(stt, ResizeImage(image, 512, 300), "Unknown", "", "Huấn luyện");
                stt++;
                image.Dispose();

            }
        }
        private List<string> DisplayDataForSelectedDate(DateTime selectedDate)
        {
            // Xóa tất cả các dòng hiện tại trong DataGridView và giải phóng tài nguyên hình ảnh
            ClearDataGridViewImages();

            string directoryPath = @"./WrongIdentification";
            string[] files = Directory.GetFiles(directoryPath);
            List<string> filteredFiles = new List<string>();
            foreach (string filePath in files)
            {
                // Lấy thông tin về thời gian chỉnh sửa của tệp tin
                DateTime fileLastWriteTime = File.GetLastWriteTime(filePath);

                // Kiểm tra xem tệp tin đã được chỉnh sửa vào ngày được chọn không
                if (fileLastWriteTime.Date == selectedDate.Date)
                {
                    filteredFiles.Add(filePath);
                }
            }
            AddFilesToDataGridView(filteredFiles.ToArray());

            return filteredFiles;
        }
        private List<string> DisplayDataForAll()
        {
            // Xóa tất cả các dòng hiện tại trong DataGridView và giải phóng tài nguyên hình ảnh
            ClearDataGridViewImages();
            

            string directoryPath = @"./WrongIdentification";
            string[] files = Directory.GetFiles(directoryPath);
            List<string> filteredFiles = new List<string>();
            foreach (string filePath in files)
            {
                filteredFiles.Add(filePath);
            }
            AddFilesToDataGridView(filteredFiles.ToArray());
            return filteredFiles;
        }
        private void ClearDataGridViewImages()
        {
            foreach (DataGridViewRow row in dtgvReport.Rows)
            {
                var imageCell = row.Cells["ImageIdentification"];
                if (imageCell.Value != null && imageCell.Value is Image)
                {
                    ((Image)imageCell.Value).Dispose();
                }
            }
            dtgvReport.Rows.Clear();
        }
        private void btnShowAll_Click(object sender, EventArgs e)
        {
            dtpk.Value = DateTime.Today;
            string directoryPath = @"./WrongIdentification";
            string[] files = Directory.GetFiles(directoryPath);
            imagePath = DisplayDataForAll();
        }
        private void dtgvReport_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Kiểm tra nếu cột được nhấp là cột button và hàng được nhấp không phải là hàng header
            if (e.ColumnIndex == dtgvReport.Columns["btnTrain"].Index && e.RowIndex >= 0)
            {
                // Thực hiện hành động khi nút được nhấp
                // Ví dụ: lấy dữ liệu từ hàng được nhấp
                int stt = Convert.ToInt32(dtgvReport.Rows[e.RowIndex].Cells["STT"].Value);
                string wrongName = dtgvReport.Rows[e.RowIndex].Cells["WrongIdentificationName"].Value.ToString();
                string correctName = dtgvReport.Rows[e.RowIndex].Cells["CorrectIdentificationName"].Value.ToString();



                // Kiểm tra ô "Correct" có dữ liệu hay không
                if (!string.IsNullOrEmpty(correctName))
                {
                    string filePath = imagePath[stt - 1];
                    try
                    {
                        // Lấy hàng hiện tại
                        DataGridViewRow row = dtgvReport.Rows[stt - 1];

                        // Lấy tên tệp từ đường dẫn file
                        string fileName = Path.GetFileName(filePath);

                        // Lưu trữ và giải phóng ảnh hiện tại nếu có
                        if (row.Cells["ImageIdentification"].Value is Image currentImage)
                        {
                            currentImage.Dispose(); // Giải phóng ảnh hiện tại
                        }
                        Image newImage = Image.FromFile("./Processing.jpg");
                        row.Cells["ImageIdentification"].Value = newImage;

                        // Di chuyển tệp từ thư mục hiện tại sang thư mục dataset
                        string destinationPath = Path.Combine("..", "..", "./Datasets", fileName);

                        // Di chuyển tệp và xử lý ngoại lệ nếu có
                        try
                        {
                            File.Move(filePath, destinationPath);
                            RunEncodeImagesInDataset(correctName);
                        }
                        catch (IOException ex)
                        {
                            // Xử lý ngoại lệ IOException khi di chuyển tệp
                            MessageBox.Show($"Có lỗi IO khi di chuyển tệp {filePath} đến {destinationPath}: {ex.Message}");
                        }
                        newImage.Dispose();

                    }
                    catch (IOException ex)
                    {
                        // Xử lý ngoại lệ IOException
                        MessageBox.Show($"Có lỗi IO khi thực hiện thao tác trên tệp {filePath}: {ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        // Xử lý các ngoại lệ khác
                        MessageBox.Show($"Đã xảy ra một ngoại lệ không xác định: {ex.Message}");
                    }


                    DateTime selectedDate = dtpk.Value.Date; // Lấy ngày được chọn từ DateTimePicker

                    if (selectedDate != DateTime.Today)
                    {
                        imagePath = DisplayDataForSelectedDate(selectedDate);
                    }
                    else
                    {
                        imagePath = DisplayDataForAll();
                    }

                }
                else
                {
                    // Nếu ô "Correct" không có dữ liệu, thông báo người dùng
                    MessageBox.Show("Vui lòng nhập tên trước khi huấn luyện!");
                }
            }
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
        private void RunEncodeImagesInDataset(string name)
        {

            string currentDirectory = Directory.GetCurrentDirectory();

            // Lấy thư mục bin
            DirectoryInfo binDirectory = Directory.GetParent(currentDirectory);
            string binPath = binDirectory.ToString();
            // Lấy thư mục Recognition
            DirectoryInfo recognitionDirectory = Directory.GetParent(binPath);
            string recognitionPath = recognitionDirectory.ToString();

            string fullPathPythonScript = Path.Combine(currentDirectory, pythonScriptPath).Replace('\\', '/');
            string datasetPath = Path.Combine(recognitionPath, "./Datasets").Replace('\\', '/');
            string encodingFilePath = Path.Combine(recognitionPath, "./Models/encodings.txt").Replace('\\', '/');
            string trainedPath = $"{datasetPath}/Trained";
            RunPythonScript("python", $"\"{fullPathPythonScript}\" encode_images_in_dataset \"{datasetPath}\" \"{encodingFilePath}\" \"{trainedPath}\" \"{name}\"");
            MessageBox.Show("Đã huấn luyện xong", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
