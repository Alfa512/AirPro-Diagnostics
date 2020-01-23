using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AirPro.Entities.Repair;

namespace AirPro.Entities.Scan
{
    [Table("DecisionVehicleMakes", Schema = "Scan")]
    public class DecisionVehicleMakeEntityModel : AuditBaseEntityModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DecisionVehicleMakeId { get; set; }

        public int DecisionId { get; set; }
        [ForeignKey(nameof(DecisionId))]
        public virtual DecisionEntityModel Decision { get; set; }

        public int VehicleMakeId { get; set; }
        [ForeignKey(nameof(VehicleMakeId))]
        public virtual VehicleMakeEntityModel VehicleMake { get; set; }

        public bool PreSelectedInd { get; set; }
    }
}