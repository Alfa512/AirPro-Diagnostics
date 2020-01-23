using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirPro.Entities.Access;

namespace AirPro.Entities.Common
{
    [Table("ReleaseNotes", Schema = "Common")]
    public class ReleaseNoteEntityModel : AuditBaseEntityModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReleaseNoteId { get; set; }

        public string Version { get; set; }
        public string Summary { get; set; }
        public string DevelopmentId { get; set; }
        public string ReleaseNote { get; set; }
        public bool DeletedInd { get; set; }


        [ForeignKey(nameof(ReleaseNoteRoleEntityModel.ReleaseNoteId))]
        public virtual ICollection<ReleaseNoteRoleEntityModel> ReleaseNoteRoles { get; set; }
    }
}
