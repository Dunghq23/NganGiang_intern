using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using AForge.Video.DirectShow;
using AForge.Video;

public class CameraDPIUtil
{
    private FilterInfoCollection videoDevices;
    private VideoCaptureDevice videoSource;

    public class DPIInfo
    {
        public double HorizontalDPI { get; set; }
        public double VerticalDPI { get; set; }
    }

    public CameraDPIUtil()
    {
        // Lấy danh sách camera có sẵn
        videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
    }

    // Lấy DPI của camera đang hoạt động
    public DPIInfo GetCameraDPI(int deviceIndex = 0)
    {
        if (videoDevices.Count == 0)
            throw new Exception("Không tìm thấy camera nào.");

        if (deviceIndex >= videoDevices.Count)
            throw new Exception("Chỉ số camera không hợp lệ.");

        DPIInfo dpiInfo = new DPIInfo();

        try
        {
            videoSource = new VideoCaptureDevice(videoDevices[deviceIndex].MonikerString);
            videoSource.NewFrame += (s, e) =>
            {
                using (Bitmap bitmap = (Bitmap)e.Frame.Clone())
                {
                    dpiInfo.HorizontalDPI = bitmap.HorizontalResolution;
                    dpiInfo.VerticalDPI = bitmap.VerticalResolution;
                }
                videoSource.SignalToStop();
            };

            videoSource.Start();
            // Đợi frame đầu tiên
            System.Threading.Thread.Sleep(1000);
            videoSource.Stop();
        }
        catch (Exception ex)
        {
            throw new Exception($"Lỗi khi lấy DPI: {ex.Message}");
        }
        finally
        {
            if (videoSource != null && videoSource.IsRunning)
                videoSource.Stop();
        }

        return dpiInfo;
    }

    // Lấy DPI từ file ảnh
    public static DPIInfo GetImageDPI(string imagePath)
    {
        try
        {
            using (var image = Image.FromFile(imagePath))
            {
                return new DPIInfo
                {
                    HorizontalDPI = image.HorizontalResolution,
                    VerticalDPI = image.VerticalResolution
                };
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Lỗi khi đọc DPI từ ảnh: {ex.Message}");
        }
    }

    // Liệt kê các camera có sẵn
    public List<string> GetAvailableCameras()
    {
        List<string> cameraList = new List<string>();
        foreach (FilterInfo device in videoDevices)
        {
            cameraList.Add(device.Name);
        }
        return cameraList;
    }
}