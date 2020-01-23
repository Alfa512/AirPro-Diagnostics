using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirPro.Entities.Billing
{
    [Table("PaymentTypes", Schema = "Billing")]
    public class PaymentTypeEntityModel
    {
        [Key]
        public int PaymentTypeId { get; set; }

        public string PaymentTypeName { get; set; }

        public int PaymentTypeSortOrder { get; set; }

        public bool PaymentTypeActiveInd { get; set; }
    }
}
