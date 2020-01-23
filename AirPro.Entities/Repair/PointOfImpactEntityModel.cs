using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirPro.Entities.Repair
{
    [Table("PointOfImpacts", Schema = "Repair")]
    public class PointOfImpactEntityModel
    {
        [Key, Column(Order = 0)]
        public int PointOfImpactId { get; set; }

        [Required, MaxLength(128)]
        public string Name { get; set; }
    }
}