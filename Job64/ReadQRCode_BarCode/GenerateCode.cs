//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Windows.Forms;
//using BarcodeLib;
//using QRCoder;

//namespace ReadQRCode_BarCode
//{
//    public partial class Form1 : Form
//    {
//        BarcodeLib.Barcode code128;
//        public Form1()
//        {
//            InitializeComponent();
//            code128 = new Barcode();
//        }

//        private void btn_Show_Click(object sender, EventArgs e)
//        {
//            Image barcode = code128.Encode(BarcodeLib.TYPE.CODE128, txb_BarCode.Text);
//            pb_BarCode.Image = barcode;

//            QRCodeGenerator qrGenerator = new QRCodeGenerator();
//            QRCode qrCode = new QRCode(qrGenerator.CreateQrCode(txb_QRCode.Text, QRCodeGenerator.ECCLevel.Q));
//            //QRCodeGenerator.ECCLevel.Q là mức chịu lỗi 25%; .L là 7%; .M là 15% và .H là trên 25%
//            pb_QRCode.Image = qrCode.GetGraphic(2, Color.Black, Color.White, false);
//            qrGenerator.Dispose();
//            qrCode.Dispose();
//        }
//    }
//}

using BarcodeStandard;
using SkiaSharp;

namespace ReadQRCode_BarCode
{
    public partial class GenerateCode : Form
    {
        private Barcode code128;

        public GenerateCode()
        {
            InitializeComponent();
            code128 = new Barcode();
        }

        private void btn_Show_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txb_BarCode.Text))
            {
                MessageBox.Show("Please enter text for the barcode.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                // Generate Barcode
                // if (!string.IsNullOrWhiteSpace(txb_BarCode.Text))
                // {
                // Tạo SKImage
                SKImage skImage = code128.Encode(BarcodeStandard.Type.Code128, txb_BarCode.Text);

                // Chuyển đổi từ SKImage sang System.Drawing.Image
                using (SKBitmap skBitmap = SKBitmap.FromImage(skImage))
                using (var ms = new MemoryStream())
                {
                    SKPixmap pixmap = skBitmap.PeekPixels();
                    pixmap.Encode(SKEncodedImageFormat.Png, 100).SaveTo(ms);

                    pb_BarCode.Image = new Bitmap(ms);
                    pb_BarCode.SizeMode = PictureBoxSizeMode.StretchImage;

                    // Lưu ảnh vào thư mục debug của project
                    string projectPath = AppDomain.CurrentDomain.BaseDirectory;
                    // Tạo một FileStream để lưu ảnh
                    // Tạo thư mục Debug nếu chưa tồn tại để tránh lỗi không tìm thấy đường dẫn
                    string imagePath = Path.Combine(projectPath, "barcode.png");
                    using (FileStream fs = new FileStream(imagePath, FileMode.Create))
                    {
                        // Nối đường dẫn với tên file
                        pixmap.Encode(SKEncodedImageFormat.Png, 100).SaveTo(fs);
                    }
                    MessageBox.Show($"Barcode image saved to: {imagePath}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                // }
                // else
                // {
                //     MessageBox.Show("Please enter text for the barcode.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                // }

                //// Generate QR Code
                //if (!string.IsNullOrWhiteSpace(txb_QRCode.Text))
                //{
                //    QRCodeGenerator qrGenerator = new QRCodeGenerator();
                //    QRCodeData qrCodeData = qrGenerator.CreateQrCode(txb_QRCode.Text, QRCodeGenerator.ECCLevel.Q);
                //    QRCode qrCode = new QRCode(qrCodeData);

                //    pb_QRCode.Image = qrCode.GetGraphic(2, Color.Black, Color.White, false);
                //    pb_QRCode.SizeMode = PictureBoxSizeMode.StretchImage;
                //    qrCode.Dispose();
                //    qrGenerator.Dispose();
                //}
                //else
                //{
                //    MessageBox.Show("Please enter text for the QR code.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
    }
}

