using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirPro.Entities.Scan
{
    [Table("WorkTypeGroups", Schema = "Scan")]
    public class WorkTypeGroupEntityModel : AuditBaseEntityModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WorkTypeGroupId { get; set; }

        public string WorkTypeGroupName { get; set; }

        public int? WorkTypeGroupSortOrder { get; set; }

        public bool WorkTypeGroupActiveInd { get; set; }
    }
}