using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AirPro.Entities.Access;

namespace AirPro.Entities.Billing
{
    [Table("Payments", Schema = "Billing")]
    public class PaymentEntityModel : AuditBaseEntityModel
    {
        [Key]
        public int PaymentId { get; set; }

        [Required]
        public Guid PaymentReceivedFromShopGuid { get; set; }
        [ForeignKey(nameof(PaymentReceivedFromShopGuid))]
        public virtual ShopEntityModel PaymentReceivedFromShop { get; set; }

        [Required]
        public int PaymentTypeId { get; set; }
        [ForeignKey(nameof(PaymentTypeId))]
        public virtual PaymentTypeEntityModel PaymentType { get; set; }

        public decimal PaymentAmount { get; set; }

        public int DiscountPercentage { get; set; }

        public string PaymentReferenceNumber { get; set; }

        public string PaymentMemo { get; set; }

        public bool PaymentVoidInd { get; set; } = false;
        public Guid? PaymentVoidByUserGuid { get; set; }
        [ForeignKey(nameof(PaymentVoidByUserGuid))]
        public virtual UserEntityModel PaymentVoidByUser { get; set; }
        public DateTimeOffset? PaymentVoidDt { get; set; } = null;
        [Column(TypeName = "Date")]
        public DateTime? PaymentDt { get; set; }

        [ForeignKey(nameof(PaymentId))]
        public virtual ICollection<PaymentTransactionEntityModel> PaymentTransactions { get; set; }

        public int? CurrencyId { get; set; }

        [ForeignKey(nameof(CurrencyId))]
        public virtual CurrencyEntityModel Currency { get; set; }
    }
}
