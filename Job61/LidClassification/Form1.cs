using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LidClassification
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // Chọn ảnh và hiển thị vào PictureBox
        private void btnChooseImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp",
                Title = "Select an Image File"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                pbOriginal.Image = Image.FromFile(filePath);
                pbOriginal.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }

        // Xử lý sự kiện chuyển ảnh sang xám và tính tổ chức đồ
        private void btnToGray_Click(object sender, EventArgs e)
        {
            if (pbOriginal.Image != null)
            {
                // Chuyển ảnh gốc sang ảnh xám
                Bitmap originalBitmap = new Bitmap(pbOriginal.Image);
                Bitmap grayBitmap = ToGray(originalBitmap);

                // Hiển thị ảnh xám lên PictureBox
                pbGray.Image = grayBitmap;
                pbGray.SizeMode = PictureBoxSizeMode.Zoom;

                // Chắc chắn rằng đã có ảnh trong PictureBox
                if (pbGray.Image != null)
                {
                    // Chuyển ảnh thành Bitmap
                    Bitmap bm = new Bitmap(pbGray.Image);

                    // Khởi tạo mảng tần suất cho tổ chức đồ xám
                    int[] grayHistogram = new int[256];

                    // Tính toán tổ chức đồ xám
                    Bitmap bmHistogram = Histogram(bm, grayHistogram);

                    // Hiển thị tổ chức đồ vào PictureBox
                    pbHistogramGray.Image = bmHistogram;
                    pbHistogramGray.SizeMode = PictureBoxSizeMode.Zoom;
                }

                // Tính toán và hiển thị tổ chức đồ màu
                Bitmap bmColor = new Bitmap(pbOriginal.Image);

                // Tạo các mảng tổ chức đồ cho từng kênh màu
                int[] hRed = new int[256];
                int[] hGreen = new int[256];
                int[] hBlue = new int[256];

                // Tính toán tổ chức đồ màu
                (Bitmap bmRedHistogram, Bitmap bmGreenHistogram, Bitmap bmBlueHistogram) = HistogramColor(bmColor, hRed, hGreen, hBlue);

                // Hiển thị tổ chức đồ màu vào PictureBox
                pbHistogramRed.Image = bmRedHistogram;
                pbHistogramRed.SizeMode = PictureBoxSizeMode.Zoom;
                pbHistogramGreen.Image = bmGreenHistogram;
                pbHistogramGreen.SizeMode = PictureBoxSizeMode.Zoom;
                pbHistogramBlue.Image = bmBlueHistogram;
                pbHistogramBlue.SizeMode = PictureBoxSizeMode.Zoom;
            }
            else
            {
                MessageBox.Show("Please select an image first.");
            }
        }

        // Chuyển ảnh sang ảnh xám
        private Bitmap ToGray(Bitmap bm)
        {
            Bitmap grayBitmap = new Bitmap(bm);

            // Khóa các pixel của ảnh để tăng hiệu suất
            Rectangle rect = new Rectangle(0, 0, bm.Width, bm.Height);
            System.Drawing.Imaging.BitmapData data = grayBitmap.LockBits(rect,
                System.Drawing.Imaging.ImageLockMode.ReadWrite, bm.PixelFormat);

            // Tạo mảng byte để xử lý dữ liệu ảnh
            int bytesPerPixel = Image.GetPixelFormatSize(bm.PixelFormat) / 8;
            int byteCount = data.Stride * bm.Height;
            byte[] pixels = new byte[byteCount];
            System.Runtime.InteropServices.Marshal.Copy(data.Scan0, pixels, 0, byteCount);

            // Duyệt qua tất cả các pixel và chuyển sang ảnh xám
            for (int y = 0; y < bm.Height; y++)
            {
                for (int x = 0; x < bm.Width; x++)
                {
                    int pixelIndex = (y * data.Stride) + (x * bytesPerPixel);
                    if (x == 414 && y == 211)
                    {
                        MessageBox.Show($"RGB: {pixels[pixelIndex].ToString()} {pixels[pixelIndex + 1].ToString()} {pixels[pixelIndex + 2].ToString()}");
                    }
                    byte blue = pixels[pixelIndex];
                    byte green = pixels[pixelIndex + 1];
                    byte red = pixels[pixelIndex + 2];

                    // Tính giá trị xám từ RGB
                    byte grayValue = (byte)(red * 0.299 + green * 0.587 + blue * 0.114);

                    // Gán giá trị xám cho tất cả các kênh màu
                    pixels[pixelIndex] = grayValue;
                    pixels[pixelIndex + 1] = grayValue;
                    pixels[pixelIndex + 2] = grayValue;
                }
            }

            // Cập nhật lại dữ liệu ảnh sau khi xử lý
            System.Runtime.InteropServices.Marshal.Copy(pixels, 0, data.Scan0, byteCount);
            grayBitmap.UnlockBits(data);

            return grayBitmap;
        }

        // Tính tổ chức đồ
        private Bitmap Histogram(Bitmap bm, int[] h, Color color = default)
        {
            color = color == default ? Color.Black : color;

            // Tính tổ chức đồ H[x]
            Array.Clear(h, 0, h.Length);

            for (int y = 0; y < bm.Height; y++)
            {
                for (int x = 0; x < bm.Width; x++)
                {
                    Color pixelColor = bm.GetPixel(x, y);
                    byte gray = pixelColor.R; // Vì ảnh đã là xám nên chỉ cần lấy một kênh
                    h[gray] += 1;
                }
            }

            // Tìm giá trị lớn nhất trong tổ chức đồ để chuẩn hóa
            int max = h.Max();

            // Vẽ tổ chức đồ
            int Height = 100;
            Bitmap bmHistogram = new Bitmap(256, Height);
            using (Graphics gp = Graphics.FromImage(bmHistogram))
            {
                gp.Clear(Color.White);

                for (int i = 0; i < 256; i++)
                {
                    int n = (h[i] * Height) / max;
                    gp.DrawLine(new Pen(color), i, Height, i, Height - n);
                }
            }

            return bmHistogram;
        }

        // Tính tổ chức đồ cho từng kênh màu
        private (Bitmap red, Bitmap green, Bitmap blue) HistogramColor(Bitmap bm, int[] redHist, int[] greenHist, int[] blueHist)
        {
            //Array.Clear(redHist, 0, redHist.Length);
            //Array.Clear(greenHist, 0, greenHist.Length);
            //Array.Clear(blueHist, 0, blueHist.Length);

            for (int i = 0; i < 256; i++)
            {
                redHist[i] = 0;
                greenHist[i] = 0;
                blueHist[i] = 0;
            }

            for (int y = 0; y < bm.Height; y++)
            {
                for (int x = 0; x < bm.Width; x++)
                {
                    Color pixelColor = bm.GetPixel(x, y);
                    redHist[pixelColor.R] += 1;
                    greenHist[pixelColor.G] += 1;
                    blueHist[pixelColor.B] += 1;
                }
            }

            Bitmap bmRedHistogram = Histogram(bm, redHist, Color.Red);
            Bitmap bmGreenHistogram = Histogram(bm, greenHist, Color.Green);
            Bitmap bmBlueHistogram = Histogram(bm, blueHist, Color.Blue);

            return (bmRedHistogram, bmGreenHistogram, bmBlueHistogram);
        }
    }
}
