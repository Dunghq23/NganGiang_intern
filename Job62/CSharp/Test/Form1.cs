using System;
using System.Drawing;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;

using AForge.Video.DirectShow;
using AForge.Video;

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
                if (!capture.IsOpened)
                {
                    ShowError("Không thể mở camera.");
                    return;
                }

                string imagePath = @"D:\\Documents\\Work\\NganGiang\\HAQUANGDUNG\\Job62\\Python\\frames_output\\frame_00000.jpg";
                //CvInvoke.Imwrite(imagePath, frame);

                (float dpiX, float dpiY) = GetImageDpi(imagePath);



                //// Thay đổi kích thước pixel của ảnh
                //string resizedPath = "resized_image.jpg";
                //ResizeImage(imagePath, resizedPath, 800, 600);
                //ShowInfo("Ảnh đã được resize thành kích thước: 800x600.");

                //// Thay đổi kích thước pixel và đặt lại DPI
                //string resizedWithDpiPath = "resized_with_dpi_image.jpg";
                //ResizeImageWithDpi(imagePath, resizedWithDpiPath, 800, 600, 300, 300);
                //ShowInfo("Ảnh đã được resize và đặt DPI thành: 300x300.");

                //// Thay đổi kích thước ảnh dựa trên kích thước vật lý
                //string physicalSizePath = "physical_size_image.jpg";
                //ResizeImageForPhysicalSize(imagePath, physicalSizePath, 4.0f, 3.0f, 300, 300);
                //ShowInfo("Ảnh đã được resize thành kích thước vật lý: 4x3 inch với DPI: 300x300.");

                // Lấy danh sách thiết bị video (camera)


                try
                {
                    var cameraUtil = new CameraDPIUtil();

                    // Liệt kê camera
                    var cameras = cameraUtil.GetAvailableCameras();
                    foreach (var camera in cameras)
                    {
                        //ShowInfo($"Tìm thấy camera: {camera}");
                    }

                    // Lấy DPI của camera
                    var dpi = cameraUtil.GetCameraDPI(0); // 0 là camera đầu tiên
                    label1.Text = $"Camera DPI - Ngang: {dpi.HorizontalDPI}, Dọc: {dpi.VerticalDPI}";



                    // Lấy độ phân giải hiện tại
                    double width = capture.Get(Emgu.CV.CvEnum.CapProp.FrameWidth);
                    double height = capture.Get(Emgu.CV.CvEnum.CapProp.FrameHeight);

                    label2.Text = $"Độ phân giải hiện tại: {width} x {height}";
                    //// Lấy DPI từ file ảnh
                    //var imageDpi = CameraDPIUtil.GetImageDPI("path/to/image.jpg");
                    //Console.WriteLine($"Ảnh DPI - Ngang: {imageDpi.HorizontalDPI}, Dọc: {imageDpi.VerticalDPI}");
                }
                catch (Exception ex)
                {
                    ShowInfo($"Lỗi: {ex.Message}");
                }

            }
        }

        private static (float HorizontalDpi, float VerticalDpi) GetImageDpi(string imagePath)
        {
            using (Bitmap bitmap = new Bitmap(imagePath))
            {
                return (bitmap.HorizontalResolution, bitmap.VerticalResolution);
            }
        }

        private void ShowError(string message)
        {
            MessageBox.Show(message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void ShowInfo(string message)
        {
            MessageBox.Show(message, "Thông tin", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Thay đổi kích thước pixel (resize)
        public static void ResizeImage(string inputPath, string outputPath, int newWidth, int newHeight)
        {
            using (Bitmap originalBitmap = new Bitmap(inputPath))
            {
                using (Bitmap resizedBitmap = new Bitmap(originalBitmap, new Size(newWidth, newHeight)))
                {
                    resizedBitmap.Save(outputPath, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
            }
        }

        // Đặt lại DPI sau khi thay đổi kích thước
        public static void ResizeImageWithDpi(string inputPath, string outputPath, int newWidth, int newHeight, float dpiX, float dpiY)
        {
            using (Bitmap originalBitmap = new Bitmap(inputPath))
            {
                using (Bitmap resizedBitmap = new Bitmap(originalBitmap, new Size(newWidth, newHeight)))
                {
                    resizedBitmap.SetResolution(dpiX, dpiY);
                    resizedBitmap.Save(outputPath, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
            }
        }

        // Thay đổi kích thước in vật lý
        public static void ResizeImageForPhysicalSize(string inputPath, string outputPath, float physicalWidthInInches, float physicalHeightInInches, float dpiX, float dpiY)
        {
            int pixelWidth = (int)(physicalWidthInInches * dpiX);
            int pixelHeight = (int)(physicalHeightInInches * dpiY);

            using (Bitmap originalBitmap = new Bitmap(inputPath))
            {
                using (Bitmap resizedBitmap = new Bitmap(originalBitmap, new Size(pixelWidth, pixelHeight)))
                {
                    resizedBitmap.SetResolution(dpiX, dpiY);
                    resizedBitmap.Save(outputPath, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
            }
        }

    }
}
