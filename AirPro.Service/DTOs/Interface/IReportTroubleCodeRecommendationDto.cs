using System;
using AirPro.Common.Enumerations;

namespace AirPro.Service.DTOs.Interface
{
    public interface IReportTroubleCodeRecommendationDto
    {
        int RequestId { get; set; }
        int RequestTypeId { get; set; }
        int RequestCategoryId { get; set; }
        bool CurrentRequestInd { get; set; }
        int RequestVersion { get; set; }
        bool RequestCanceleInd { get; set; }
        string RequestCancelNotes { get; set; }
        int? ReportId { get; set; }
        int? DiagnosticResultId { get; set; }
        long? ResultTroubleCodeId { get; set; }
        string TroubleCodeInformation { get; set; }
        bool InformCustomerInd { get; set; }
        bool? AccidentRelatedInd { get; set; }
        bool ExcludeFromReportInd { get; set; }
        bool CodeClearedInd { get; set; }
        string TroubleCodeNoteText { get; set; }
        int? TroubleCodeRecommendationId { get; set; }
        string TroubleCodeRecommendationText { get; set; }
        Guid? RecommendationCreatedByUserGuid { get; set; }
        string RecommendationCreatedByUserDisplay { get; set; }
        DateTime? RecommendationCreatedDt { get; set; }
        Guid? RecommendationUpdatedByUserGuid { get; set; }
        string RecommendationUpdatedByUserDisplay { get; set; }
        DateTime? RecommendationUpdatedDt { get; set; }
        ReportTextSeverity RecommendationTextSeverity { get; set; }
    }
}