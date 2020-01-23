using System.Collections.Generic;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Site.Models.Request
{
    public class DiagnosticFreezeFrameViewModel : IDiagnosticFreezeFrameDto
    {
        public string FreezeFrameDiagnosticTroubleCode { get; set; }
        public ICollection<IDiagnosticFreezeFrameSensorGroupDto> FreezeFrameSensorGroups { get; set; }
    }
}