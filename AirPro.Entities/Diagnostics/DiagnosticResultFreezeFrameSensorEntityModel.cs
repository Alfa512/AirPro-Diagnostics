using System.ComponentModel.DataAnnotations;

namespace AirPro.Entities.Diagnostics
{
    public class DiagnosticResultFreezeFrameSensorEntityModel
    {
        [MaxLength(100)]
        public string SensorName { get; set; }
        [MaxLength(20)]
        public string SensorUnit { get; set; }
        [MaxLength(100)]
        public string SensorValue { get; set; }
    }
}
