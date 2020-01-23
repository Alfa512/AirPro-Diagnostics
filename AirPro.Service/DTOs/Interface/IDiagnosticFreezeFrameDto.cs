using System.Collections.Generic;

namespace AirPro.Service.DTOs.Interface
{
    public interface IDiagnosticFreezeFrameDto
    {
        string FreezeFrameDiagnosticTroubleCode { get; set; }
        ICollection<IDiagnosticFreezeFrameSensorGroupDto> FreezeFrameSensorGroups { get; set; }
    }
}