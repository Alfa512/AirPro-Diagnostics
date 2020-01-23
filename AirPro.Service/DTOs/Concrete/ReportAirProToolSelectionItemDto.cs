using AirPro.Service.DTOs.Interface;

namespace AirPro.Service.DTOs.Concrete
{
    internal class ReportAirProToolSelectionItemDto : IReportAirProToolSelectionItemDto
    {
        public int AirProToolId { get; set; }
        public string AirProToolName { get; set; }
        public string AirProToolPassword { get; set; }
    }
}