using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AirPro.Common.Enumerations;

namespace AirPro.Entities.Scan
{
    [Table("ReportDecisions", Schema = "Scan")]
    public class ReportDecisionEntityModel : AuditBaseEntityModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReportDecisionId { get; set; }

        public int ReportId { get; set; }
        [ForeignKey(nameof(ReportId))]
        public virtual ReportEntityModel Report { get; set; }

        public int DecisionId { get; set; }
        [ForeignKey(nameof(DecisionId))]
        public virtual DecisionEntityModel Decision { get; set; }

        public ReportTextSeverity TextSeverity { get; set; }
    }
}