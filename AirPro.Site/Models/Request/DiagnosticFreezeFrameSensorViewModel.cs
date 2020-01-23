using AirPro.Service.DTOs.Interface;

namespace AirPro.Site.Models.Request
{
    public class DiagnosticFreezeFrameSensorViewModel : IDiagnosticFreezeFrameSensorDto
    {
        public string SensorName { get; set; }
        public string SensorUnit { get; set; }
        public string SensorValue { get; set; }
    }
}