using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirPro.Entities.Repair
{
    [Table("OrderPointOfImpacts", Schema = "Repair")]
    public class OrderPointOfImpactEntityModel
    {
        [Key, Column(Order = 0)]
        [ForeignKey(nameof(Order))]
        public int OrderID { get; set; }
        public virtual OrderEntityModel Order { get; set; }

        [Key, Column(Order = 1)]
        [ForeignKey(nameof(PointOfImpact))]
        public int PointOfImpactId { get; set; }
        public virtual PointOfImpactEntityModel PointOfImpact { get; set; }
    }
}