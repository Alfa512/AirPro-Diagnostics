using System;
using System.Collections.Generic;
using AirPro.Library.Models.Concrete;

namespace AirPro.Library.Models.Interface
{
    public interface IScanReportLibraryModel
    {
        int? AirProToolId { get; set; }
        string AirProToolName { get; set; }
        string AirProToolPassword { get; set; }
        bool AllowEdit { get; set; }
        string CancelNotes { get; set; }
        bool CancelReport { get; set; }
        bool CompleteReport { get; set; }
        string CreatedBy { get; set; }
        DateTime CreatedDt { get; set; }
        string ReportNotes { get; set; }
        byte[] ReportVersion { get; set; }
        int RequestId { get; set; }
        int? ReportTypeCategoryId { get; set; }
        int ReportTypeId { get; set; }
        IEnumerable<int> RequestWorkTypeIds { get; set; }
        DateTime? ResponsibleDt { get; set; }
        string ResponsibleTech { get; set; }
        Guid ResponsibleTechUserId { get; set; }
        Guid ShopGuid { get; set; }
        string TechnicianNotes { get; set; }
        string UpdatedBy { get; set; }
        DateTime? UpdatedDt { get; set; }
        IEnumerable<ScanReportLineItemViewModel> PreviousScanReports { get; set; }
    }
}