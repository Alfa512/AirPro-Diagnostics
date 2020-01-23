using System.Collections.Generic;

namespace AirPro.Service.DTOs.Interface
{
    public interface IDiagnosticFreezeFrameSensorGroupDto
    {
        ICollection<IDiagnosticFreezeFrameSensorDto> FreezeFrameSensors { get; set; }
    }
}