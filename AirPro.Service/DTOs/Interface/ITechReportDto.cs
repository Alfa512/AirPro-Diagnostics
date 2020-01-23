using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirPro.Service.DTOs.Interface
{
    public interface ITechReportDto
    {
        int RequestId { get; set; }
        int ReportId { get; set; }
        int RepairId { get; set; }
        Guid ShopGuid { get; set; }
        string ShopName { get; set; }
        string VehicleVIN { get; set; }
        string VehicleMakeName { get; set; }
        string VehicleModelName { get; set; }
        string VehicleTransmission { get; set; }
        string VehicleYear { get; set; }
        Guid? ResponsibleTechnicianUserGuid { get; set; }
    }
}
