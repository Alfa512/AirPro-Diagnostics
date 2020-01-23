using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirPro.Entities.Scan
{
    [Table("ReportValidationRules", Schema = "Scan")]
    public class ReportValidationRuleEntityModel
    {
        [Key, Column(Order = 1)]
        public int ReportId { get; set; }
        [ForeignKey(nameof(ReportId))]
        public virtual ReportEntityModel Report { get; set; }

        [Key, Column(Order = 2)]
        public int ValidationRuleId { get; set; }
        [ForeignKey(nameof(ValidationRuleId))]
        public virtual ValidationRuleEntityModel ValidationRule { get; set; }

        public bool ValidationRuleResultInd { get; set; }

        public bool? ResultAcknowledgedInd { get; set; }

        public Guid? ResultAcknowledgedByUserGuid { get; set; }
    }
}