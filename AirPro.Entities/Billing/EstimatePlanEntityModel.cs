using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirPro.Entities.Billing
{
    [Table("EstimatePlans", Schema = "Billing")]
    public class EstimatePlanEntityModel : AuditBaseEntityModel
    {
        [Key]
        public int EstimatePlanId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool ActiveInd { get; set; }

        [ForeignKey(nameof(EstimatePlanVehicleEntityModel.EstimatePlanId))]
        public virtual ICollection<EstimatePlanVehicleEntityModel> EstimatePlanVehicles { get; set; }
    }
}