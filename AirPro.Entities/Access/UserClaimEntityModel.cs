using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AirPro.Entities.Access
{
    [Table("UserClaims", Schema = "Access")]
    public class UserClaimEntityModel : IdentityUserClaim<Guid>
    {
    }
}
