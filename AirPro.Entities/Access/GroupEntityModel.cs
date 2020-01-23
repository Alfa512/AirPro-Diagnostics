using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AirPro.Entities.Interfaces;

namespace AirPro.Entities.Access
{
    [Table("Groups", Schema = "Access")]
    public class GroupEntityModel : AuditBaseEntityModel, IGroupEntityModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid GroupGuid { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        [ForeignKey(nameof(GroupRoleEntityModel.GroupGuid))]
        public virtual ICollection<GroupRoleEntityModel> Roles { get; set; }

        [ForeignKey(nameof(UserGroupEntityModel.UserGuid))]
        public virtual ICollection<UserGroupEntityModel> GroupUsers { get; set; }
    }
}
