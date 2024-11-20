using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VehicleDetection.src.CSharp.Models
{
    public class DetectionResult
    {
        [JsonPropertyName("total_time")]
        public double TotalTime { get; set; } // Thời gian xử lý tổng

        [JsonPropertyName("total_vehicles")]
        public int TotalVehicles { get; set; } // Tổng số phương tiện

        [JsonPropertyName("vehicle_counts")]
        public Dictionary<string, int> VehicleCounts { get; set; } = new Dictionary<string, int>();
        public Bitmap image { get; set; }

        public double PreprocessTime { get; set; }
        public double DetectTime { get; set; }
        public double DrawBoxTime { get; set; }
    }
}
