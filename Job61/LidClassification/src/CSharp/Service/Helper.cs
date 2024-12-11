using System.Windows.Forms;

namespace VehicleDetection.src.CSharp.Services
{
    public static class Helper
    {
        public static void DeleteImagesInDirectory(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                Console.WriteLine("Thư mục không tồn tại.");
                return;
            }

            var imageExtensions = new[] { ".jpg", ".png", ".jpeg", ".gif" };
            foreach (var file in Directory.GetFiles(directoryPath))
            {
                if (Array.Exists(imageExtensions, ext => file.EndsWith(ext, StringComparison.OrdinalIgnoreCase)))
                {
                    File.Delete(file);
                    Console.WriteLine($"Đã xóa: {file}");
                }
            }
            Console.WriteLine("Hoàn thành việc xóa ảnh.");
        }

        public static Bitmap DrawBoundingBoxes(string filePath, dynamic boundingBoxes)
        {
            using (Image img = Image.FromFile(filePath))
            {
                Bitmap bitmap = new Bitmap(img);
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    // Định nghĩa màu sắc
                    Color defaultColor = ColorTranslator.FromHtml("#0000FF"); // Màu xanh
                    Color lidColor = ColorTranslator.FromHtml("#FF0000");     // Màu đỏ

                    foreach (var box in boundingBoxes)
                    {
                        // Lấy thông tin bounding box
                        int x = box.x, y = box.y, w = box.w, h = box.h;

                        // Chọn màu sắc tùy thuộc vào nhãn
                        Color boxColor = box.label.ToString() == "Lid" ? lidColor : defaultColor;

                        // Vẽ hình chữ nhật và nhãn
                        using (Pen pen = new Pen(boxColor, 2))
                        using (SolidBrush brush = new SolidBrush(boxColor))
                        using (Font font = new Font("Arial", 12))
                        {
                            g.DrawRectangle(pen, x, y, w, h);
                            g.DrawString(box.label.ToString(), font, brush, x, y - 24);
                        }
                    }
                }
                return bitmap;
            }
        }

    }
}
