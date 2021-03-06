﻿using AirPro.Service.DTOs.Interface;

namespace AirPro.Parser.Output.Concrete
{
    internal class DiagnosticFreezeFrameSensorDto : IDiagnosticFreezeFrameSensorDto
    {
        public string SensorName { get; set; }
        public string SensorUnit { get; set; }
        public string SensorValue { get; set; }
    }
}
