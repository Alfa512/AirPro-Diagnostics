using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AirPro.Entities.Access
{
    [Table("UserRoles", Schema = "Access")]
    public class UserRoleEntityModel : IdentityUserRole<Guid>
    {
        [ForeignKey(nameof(UserId))]
        public virtual UserEntityModel User { get; set; }

        [ForeignKey(nameof(RoleId))]
        public virtual RoleEntityModel Role { get; set; }
    }
}
