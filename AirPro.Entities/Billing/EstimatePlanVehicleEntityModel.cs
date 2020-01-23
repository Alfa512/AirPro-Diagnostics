using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AirPro.Entities.Repair;

namespace AirPro.Entities.Billing
{
    [Table("EstimatePlanVehicles", Schema = "Billing")]
    public class EstimatePlanVehicleEntityModel
    {
        [Column(Order = 0), Key, Required, ForeignKey(nameof(EstimatePlan))]
        public int EstimatePlanId { get; set; }

        public virtual EstimatePlanEntityModel EstimatePlan { get; set; }

        [Column(Order = 1), Key, Required, ForeignKey(nameof(VehicleMake))]
        public int VehicleMakeId { get; set; }

        public virtual VehicleMakeEntityModel VehicleMake { get; set; }
        public decimal CompletionCost { get; set; }
    }
}