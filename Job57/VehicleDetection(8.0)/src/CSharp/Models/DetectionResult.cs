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
        public Dictionary<string, int> VehicleCounts { get; set; } // Số lượng các loại phương tiện
    }
}
