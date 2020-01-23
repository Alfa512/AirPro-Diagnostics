using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirPro.Entities.Repair
{
    [Table("VehicleMakeTypes", Schema = "Repair")]
    public class VehicleMakeTypeEntityModel
    {
        [Key]
        public int VehicleMakeTypeId { get; set; }
        public string VehicleMakeTypeName { get; set; }
    }
}
