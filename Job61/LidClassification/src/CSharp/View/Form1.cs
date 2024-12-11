using LidClassification.src.CSharp.Service;
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
                Bitmap grayBitmap = HistogramHelper.ToGray(originalBitmap);

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
                    Bitmap bmHistogram = HistogramHelper.Histogram(bm, grayHistogram);

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
                (Bitmap bmRedHistogram, Bitmap bmGreenHistogram, Bitmap bmBlueHistogram) = HistogramHelper.HistogramColor(bmColor, hRed, hGreen, hBlue);

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

    }
}
