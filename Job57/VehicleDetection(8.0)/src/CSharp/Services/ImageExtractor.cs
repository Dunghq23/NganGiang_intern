using Emgu.CV;
using Emgu.CV.CvEnum;

namespace VehicleDetection.src.CSharp.Services
{
    public class ImageExtractor
    {
        private readonly string _outputImagePath;

        // Constructor nhận đường dẫn để lưu các ảnh trích xuất được
        public ImageExtractor(string outputImagePath)
        {
            _outputImagePath = outputImagePath;
        }

        // Phương thức trích xuất ảnh từ video
        public void ExtractImages(string videoPath, int frameSkip = 1)
        {
            // Kiểm tra video có tồn tại không
            if (!File.Exists(videoPath))
            {
                Console.WriteLine("Video file not found!");
                return;
            }

            // Mở video
            using (var capture = new VideoCapture(videoPath))
            {
                // Lấy tổng số frame trong video
                int totalFrames = (int)capture.Get(CapProp.FrameCount);
                int frameCount = 0;

                // Đảm bảo thư mục lưu ảnh đã tồn tại
                if (!Directory.Exists(_outputImagePath))
                {
                    Directory.CreateDirectory(_outputImagePath);
                }

                // Lặp qua từng frame của video
                while (frameCount < totalFrames)
                {
                    // Đọc một frame từ video
                    Mat frame = new Mat();
                    capture.Read(frame);

                    // Kiểm tra nếu không có frame nào để đọc (end of video)
                    if (frame.IsEmpty)
                    {
                        break;
                    }

                    // Nếu frameCount chia hết cho frameSkip thì lưu frame
                    if (frameCount % frameSkip == 0)
                    {
                        SaveFrame(frame, frameCount);
                    }

                    frameCount++;
                }

                Console.WriteLine("Complete!");
            }
        }

        // Phương thức lưu frame dưới dạng ảnh
        private void SaveFrame(Mat frame, int frameCount)
        {
            // Tạo tên tệp cho ảnh
            string fileName = Path.Combine(_outputImagePath, $"frame_{frameCount:D6}.jpg");

            // Lưu frame thành ảnh
            frame.Save(fileName);
            Console.WriteLine($"Frame {frameCount} saved to {fileName}");
        }
    }
}