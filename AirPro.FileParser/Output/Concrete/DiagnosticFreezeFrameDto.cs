using System.Collections.Generic;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Parser.Output.Concrete
{
    internal class DiagnosticFreezeFrameDto : IDiagnosticFreezeFrameDto
    {
        public string FreezeFrameDiagnosticTroubleCode { get; set; }
        public ICollection<IDiagnosticFreezeFrameSensorGroupDto> FreezeFrameSensorGroups { get; set; }
    }
}
