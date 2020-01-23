using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AirPro.Entities.Diagnostics;

namespace AirPro.Entities.Scan
{
    [Table("ReportDiagnosticResults", Schema = "Scan")]
    public class ReportDiagnosticResultEntityModel
    {
        [Key, Column(Order = 1)]
        public int ReportId { get; set; }
        [ForeignKey(nameof(ReportId))]
        public virtual ReportEntityModel Report { get; set; }

        [Key, Column(Order = 2)]
        public int DiagnosticResultId { get; set; }
        [ForeignKey(nameof(DiagnosticResultId))]
        public virtual DiagnosticResultEntityModel DiagnosticResult { get; set; }
    }
}
