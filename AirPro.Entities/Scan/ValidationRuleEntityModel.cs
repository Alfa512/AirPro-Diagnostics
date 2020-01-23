using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirPro.Entities.Scan
{
    [Table("ValidationRules", Schema = "Scan")]
    public class ValidationRuleEntityModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ValidationRuleId { get; set; }

        [Required, MaxLength(100)]
        public string ValidationRuleText { get; set; }

        [MaxLength(1000)]
        public string ValidationRuleDetails { get; set; }

        public int ValidationRuleSortOrder { get; set; }

        public bool ValidationRuleActiveInd { get; set; }
    }
}
