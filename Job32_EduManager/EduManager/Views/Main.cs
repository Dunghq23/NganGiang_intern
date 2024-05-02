using System;
using System.Drawing;
using System.Windows.Forms;

using EduManager.Views;

namespace EduManager
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void btnEduManage_Click(object sender, EventArgs e)
        {
            EduProgram_Form f = new EduProgram_Form();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            // Điều chỉnh kích thước hình ảnh để vừa với nút
            Size buttonSize = btnEduManage.Size;
            Image resizedIcon = ResizeImage(Properties.Resources.QLDT, new Size(buttonSize.Width, buttonSize.Height / 2)); // Chia đôi chiều cao

            // Gán hình ảnh đã được điều chỉnh vào nút
            btnEduManage.Image = resizedIcon;

            // Chỉnh văn bản hiển thị phía dưới hình ảnh
            btnEduManage.TextImageRelation = TextImageRelation.ImageAboveText;
        }

        private Image ResizeImage(Image img, Size newSize)
        {
            Bitmap resizedImage = new Bitmap(newSize.Width, newSize.Height);
            using (Graphics g = Graphics.FromImage(resizedImage))
            {
                g.DrawImage(img, 0, 0, newSize.Width, newSize.Height);
            }
            return resizedImage;
        }
    }
}
