using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirPro.Entities.Repair
{
    [Table("VehicleMakes", Schema = "Repair")]
    public class VehicleMakeEntityModel
    {
        [Key]
        public int VehicleMakeId { get; set; }
        [MaxLength(100)]
        public string VehicleMakeName { get; set; }
        public string ProgramName { get; set; }
        public string ProgramInstructions { get; set; }

        [ForeignKey(nameof(VehicleMakeType))]
        public int VehicleMakeTypeId { get; set; }
        public virtual VehicleMakeTypeEntityModel VehicleMakeType { get; set; }

        public virtual ICollection<VehicleEntityModel> Vehicles { get; set; }
        public virtual ICollection<VehicleMakeToolEntityModel> ProgrammTools { get; set; }
    }
}