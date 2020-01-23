using System;
using System.Collections.Generic;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Service.DTOs.Concrete
{
    internal class ReportTroubleCodeDto : IReportTroubleCodeDto
    {
        public long ReportOrderTroubleCodeId { get; set; }
        public int ControllerId { get; set; }
        public string ControllerName { get; set; }
        public int ControllerIdOrig { get; set; }
        public string ControllerNameOrig { get; set; }
        public int TroubleCodeId { get; set; }
        public int TroubleCodeIdOrig { get; set; }
        public string TroubleCode { get; set; }
        public string TroubleCodeOrig { get; set; }
        public string TroubleCodeDescription { get; set; }
        public string TroubleCodeDescriptionOrig { get; set; }
        public Guid? OverrideCreatedByUserGuid { get; set; }
        public string OverrideCreatedByUserDisplay { get; set; }
        public DateTime? OverrideCreatedDt { get; set; }
        public Guid? OverrideUpdatedByUserGuid { get; set; }
        public string OverrideUpdatedByUserDisplay { get; set; }
        public DateTime? OverrideUpdatedDt { get; set; }

        public IEnumerable<IReportTroubleCodeRecommendationDto> Recommendations { get; set; }
    }
}