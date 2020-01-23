using System;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Service.DTOs.Concrete
{
    internal class ReportResponsibilityHistoryListItemDto : IReportResponsibilityHistoryListItemDto
    {
        public string ResponsibleTech { get; set; }
        public DateTime ResponsibleStartDt { get; set; }
        public DateTime? ResponsibleEndDt { get; set; }
    }
}