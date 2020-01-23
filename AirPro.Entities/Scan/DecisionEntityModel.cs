using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AirPro.Common.Enumerations;

namespace AirPro.Entities.Scan
{
    [Table("Decisions", Schema = "Scan")]
    public class DecisionEntityModel : AuditBaseEntityModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DecisionId { get; set; }
        public string DecisionText { get; set; }
        public bool ActiveInd { get; set; }

        public ReportTextSeverity DefaultTextSeverity { get; set; }

    }
}