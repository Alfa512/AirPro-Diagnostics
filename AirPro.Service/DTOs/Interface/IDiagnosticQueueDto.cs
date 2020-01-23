using System;

namespace AirPro.Service.DTOs.Interface
{
    public interface IDiagnosticQueueDto
    {
        int ResultId { get; set; }
        string VehicleVin { get; set; }
        string VehicleMake { get; set; }
        string VehicleModel { get; set; }
        string VehicleYear { get; set; }
        DateTime ScanDateTime { get; set; }
    }
}
