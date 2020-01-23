using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AirPro.Entities.Interfaces;

namespace AirPro.Entities.Access
{
    [Table("UserGroups", Schema = "Access")]
    public class UserGroupEntityModel : AuditBaseEntityModel, IUserGroupEntityModel
    {
        [Column(Order = 0), Key, Required, ForeignKey(nameof(User))]
        public Guid UserGuid { get; set; }
        public virtual UserEntityModel User { get; set; }

        [Column(Order = 1), Key, Required, ForeignKey(nameof(Group))]
        public Guid GroupGuid { get; set; }
        public virtual GroupEntityModel Group { get; set; }
    }
}
