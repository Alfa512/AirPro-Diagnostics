using System;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Service.DTOs.Concrete
{
    internal class DiagnosticQueueDto : IDiagnosticQueueDto
    {
        public int ResultId { get; set; }
        public string VehicleVin { get; set; }
        public string VehicleMake { get; set; }
        public string VehicleModel { get; set; }
        public string VehicleYear { get; set; }
        public DateTime ScanDateTime { get; set; }
    }
}