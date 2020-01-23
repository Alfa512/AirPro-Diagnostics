using System;
using System.Collections.Generic;

namespace AirPro.Service.DTOs.Interface
{
    public interface IReportTroubleCodeDto
    {
        long ReportOrderTroubleCodeId { get; set; }
        int ControllerId { get; set; }
        string ControllerName { get; set; }
        int ControllerIdOrig { get; set; }
        string ControllerNameOrig { get; set; }
        int TroubleCodeId { get; set; }
        int TroubleCodeIdOrig { get; set; }
        string TroubleCode { get; set; }
        string TroubleCodeOrig { get; set; }
        string TroubleCodeDescription { get; set; }
        string TroubleCodeDescriptionOrig { get; set; }
        Guid? OverrideCreatedByUserGuid { get; set; }
        string OverrideCreatedByUserDisplay { get; set; }
        DateTime? OverrideCreatedDt { get; set; }
        Guid? OverrideUpdatedByUserGuid { get; set; }
        string OverrideUpdatedByUserDisplay { get; set; }
        DateTime? OverrideUpdatedDt { get; set; }

        IEnumerable<IReportTroubleCodeRecommendationDto> Recommendations { get; set; }
    }
}