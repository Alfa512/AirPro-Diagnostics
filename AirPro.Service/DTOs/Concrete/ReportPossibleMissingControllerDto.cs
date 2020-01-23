using AirPro.Service.DTOs.Interface;

namespace AirPro.Service.DTOs.Concrete
{
    internal class ReportPossibleMissingControllerDto : IReportPossibleMissingControllerDto
    {
        public int ControllerId { get; set; }
        public string ControllerName { get; set; }
    }
}