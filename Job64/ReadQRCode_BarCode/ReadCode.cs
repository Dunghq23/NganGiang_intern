using IronBarCode;

namespace ReadQRCode_BarCode
{
    public partial class ReadCode : Form
    {
        //private string selectedQRCodeImagePath;
        private string selectedBarCodeImagePath;

        public ReadCode()
        {
            InitializeComponent();
        }

        private void btn_ChooseQRCode_Click(object sender, EventArgs e)
        {
            //using (OpenFileDialog openFileDialog = new OpenFileDialog())
            //{
            //    openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
            //    openFileDialog.Title = "Chọn hình ảnh chứa mã";

            //    if (openFileDialog.ShowDialog() == DialogResult.OK)
            //    {
            //        selectedQRCodeImagePath = openFileDialog.FileName;
            //        pb_QRCode.Image = new Bitmap(selectedQRCodeImagePath);
            //        pb_QRCode.SizeMode = PictureBoxSizeMode.StretchImage;
            //    }
            //}
        }

        private void btn_ChooseBarCode_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                openFileDialog.Title = "Chọn hình ảnh chứa mã";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    selectedBarCodeImagePath = openFileDialog.FileName;
                    pb_BarCode.Image = new Bitmap(selectedBarCodeImagePath);
                    pb_BarCode.SizeMode = PictureBoxSizeMode.StretchImage;
                }
            }
        }

        private void btn_ReadCode_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedBarCodeImagePath))
            {
                MessageBox.Show("Vui lòng chọn một hình ảnh BarCode trước.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //// Lấy đường dẫn ảnh từ thư mục Debug của project
            //string projectPath = AppDomain.CurrentDomain.BaseDirectory;
            //string imagePath = Path.Combine(projectPath, "barcode.png");

            try
            {
                string decodedText = DecodeImage(selectedBarCodeImagePath);
                if (!string.IsNullOrEmpty(decodedText))
                {
                    MessageBox.Show($"Nội dung mã: {decodedText}", "Kết quả", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Không tìm thấy mã hợp lệ trong hình ảnh.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Có lỗi xảy ra: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string DecodeImage(string imagePath)
        {
            try
            {
                var result = BarcodeReader.Read(imagePath);

                if (result != null && result.Any())
                {
                    var firstResult = result.First();
                    return firstResult.Text;
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi đọc mã: {ex.Message}");
            }
        }
    }
}
