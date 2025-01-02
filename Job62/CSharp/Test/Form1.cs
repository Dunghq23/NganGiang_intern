using System;
using System.Drawing;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Mở camera mặc định (0 là camera mặc định, có thể thay đổi nếu có nhiều camera)
            using (VideoCapture capture = new VideoCapture(0))
            {
                // Kiểm tra xem camera có mở thành công không
                if (!capture.IsOpened)
                {
                    MessageBox.Show("Không thể mở camera.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //// Đặt độ phân giải mới cho khung hình (ví dụ: 640x480)
                //capture.Set(CapProp.FrameWidth, 1280);  // Thay đổi chiều rộng
                //capture.Set(CapProp.FrameHeight, 960); // Thay đổi chiều cao

                //// Kiểm tra xem độ phân giải đã được thay đổi hay chưa
                //double width = capture.Get(CapProp.FrameWidth);
                //double height = capture.Get(CapProp.FrameHeight);
                //MessageBox.Show($"Kích thước khung hình đã được thay đổi thành: {width}x{height}",
                //                 "Thông tin độ phân giải",
                //                 MessageBoxButtons.OK,
                //                 MessageBoxIcon.Information);

                // Chụp một khung hình từ camera
                Mat frame = new Mat();
                capture.Read(frame);

                // Kiểm tra nếu có ảnh
                if (frame.IsEmpty)
                {
                    MessageBox.Show("Không thể chụp ảnh từ camera.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Lưu ảnh vào file
                string imagePath = "captured_image2.jpg";
                CvInvoke.Imwrite(imagePath, frame);

                // Mở ảnh từ file và lấy DPI
                using (Bitmap bitmap = new Bitmap(imagePath))
                {
                    float dpiX = bitmap.HorizontalResolution;  // DPI theo chiều ngang
                    float dpiY = bitmap.VerticalResolution;    // DPI theo chiều dọc

                    // Hiển thị thông tin camera và DPI của ảnh
                    MessageBox.Show($"Camera Resolution: {frame.Width}x{frame.Height}\n" +
                                    $"DPI (Horizontal): {dpiX}\nDPI (Vertical): {dpiY}",
                                    "Thông tin ảnh và camera",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}
