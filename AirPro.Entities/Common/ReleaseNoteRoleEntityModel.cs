using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirPro.Entities.Access;
using AirPro.Entities.Interfaces;

namespace AirPro.Entities.Common
{
    [Table("ReleaseNoteRoles", Schema = "Common")]
    public class ReleaseNoteRoleEntityModel : AuditBaseEntityModel
    {
        [Column(Order = 0), Key, Required, ForeignKey(nameof(ReleaseNote))]
        public int ReleaseNoteId { get; set; }
        public virtual ReleaseNoteEntityModel ReleaseNote { get; set; }

        [Column(Order = 1), Key, Required, ForeignKey(nameof(Role))]
        public Guid RoleGuid { get; set; }
        public virtual RoleEntityModel Role { get; set; }
    }
}
