using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AirPro.Entities.Access;
using AirPro.Entities.Repair;

namespace AirPro.Entities.Technician
{
    [Table("ProfileVehicleMakes", Schema = "Technician")]
    public class ProfileVehicleMakeEntityModel
    {
        [Column(Order = 0), Key, Required, ForeignKey(nameof(User))]
        public Guid UserGuid { get; set; }
        public virtual UserEntityModel User { get; set; }

        [Column(Order = 1), Key, Required, ForeignKey(nameof(VehicleMake))]
        public int VehicleMakeId { get; set; }
        public virtual VehicleMakeEntityModel VehicleMake { get; set; }
    }
}
