using System.Collections.Generic;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Site.Models.Request
{
    public class DiagnosticFreezeFrameSensorGroupViewModel : IDiagnosticFreezeFrameSensorGroupDto
    {
        public ICollection<IDiagnosticFreezeFrameSensorDto> FreezeFrameSensors { get; set; }
    }
}