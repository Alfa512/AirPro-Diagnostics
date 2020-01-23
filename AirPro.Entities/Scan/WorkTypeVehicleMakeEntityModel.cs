using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AirPro.Entities.Repair;

namespace AirPro.Entities.Scan
{
    [Table("WorkTypeVehicleMakes", Schema = "Scan")]
    public class WorkTypeVehicleMakeEntityModel
    {
        [Column(Order = 0), Key, Required, ForeignKey(nameof(WorkType))]
        public int WorkTypeId { get; set; }
        public virtual WorkTypeEntityModel WorkType { get; set; }

        [Column(Order = 1), Key, Required, ForeignKey(nameof(VehicleMake))]
        public int VehicleMakeId { get; set; }
        public virtual VehicleMakeEntityModel VehicleMake { get; set; }
    }
}
