using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AirPro.Entities.Access;
using AirPro.Entities.Diagnostics;

namespace AirPro.Entities.Scan
{
    [Table("Reports", Schema = "Scan")]
    public class ReportEntityModel : AuditBaseEntityModel, IReportEntityModel
    {
        [Key]
        public int ReportId { get; set; }
        public string TechnicianNotes { get; set; }
        public string ReportNotes { get; set; }
        public string ReportFooterHTML { get; set; }

        public bool CompletedInd { get; set; } = false;
        public bool CanceledInd { get; set; } = false;
        public string CancellationNotes { get; set; }
        public int? CancelReasonTypeId { get; set; }
        [ForeignKey(nameof(CancelReasonTypeId))]
        public virtual CancelReasonTypeEntityModel CancelReasonType { get; set; }
        public DateTimeOffset? CompletedDt { get; set; }
        public Guid? CompletedByUserGuid { get; set; }
        [ForeignKey(nameof(CompletedByUserGuid))]
        public virtual UserEntityModel CompletedByUser { get; set; }

        public bool InvoicedInd { get; set; } = false;
        public Guid? InvoicedByUserGuid { get; set; }
        [ForeignKey(nameof(InvoicedByUserGuid))]
        public virtual UserEntityModel InvoicedByUser { get; set; } = null;
        public DateTimeOffset? InvoicedDt { get; set; } = null;
        public decimal? InvoiceAmount { get; set; }

        public Guid? ResponsibleTechnicianUserGuid { get; set; }
        [ForeignKey(nameof(ResponsibleTechnicianUserGuid))]
        public virtual UserEntityModel ResponsibleTechnician { get; set; } = null;
        public DateTimeOffset? ResponsibleSetDt { get; set; }

        public bool CodesClearedInd { get; set; } = false;
        public int DangerCodeCount { get; set; }
        public int WarningCodeCount { get; set; }
        public int CautionCodeCount { get; set; }
        public int OtherCodeCount { get; set; }

        public virtual ICollection<ReportWorkTypeEntityModel> ReportWork { get; set; }

        public virtual ICollection<ReportDiagnosticResultEntityModel> DiagnosticResults { get; set; }

        [Timestamp]
        public byte[] ReportVersion { get; private set; }
    }
}