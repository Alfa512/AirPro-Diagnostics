using System.Collections.Generic;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Service.DTOs.Concrete
{
    internal class DiagnosticControllerDto : IDiagnosticControllerDto
    {
        public int? ControllerId { get; set; }
        public string ControllerName { get; set; }
        public ICollection<IDiagnosticTroubleCodeDto> TroubleCodes { get; set; }
        public ICollection<IDiagnosticFreezeFrameDto> FreezeFrames { get; set; }
    }
}