using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirPro.Entities.Diagnostics
{
    [Table("UploadFileTypes", Schema = "Diagnostic")]
    public class DiagnosticUploadFileTypeEntityModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UploadFileTypeId { get; set; }
        [MaxLength(20)]
        public string UploadFileTypeName { get; set; }
    }
}