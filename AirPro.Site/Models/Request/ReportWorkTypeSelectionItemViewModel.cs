using AirPro.Service.DTOs.Interface;

namespace AirPro.Site.Models.Request
{
    public class ReportWorkTypeSelectionItemViewModel : IReportWorkTypeSelectionItemDto
    {
        public string WorkTypeGroupName { get; set; }
        public int WorkTypeId { get; set; }
        public string WorkTypeName { get; set; }
        public bool WorkTypeSelected { get; set; }
    }
}