using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirPro.Entities.Common
{
    [Table("UploadTypes", Schema = "Common")]
    public class UploadTypeEntityModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UploadTypeId { get; set; }
        [MaxLength(50)]
        public string UploadTypeName { get; set; }
        [MaxLength(50)]
        public string UploadTypeSchema { get; set; }
        [MaxLength(50)]
        public string UploadTypeTable { get; set; }
    }
}