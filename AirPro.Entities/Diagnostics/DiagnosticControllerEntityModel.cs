using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirPro.Entities.Diagnostics
{
    [Table("Controllers", Schema = "Diagnostic")]
    public class DiagnosticControllerEntityModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ControllerId { get; set; }

        [MaxLength(200)]
        public string ControllerName { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed), Index]
        public int ControllerHash { get; private set; }
    }
}
