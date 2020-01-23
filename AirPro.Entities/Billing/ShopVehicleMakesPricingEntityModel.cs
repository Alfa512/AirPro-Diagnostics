using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirPro.Entities.Access;
using AirPro.Entities.Repair;

namespace AirPro.Entities.Billing
{
    [Table("ShopVehicleMakesPricing", Schema = "Billing")]
    public class ShopVehicleMakesPricingEntityModel
    {
        [Column(Order = 0), Key, Required, ForeignKey(nameof(Shop))]
        public Guid ShopId { get; set; }
        [Column(Order = 1), Key, Required, ForeignKey(nameof(VehicleMake))]
        public int VehicleMakeId { get; set; }
        [ForeignKey(nameof(PricingPlan))]
        public int PricingPlanId { get; set; }

        public virtual ShopEntityModel Shop { get; set; }
        public virtual VehicleMakeEntityModel VehicleMake { get; set; }
        public virtual PricingPlanEntityModel PricingPlan { get; set; }
    }
}
