using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirPro.Entities.Diagnostics
{
    [Table("TroubleCodes", Schema = "Diagnostic")]
    public class DiagnosticTroubleCodeEntityModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TroubleCodeId { get; set; }

        [MaxLength(20)]
        public string TroubleCode { get; set; }

        [MaxLength(1000)]
        public string TroubleCodeDescription { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed), Index]
        public int TroubleCodeHash { get; private set; }
    }
}
