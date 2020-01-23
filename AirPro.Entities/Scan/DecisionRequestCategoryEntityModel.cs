using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirPro.Entities.Scan
{
    [Table("DecisionRequestCategories", Schema = "Scan")]
    public class DecisionRequestCategoryEntityModel : AuditBaseEntityModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DecisionRequestCategoryId { get; set; }

        public int DecisionId { get; set; }
        [ForeignKey(nameof(DecisionId))]
        public virtual DecisionEntityModel Decision { get; set; }

        public int RequestCategoryId { get; set; }
        [ForeignKey(nameof(RequestCategoryId))]
        public virtual RequestCategoryEntityModel RequestCategory { get; set; }

        public bool PreSelectedInd { get; set; }
    }
}