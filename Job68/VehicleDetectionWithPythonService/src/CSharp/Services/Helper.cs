using System.Windows.Forms;
using VehicleDetection_8._0_.src.CSharp.Models;

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
        //public static void WriteLog(string filePath, string message)
        //{
        //    try
        //    {
        //        // Kiểm tra và tạo file nếu chưa tồn tại
        //        if (!File.Exists(filePath))
        //        {
        //            using (var fileStream = File.Create(filePath))
        //            {
        //                // Đảm bảo file được tạo trước khi đóng
        //            }
        //        }

        //        // Ghi log với timestamp
        //        using (StreamWriter writer = new StreamWriter(filePath, true))
        //        {
        //            writer.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] {message}");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Lỗi khi ghi log: {ex.Message}");
        //    }
        //}

        public static Bitmap DrawBoundingBoxes(string filePath, dynamic boundingBoxes)
        {
            using (Image img = Image.FromFile(filePath))
            {
                Bitmap bitmap = new Bitmap(img);
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    Color customColor = ColorTranslator.FromHtml("#33FF66");
                    using (Pen pen = new Pen(customColor, 2)) // Độ dày là 2
                    {
                        foreach (var box in boundingBoxes)
                        {
                            int x = box.x, y = box.y, w = box.w, h = box.h;
                            g.DrawRectangle(pen, x, y, w, h);
                            g.DrawString(box.label.ToString(), new Font("Arial", 12), new SolidBrush(ColorTranslator.FromHtml("#33FF66")), x, y - 24);
                        }
                    }
                }
                return bitmap;
            }
        }

    }
}
