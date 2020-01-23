using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AirPro.Entities.Access;

namespace AirPro.Entities.Common
{
    [Table("Notes", Schema = "Common")]
    public class NoteEntityModel : AuditBaseEntityModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NoteId { get; set; }
        [Required, MaxLength(50)]
        public string NoteKey { get; set; }
        [Required]
        public int NoteTypeId { get; set; }
        [ForeignKey(nameof(NoteTypeId))]
        public virtual NoteTypeEntityModel NoteType { get; set; }
        public string NoteDescription { get; set; }
        public bool NoteDeletedInd { get; set; }
        public Guid? NoteDeletedByUserGuid { get; set; }
        [ForeignKey(nameof(NoteDeletedByUserGuid))]
        public virtual UserEntityModel NoteDeletedByUser { get; set; }
        public DateTimeOffset? NoteDeletedDt { get; set; }
    }
}
