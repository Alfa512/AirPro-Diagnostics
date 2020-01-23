using System;
using System.Collections.Generic;

namespace AirPro.Service.DTOs.Interface
{
    public interface IReportDto
    {
        int RequestId { get; set; }
        int RequestTypeId { get; set; }
        int? RequestCategoryId { get; set; }

        int? AirProToolId { get; set; }
        string AirProToolName { get; set; }
        string AirProToolPassword { get; set; }

        string ReportFooterHTML { get; set; }
        string ReportHeaderHTML { get; set; }
        string TechnicianNotes { get; set; }

        bool CompleteReport { get; set; }
        bool CancelReport { get; set; }
        string CancelNotes { get; set; }
        int? CancelReasonTypeId { get; set; }

        Guid? ResponsibleTechUserId { get; set; }
        string ResponsibleTech { get; set; }
        DateTime? ResponsibleDt { get; set; }

        string CreatedBy { get; set; }
        DateTime CreatedDt { get; set; }

        string UpdatedBy { get; set; }
        DateTime? UpdatedDt { get; set; }

        bool AllowEdit { get; set; }
        bool RepairComplete { get; set; }
        byte[] ReportVersion { get; set; }
        string VehicleInstructions { get; set; }
        bool OEMCertifiedInd { get; set; }
        IEnumerable<IReportRequestTypeSelectionItemDto> RequestTypeSelections { get; set; }
        IEnumerable<IReportAirProToolSelectionItemDto> AirProToolSelections { get; set; }
        IEnumerable<IReportResponsibilityHistoryListItemDto> ResponsibilityHistory { get; set; }
        IEnumerable<IReportInternalNoteHistoryListItemDto> InternalNoteHistory { get; set; }

        IEnumerable<IReportWorkTypeSelectionItemDto> WorkTypeSelections { get; set; }
        IEnumerable<IReportDecisionSelectionItemDto> DecisionSelections { get; set; }

        IEnumerable<IReportDiagnosticResultSelectionItemDto> DiagnosticResultSelections { get; set; }

        IEnumerable<IReportTroubleCodeDto> TroubleCodeRecommendations { get; set; }

        IEnumerable<IReportFrequentRecommendationSelectionItemDto> FrequentRecommendationSelections { get; set; }
        IEnumerable<ICancelReasonTypeDto> CancelReasonTypes { get; set; }

        IEnumerable<IReportPossibleMissingControllerDto> PossibleMissingControllers { get; set; }
        IEnumerable<IReportVehicleMakeToolDto> VehicleMakeTools { get; set; }

        IEnumerable<IReportValidationRuleDto> ValidationRules { get; set; }

        IUpdateResultDto UpdateResult { get; set; }
    }
}