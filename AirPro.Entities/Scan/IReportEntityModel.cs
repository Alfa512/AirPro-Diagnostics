using System;

namespace AirPro.Entities.Scan
{
    public interface IReportEntityModel
    {
        int ReportId { get; set; }
        string TechnicianNotes { get; set; }
        string ReportNotes { get; set; }
        string ReportFooterHTML { get; set; }
        bool CompletedInd { get; set; }
        bool CanceledInd { get; set; }
        string CancellationNotes { get; set; }
        DateTimeOffset? CompletedDt { get; set; }
        Guid? CompletedByUserGuid { get; set; }
        bool InvoicedInd { get; set; }
        Guid? InvoicedByUserGuid { get; set; }
        DateTimeOffset? InvoicedDt { get; set; }
        decimal? InvoiceAmount { get; set; }
        Guid? ResponsibleTechnicianUserGuid { get; set; }
        DateTimeOffset? ResponsibleSetDt { get; set; }
    }
}