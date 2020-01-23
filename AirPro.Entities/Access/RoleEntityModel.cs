using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AirPro.Entities.Access
{
    [Table("Roles", Schema = "Access")]
    public class RoleEntityModel : IdentityRole<Guid, UserRoleEntityModel>
    {

    }
}
