using AirPro.Service.DTOs.Interface;
using System;

namespace AirPro.Service.DTOs.Concrete
{
    internal class TechReportDto : ITechReportDto
    {
        public int RequestId { get; set; }
        public int ReportId { get; set; }
        public int RepairId { get; set; }
        public Guid ShopGuid { get; set; }
        public string ShopName { get; set; }
        public string VehicleVIN { get; set; }
        public string VehicleMakeName { get; set; }
        public string VehicleModelName { get; set; }
        public string VehicleTransmission { get; set; }
        public string VehicleYear { get; set; }
        public Guid? ResponsibleTechnicianUserGuid { get; set; }
    }
}