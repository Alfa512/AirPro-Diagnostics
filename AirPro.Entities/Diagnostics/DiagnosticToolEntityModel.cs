using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirPro.Entities.Diagnostics
{
    [Table("Tools", Schema = "Diagnostic")]
    public class DiagnosticToolEntityModel
    {
        [Key]
        public int DiagnosticToolId { get; set; }
        [MaxLength(50)]
        public string DiagnosticToolName { get; set; }
    }
}
