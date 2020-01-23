using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AirPro.Entities.Interfaces;

namespace AirPro.Entities.Scan
{
    [Table("ReportsArchive", Schema = "Scan")]
    public class ReportArchiveEntityModel : IArchiveEntityModel, IReportEntityModel, IAuditBaseEntityModel
    {
        [Key]
        public int ArchiveId { get; set; }
        public DateTimeOffset ArchiveDt { get; set; }
        public int ReportId { get; set; }
        public string TechnicianNotes { get; set; }
        public string ReportNotes { get; set; }
        public string ReportFooterHTML { get; set; }
        public bool CompletedInd { get; set; }
        public bool CanceledInd { get; set; }
        public string CancellationNotes { get; set; }
        public DateTimeOffset? CompletedDt { get; set; }
        public Guid? CompletedByUserGuid { get; set; }
        public bool InvoicedInd { get; set; }
        public Guid? InvoicedByUserGuid { get; set; }
        public DateTimeOffset? InvoicedDt { get; set; }
        public decimal? InvoiceAmount { get; set; }
        public Guid? ResponsibleTechnicianUserGuid { get; set; }
        public DateTimeOffset? ResponsibleSetDt { get; set; }
        public int? DiagnosticResultId { get; set; }
        public Guid CreatedByUserGuid { get; set; }
        public DateTimeOffset CreatedDt { get; set; }
        public Guid? UpdatedByUserGuid { get; set; }
        public DateTimeOffset? UpdatedDt { get; set; }
    }
}