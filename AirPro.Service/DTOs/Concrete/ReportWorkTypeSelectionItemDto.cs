using AirPro.Service.DTOs.Interface;

namespace AirPro.Service.DTOs.Concrete
{
    internal class ReportWorkTypeSelectionItemDto : IReportWorkTypeSelectionItemDto
    {
        public string WorkTypeGroupName { get; set; }
        public int WorkTypeId { get; set; }
        public string WorkTypeName { get; set; }
        public bool WorkTypeSelected { get; set; }
    }
}