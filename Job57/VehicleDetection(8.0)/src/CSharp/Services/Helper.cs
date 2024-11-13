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
    }
}
