using System.Collections.Generic;
using AirPro.Service.DTOs.Interface;
using Newtonsoft.Json;

namespace AirPro.Parser.Output.Concrete
{
    internal class DiagnosticControllerDto : IDiagnosticControllerDto
    {
        public int? ControllerId { get; set; }

        [JsonProperty("CONTROLLER_NAME")]
        public string ControllerName { get; set; }
        public ICollection<IDiagnosticTroubleCodeDto> TroubleCodes { get; set; }
        public ICollection<IDiagnosticFreezeFrameDto> FreezeFrames { get; set; }
    }
}
