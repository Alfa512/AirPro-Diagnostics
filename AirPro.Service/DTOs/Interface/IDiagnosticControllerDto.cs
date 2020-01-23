using System.Collections.Generic;

namespace AirPro.Service.DTOs.Interface
{
    public interface IDiagnosticControllerDto
    {
        int? ControllerId { get; set; }
        string ControllerName { get; set; }
        ICollection<IDiagnosticTroubleCodeDto> TroubleCodes { get; set; }
        ICollection<IDiagnosticFreezeFrameDto> FreezeFrames { get; set; }
    }
}