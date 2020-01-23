using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AirPro.Entities.Access;
using AirPro.Entities.Repair;

namespace AirPro.Entities.Billing
{
    [Table("PaymentTransactions", Schema = "Billing")]
    public class PaymentTransactionEntityModel : AuditBaseEntityModel
    {
        [Key]
        public int PaymentTransactionId { get; set; }

        [Required]
        public int PaymentId { get; set; }
        [ForeignKey(nameof(PaymentId))]
        public virtual PaymentEntityModel Payment { get; set; }

        // Payment applied?
        [Required]
        public int InvoiceId { get; set; }
        [ForeignKey(nameof(InvoiceId))]
        public virtual InvoiceEntityModel Invoice { get; set; }

        public decimal InvoiceAmountApplied { get; set; }

        public decimal DiscountAmountApplied { get; set; }

        public bool PaymentTransactionVoidInd { get; set; } = false;

        public Guid? PaymentTransactionVoidByUserGuid { get; set; }
        [ForeignKey(nameof(PaymentTransactionVoidByUserGuid))]
        public virtual UserEntityModel PaymentTransactionVoidByUser { get; set; }
        public DateTimeOffset? PaymentTransactionVoidDt { get; set; } = null;
    }
}
