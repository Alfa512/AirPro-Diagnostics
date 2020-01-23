using System.Collections.Generic;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Site.Models.Request
{
    public class DiagnosticControllerViewModel : IDiagnosticControllerDto
    {
        public int? ControllerId { get; set; }
        public string ControllerName { get; set; }
        public ICollection<IDiagnosticTroubleCodeDto> TroubleCodes { get; set; }
        public ICollection<IDiagnosticFreezeFrameDto> FreezeFrames { get; set; }
    }
}