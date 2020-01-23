namespace AirPro.Service.DTOs.Interface
{
    public interface IDiagnosticFreezeFrameSensorDto
    {
        string SensorName { get; set; }
        string SensorUnit { get; set; }
        string SensorValue { get; set; }
    }
}