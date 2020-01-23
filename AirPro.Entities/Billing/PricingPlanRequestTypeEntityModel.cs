using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AirPro.Entities.Repair;
using AirPro.Entities.Scan;

namespace AirPro.Entities.Billing
{
    [Table("PricingPlanRequestTypes", Schema = "Billing")]
    public class PricingPlanRequestTypeEntityModel
    {
        [Key]
        public int PricingPlanRequestTypeId { get; set; }

        public int PricingPlanId { get; set; }
        [ForeignKey(nameof(PricingPlanId))]
        public virtual PricingPlanEntityModel PricingPlan { get; set; }

        public int RequestTypeId { get; set; }
        [ForeignKey(nameof(RequestTypeId))]
        public virtual RequestTypeEntityModel RequestType { get; set; }

        public int VehicleMakeTypeId { get; set; }
        [ForeignKey(nameof(VehicleMakeTypeId))]
        public virtual VehicleMakeTypeEntityModel VehicleMakeType { get; set; }

        [Required]
        public decimal LineItemCost { get; set; }
    }
}