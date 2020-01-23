using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AirPro.Entities.Access;

namespace AirPro.Entities.Scan
{
    [Table("ReportWorkTypes", Schema = "Scan")]
    public class ReportWorkTypeEntityModel
    {
        [Column(Order = 0), Key, Required, ForeignKey(nameof(Report))]
        public int ReportId { get; set; }
        public virtual ReportEntityModel Report { get; set; }

        [Column(Order = 1), Key, Required, ForeignKey(nameof(WorkType))]
        public int WorkTypeId { get; set; }
        public virtual WorkTypeEntityModel WorkType { get; set; }

        public bool InvoicedInd { get; set; } = false;
        public Guid? InvoicedByUserGuid { get; set; }
        [ForeignKey(nameof(InvoicedByUserGuid))]
        public virtual UserEntityModel InvoicedByUser { get; set; } = null;
        public decimal? InvoiceAmount { get; set; }
    }
}