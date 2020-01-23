using System;

namespace AirPro.Service.DTOs.Interface
{
    public interface IReportResponsibilityHistoryListItemDto
    {
        string ResponsibleTech { get; set; }
        DateTime ResponsibleStartDt { get; set; }
        DateTime? ResponsibleEndDt { get; set; }
    }
}