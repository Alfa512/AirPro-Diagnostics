using System.Collections.Generic;

namespace AirPro.Entities.Diagnostics
{
    public class DiagnosticResultFreezeFrameSensorGroupEntityModel
    {
        public ICollection<DiagnosticResultFreezeFrameSensorEntityModel> FreezeFrameSensors { get; set; }
    }
}
