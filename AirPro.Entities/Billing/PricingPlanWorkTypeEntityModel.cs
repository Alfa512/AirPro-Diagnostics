using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AirPro.Entities.Repair;
using AirPro.Entities.Scan;

namespace AirPro.Entities.Billing
{
    [Table("PricingPlanWorkTypes", Schema = "Billing")]
    public class PricingPlanWorkTypeEntityModel
    {
        [Key]
        public int PricingPlanWorkTypeId { get; set; }

        public int PricingPlanId { get; set; }
        [ForeignKey(nameof(PricingPlanId))]
        public virtual PricingPlanEntityModel PricingPlan { get; set; }

        public int WorkTypeId { get; set; }
        [ForeignKey(nameof(WorkTypeId))]
        public virtual WorkTypeEntityModel WorkType { get; set; }

        public int VehicleMakeTypeId { get; set; }
        [ForeignKey(nameof(VehicleMakeTypeId))]
        public virtual VehicleMakeTypeEntityModel VehicleMakeType { get; set; }

        [Required]
        public decimal LineItemCost { get; set; }
    }
}