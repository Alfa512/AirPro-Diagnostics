using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;
using AirPro.Entities.Interfaces;

namespace AirPro.Entities.Access
{
    [Table("Users", Schema = "Access")]
    public class UserEntityModel : IdentityUser<Guid, UserLoginEntityModel, UserRoleEntityModel, UserClaimEntityModel>, IUserEntityModel, IAuditBaseEntityModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override Guid Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string DisplayName { get; set; }

        public string JobTitle { get; set; }
        public string ContactNumber { get; set; }
        public override string PhoneNumber { get; set; }

        [ForeignKey(nameof(UserShopEntityModel.UserGuid))]
        public virtual ICollection<UserShopEntityModel> UserShops { get; set; }

        [ForeignKey(nameof(UserAccountEntityModel.UserGuid))]
        public virtual ICollection<UserAccountEntityModel> UserAccounts { get; set; }

        [ForeignKey(nameof(UserGroupEntityModel.UserGuid))]
        public virtual ICollection<UserGroupEntityModel> UserGroups { get; set; }

        [ForeignKey(nameof(UserRoleEntityModel.UserId))]
        public virtual ICollection<UserRoleEntityModel> UserRoles { get; set; }

        [ForeignKey(nameof(ShopEntityModel.EmployeeGuid))]
        public virtual ICollection<ShopEntityModel> EmployeeShops { get; set; }

        [ForeignKey(nameof(ShopEntityModel.EmployeeGuid))]
        public virtual ICollection<AccountEntityModel> EmployeeAccounts { get; set; }

        public bool ShopBillingNotification { get; set; }
        public bool ShopReportNotification { get; set; }
        public bool ShopStatementNotification { get; set; }

        public string TimeZoneInfoId { get; set; } = "Eastern Standard Time";

        [MaxLength(24), Index]
        public string SessionId { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<UserEntityModel, Guid> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            // Add custom user claims here
            return userIdentity;
        }

        public bool GetAccountLocked => LockoutEnabled && LockoutEndDateUtc != null && LockoutEndDateUtc > DateTime.UtcNow;
        public string GetDisplayName => $"{LastName}, {FirstName}";

        [Required, Index]
        public Guid CreatedByUserGuid { get; set; }
        [Required]
        public DateTimeOffset CreatedDt { get; set; } = DateTimeOffset.UtcNow;

        [Index]
        public Guid? UpdatedByUserGuid { get; set; }
        public DateTimeOffset? UpdatedDt { get; set; } = null;
        public bool EmployeeInd { get; set; }
    }
}
