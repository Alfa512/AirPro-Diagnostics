﻿using System;
using System.Collections.Generic;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Service.DTOs.Concrete
{
    internal class ReportDto : IReportDto
    {
        public int RequestId { get; set; }
        public int RequestTypeId { get; set; }
        public int? RequestCategoryId { get; set; }
        public int? AirProToolId { get; set; }
        public string AirProToolName { get; set; }
        public string AirProToolPassword { get; set; }
        public string ReportFooterHTML { get; set; }
        public string ReportHeaderHTML { get; set; }
        public string TechnicianNotes { get; set; }
        public bool CompleteReport { get; set; }
        public bool CancelReport { get; set; }
        public string CancelNotes { get; set; }
        public int? CancelReasonTypeId { get; set; }
        public Guid? ResponsibleTechUserId { get; set; }
        public string ResponsibleTech { get; set; }
        public DateTime? ResponsibleDt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDt { get; set; }
        public bool AllowEdit { get; set; }
        public bool RepairComplete { get; set; }
        public byte[] ReportVersion { get; set; }
        public string VehicleInstructions { get; set; }
        public bool OEMCertifiedInd { get; set; }
        public IEnumerable<IReportRequestTypeSelectionItemDto> RequestTypeSelections { get; set; }
        public IEnumerable<IReportAirProToolSelectionItemDto> AirProToolSelections { get; set; }
        public IEnumerable<IReportResponsibilityHistoryListItemDto> ResponsibilityHistory { get; set; }
        public IEnumerable<IReportInternalNoteHistoryListItemDto> InternalNoteHistory { get; set; }
        public IEnumerable<IReportWorkTypeSelectionItemDto> WorkTypeSelections { get; set; }
        public IEnumerable<IReportDecisionSelectionItemDto> DecisionSelections { get; set; }
        public IEnumerable<IReportDiagnosticResultSelectionItemDto> DiagnosticResultSelections { get; set; }
        public IEnumerable<IReportTroubleCodeDto> TroubleCodeRecommendations { get; set; }
        public IEnumerable<IReportFrequentRecommendationSelectionItemDto> FrequentRecommendationSelections { get; set; }
        public IEnumerable<IReportPossibleMissingControllerDto> PossibleMissingControllers { get; set; }
        public IEnumerable<ICancelReasonTypeDto> CancelReasonTypes { get; set; }
        public IEnumerable<IReportVehicleMakeToolDto> VehicleMakeTools { get; set; }

        public IEnumerable<IReportValidationRuleDto> ValidationRules { get; set; }

        public IUpdateResultDto UpdateResult { get; set; }
    }
}
