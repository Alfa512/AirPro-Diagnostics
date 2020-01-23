using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirPro.Entities.Scan
{
    [Table("WorkTypes", Schema = "Scan")]
    public class WorkTypeEntityModel : AuditBaseEntityModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WorkTypeId { get; set; }

        public string WorkTypeName { get; set; }

        public int? WorkTypeSortOrder { get; set; }

        [ForeignKey(nameof(WorkTypeGroup))]
        public int WorkTypeGroupId { get; set; }
        public virtual WorkTypeGroupEntityModel WorkTypeGroup { get; set; }

        [ForeignKey(nameof(WorkTypeRequestTypeEntityModel.WorkTypeId))]
        public virtual ICollection<WorkTypeRequestTypeEntityModel> WorkTypeRequestTypes { get; set; }

        [ForeignKey(nameof(WorkTypeVehicleMakeEntityModel.WorkTypeId))]
        public virtual ICollection<WorkTypeVehicleMakeEntityModel> WorkTypeVehicleMakes { get; set; }

        public bool WorkTypeActiveInd { get; set; }

        public string WorkTypeDescription { get; set; }
    }
}
