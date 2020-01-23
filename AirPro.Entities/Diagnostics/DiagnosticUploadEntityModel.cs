using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirPro.Entities.Diagnostics
{
    [Table("Uploads", Schema = "Diagnostic")]
    public class DiagnosticUploadEntityModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UploadId { get; set; }

        public int? ResultId { get; set; }
        [ForeignKey(nameof(ResultId))]
        public virtual DiagnosticResultEntityModel Result { get; set; }

        public int UploadFileTypeId { get; set; }
        [ForeignKey(nameof(UploadFileTypeId))]
        public virtual DiagnosticUploadFileTypeEntityModel UploadFileType { get; set; }

        public string UploadText { get; set; }
    }
}
