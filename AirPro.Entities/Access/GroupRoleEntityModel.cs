using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AirPro.Entities.Interfaces;

namespace AirPro.Entities.Access
{
    [Table("GroupRoles", Schema = "Access")]
    public class GroupRoleEntityModel : AuditBaseEntityModel, IGroupRoleEntityModel
    {
        [Column(Order = 0), Key, Required, ForeignKey(nameof(Group))]
        public Guid GroupGuid { get; set; }
        public virtual GroupEntityModel Group { get; set; }

        [Column(Order = 1), Key, Required, ForeignKey(nameof(Role))]
        public Guid RoleGuid { get; set; }
        public virtual RoleEntityModel Role { get; set; }
    }
}
