using System;

namespace AirPro.Service.DTOs.Interface
{
    public interface IReportDiagnosticResultSelectionItemDto
    {
        int ResultId { get; set; }
        string VehicleVIN { get; set; }
        string VehicleMake { get; set; }
        string VehicleModel { get; set; }
        string VehicleYear { get; set; }
        int ControllerCount { get; set; }
        int TroubleCodeCount { get; set; }
        DateTime ScanPerformedDt { get; set; }
        DateTime ScanUploadedDt { get; set; }
        bool VinMatchInd { get; set; }
        bool AssignedToRequestInd { get; set; }
        bool SelectedForReportInd { get; set; }
    }
}