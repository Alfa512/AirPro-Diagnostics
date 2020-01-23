using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirPro.Entities.Billing
{
    [Table("PricingPlans", Schema = "Billing")]
    public class PricingPlanEntityModel : AuditBaseEntityModel
    {
        [Key]
        public int PricingPlanId { get; set; }
        public string PricingPlanName { get; set; }
        public string PricingPlanDescription { get; set; }
        public bool PricingPlanActiveInd { get; set; } = true;
        public int? CurrencyId { get; set; }

        [ForeignKey(nameof(PricingPlanId))]
        public virtual ICollection<PricingPlanRequestTypeEntityModel> RequestTypes { get; set; }

        [ForeignKey(nameof(PricingPlanId))]
        public virtual ICollection<PricingPlanWorkTypeEntityModel> WorkTypes { get; set; }

        [ForeignKey(nameof(CurrencyId))]
        public virtual CurrencyEntityModel Currency { get; set; }
    }
}
