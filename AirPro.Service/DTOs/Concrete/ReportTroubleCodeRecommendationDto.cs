using System;
using AirPro.Common.Enumerations;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Service.DTOs.Concrete
{
    internal class ReportTroubleCodeRecommendationDto : IReportTroubleCodeRecommendationDto
    {
        public int RequestId { get; set; }
        public int RequestTypeId { get; set; }
        public int RequestCategoryId { get; set; }
        public bool CurrentRequestInd { get; set; }
        public int RequestVersion { get; set; }
        public bool RequestCanceleInd { get; set; }
        public string RequestCancelNotes { get; set; }
        public int? ReportId { get; set; }
        public int? DiagnosticResultId { get; set; }
        public long? ResultTroubleCodeId { get; set; }
        public string TroubleCodeInformation { get; set; }
        public bool InformCustomerInd { get; set; }
        public bool? AccidentRelatedInd { get; set; }
        public bool ExcludeFromReportInd { get; set; }
        public bool CodeClearedInd { get; set; }
        public int? TroubleCodeRecommendationId { get; set; }
        public string TroubleCodeNoteText { get; set; }
        public string TroubleCodeRecommendationText { get; set; }
        public Guid? RecommendationCreatedByUserGuid { get; set; }
        public string RecommendationCreatedByUserDisplay { get; set; }
        public DateTime? RecommendationCreatedDt { get; set; }
        public Guid? RecommendationUpdatedByUserGuid { get; set; }
        public string RecommendationUpdatedByUserDisplay { get; set; }
        public DateTime? RecommendationUpdatedDt { get; set; }
        public ReportTextSeverity RecommendationTextSeverity { get; set; }
    }
}