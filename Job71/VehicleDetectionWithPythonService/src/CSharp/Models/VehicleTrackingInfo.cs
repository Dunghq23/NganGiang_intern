using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace VehicleDetection_8._0_.src.CSharp.Models
{
    /// <summary>
    /// VehicleTrackingInfo: Lớp này lưu trữ thông tin theo dõi của mỗi phương tiện, bao gồm vị trí, thời gian và vận tốc.
    /// CalculateSpeed: Hàm này tính toán vận tốc dựa trên khoảng cách và thời gian giữa hai vị trí.
    /// CalculateSpeed trong ProcessImage: Hàm này được gọi để tính toán vận tốc cho mỗi phương tiện được phát hiện.
    /// </summary>
    public class VehicleTrackingInfo
    {
        public string Type { get; set; }
        public List<PointF> Positions { get; set; }
        public List<double> Timestamps { get; set; }
        public List<double> Velocities { get; set; }
    }
}
