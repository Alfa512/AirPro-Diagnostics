using System;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Site.Models.Request
{
    public class ReportDiagnosticResultSelectionItemViewModel : IReportDiagnosticResultSelectionItemDto
    {
        public int ResultId { get; set; }
        public string VehicleVIN { get; set; }
        public string VehicleMake { get; set; }
        public string VehicleModel { get; set; }
        public string VehicleYear { get; set; }
        public int ControllerCount { get; set; }
        public int TroubleCodeCount { get; set; }
        public DateTime ScanPerformedDt { get; set; }
        public DateTime ScanUploadedDt { get; set; }
        public bool VinMatchInd { get; set; }
        public bool AssignedToRequestInd { get; set; }
        public bool SelectedForReportInd { get; set; }
    }
}