using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirPro.Entities.Scan
{
    [Table("RequestTypeValidationRules", Schema = "Scan")]
    public class RequestTypeValidationRuleEntityModel
    {
        [Key, Column(Order = 1)]
        public int RequestTypeId { get; set; }
        [ForeignKey(nameof(RequestTypeId))]
        public virtual RequestTypeEntityModel RequestType { get; set; }

        [Key, Column(Order = 2)]
        public int ValidationRuleId { get; set; }
        [ForeignKey(nameof(ValidationRuleId))]
        public virtual ValidationRuleEntityModel ValidationRule { get; set; }
    }
}