using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirPro.Entities.Scan
{
    [Table("DecisionRequestTypes", Schema = "Scan")]
    public class DecisionRequestTypeEntityModel : AuditBaseEntityModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DecisionRequestTypeId { get; set; }

        public int DecisionId { get; set; }
        [ForeignKey(nameof(DecisionId))]
        public virtual DecisionEntityModel Decision { get; set; }

        public int RequestTypeId { get; set; }
        [ForeignKey(nameof(RequestTypeId))]
        public virtual RequestTypeEntityModel RequestType { get; set; }

        public bool PreSelectedInd { get; set; }
    }
}