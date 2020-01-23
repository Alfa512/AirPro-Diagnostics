using System.Collections.Generic;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Parser.Output.Concrete
{
    internal class DiagnosticFreezeFrameSensorGroupDto : IDiagnosticFreezeFrameSensorGroupDto
    {
        public ICollection<IDiagnosticFreezeFrameSensorDto> FreezeFrameSensors { get; set; }
    }
}
