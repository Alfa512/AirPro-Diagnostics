using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirPro.Entities.Common
{
    [Table("NoteTypes", Schema = "Common")]
    public class NoteTypeEntityModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NoteTypeId { get; set; }
        [MaxLength(50)]
        public string NoteTypeName { get; set; }
    }
}