using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using AirPro.Entities.Access;
using AirPro.Entities.Billing;

namespace AirPro.Entities.Repair
{
    [Table("Invoices", Schema = "Repair")]
    [MetadataType(typeof(InvoiceEntityMetadata))]
    public class InvoiceEntityModel : AuditBaseEntityModel
    {
        [Key, ForeignKey(nameof(Repair))]
        public int InvoiceId { get; set; }

        public virtual OrderEntityModel Repair { get; set; }

        public string CustomerMemo { get; set; }

        public bool InvoicedInd { get; set; } = false;
        public DateTimeOffset? InvoicedDt { get; set; }
        public int? CurrencyId { get; set; }
        [ForeignKey(nameof(CurrencyId))]
        public virtual CurrencyEntityModel Currency { get; set; }

        public Guid? InvoicedByUserGuid { get; set; }
        [ForeignKey(nameof(InvoicedByUserGuid))]
        public virtual UserEntityModel InvoicedByUser { get; set; }

        public decimal? InvoiceTotal => InvoicedInd ? GetInvoiceTotalByRequests() + GetInvoiceTotalByWorkItems() : null;

        private decimal? GetInvoiceTotalByRequests()
        {
            return (from sr in this.Repair.ScanRequests
                where sr.Report.InvoicedInd == true && sr.Report.InvoiceAmount > 0
                select sr.Report.InvoiceAmount)?.Sum();
        }

        private decimal? GetInvoiceTotalByWorkItems()
        {
            var reportWork = this.Repair.ScanRequests.SelectMany(x => x.Report.ReportWork);
            return (from rw in reportWork
                where rw.InvoicedInd == true && rw.InvoiceAmount > 0
                select rw.InvoiceAmount)?.Sum();
        }
    }
}