using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AirPro.Entities.Repair;

namespace AirPro.Entities.Access
{
    [Table("ShopVehicleMakes", Schema = "Access")]
    public class ShopVehicleMakeEntityModel
    {
        [Column(Order = 0), Key, Required, ForeignKey(nameof(Shop))]
        public Guid ShopId { get; set; }

        [Column(Order = 1), Key, Required, ForeignKey(nameof(VehicleMake))]
        public int VehicleMakeId { get; set; }

        public virtual ShopEntityModel Shop { get; set; }
        public virtual VehicleMakeEntityModel VehicleMake { get; set; }
    }
}