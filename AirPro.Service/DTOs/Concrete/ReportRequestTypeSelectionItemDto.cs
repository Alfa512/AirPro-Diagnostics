using AirPro.Service.DTOs.Interface;

namespace AirPro.Service.DTOs.Concrete
{
    internal class ReportRequestTypeSelectionItemDto : IReportRequestTypeSelectionItemDto
    {
        public int RequestCategoryId { get; set; }
        public string RequestCategoryName { get; set; }
        public int RequestTypeId { get; set; }
        public string RequestTypeName { get; set; }
    }
}
