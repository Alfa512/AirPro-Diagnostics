using System.Collections.Generic;
using System.Linq;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Service.DTOs.Concrete
{
    internal class DiagnosticFreezeFrameSensorGroupDto : IDiagnosticFreezeFrameSensorGroupDto
    {
        public DiagnosticFreezeFrameSensorGroupDto(ICollection<DiagnosticFreezeFrameSensorDto> freezeFrameSensors)
        {
            FreezeFrameSensors = freezeFrameSensors.ToList<IDiagnosticFreezeFrameSensorDto>();
        }

        public ICollection<IDiagnosticFreezeFrameSensorDto> FreezeFrameSensors { get; set; }
    }
}