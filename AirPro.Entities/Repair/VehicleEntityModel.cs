using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirPro.Entities.Repair
{
    [Table("Vehicles", Schema = "Repair")]
    public class VehicleEntityModel
    {
        [Key, Display(Name = "VIN")]
        public string VehicleVIN { get; set; }
        [Required, MaxLength(100)]
        public string Make { get; set; }
        [Required, MaxLength(300)]
        public string Model { get; set; }
        [Required]
        public string Year { get; set; }
        [Required, MaxLength(200)]
        public string Transmission { get; set; } = "Unknown";

        public bool LookupFound => VehicleLookup != null;

        [ForeignKey(nameof(VehicleLookup))]
        public int? VehicleLookupId { get; set; }
        public virtual VehicleLookupEntityModel VehicleLookup { get; set; }

        [Required, ForeignKey(nameof(VehicleMake))]
        public int VehicleMakeId { get; set; }
        public virtual VehicleMakeEntityModel VehicleMake { get; set; }
    }
}
