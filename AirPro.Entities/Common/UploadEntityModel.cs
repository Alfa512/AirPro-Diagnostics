using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AirPro.Entities.Access;

namespace AirPro.Entities.Common
{
    [Table("Uploads", Schema = "Common")]
    public class UploadEntityModel : AuditBaseEntityModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UploadId { get; set; }
        [Required, MaxLength(50)]
        public string UploadKey { get; set; }
        [Required]
        public int UploadTypeId { get; set; }
        [ForeignKey(nameof(UploadTypeId))]
        public virtual UploadTypeEntityModel UploadType { get; set; }
        [MaxLength(150)]
        public string UploadFileName { get; set; }
        [MaxLength(10)]
        public string UploadFileExtension { get; set; }
        public long UploadFileSizeBytes { get; set; }
        [MaxLength(50)]
        public string UploadStorageName { get; set; }
        [MaxLength(100)]
        public string UploadMimeType { get; set; }
        public bool UploadDeletedInd { get; set; }
        public Guid? UploadDeletedByUserGuid { get; set; }
        [ForeignKey(nameof(UploadDeletedByUserGuid))]
        public virtual UserEntityModel UploadDeletedByUser { get; set; }
        public DateTimeOffset? UploadDeletedDt { get; set; }
    }
}
