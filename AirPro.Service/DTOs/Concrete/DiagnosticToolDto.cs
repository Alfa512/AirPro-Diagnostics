using AirPro.Service.DTOs.Interface;

namespace AirPro.Service.DTOs.Concrete
{
    public class DiagnosticToolDto : IDiagnosticToolDto
    {
        public int DiagnosticToolId { get; set; }
        public string DiagnosticToolName { get; set; }
    }
}
