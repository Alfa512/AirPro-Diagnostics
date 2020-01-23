using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AirPro.Entities.Access
{
    [Table("UserLogins", Schema = "Access")]
    public class UserLoginEntityModel : IdentityUserLogin<Guid>
    {
    }
}
