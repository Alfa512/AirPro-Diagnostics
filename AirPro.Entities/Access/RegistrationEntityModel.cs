using AirPro.Common.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirPro.Entities.Access
{
    [Table("Registrations", Schema = "Access")]
    public class RegistrationEntityModel : AuditBaseEntityModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid RegistrationId { get; set; }
        public RegistrationStatus RegistrationStatus { get; set; }
        public DateTimeOffset? StatusUpdateDt { get; set; } = null;

        public string CallbackUrl { get; set; }
        public string Email { get; set; }
        public bool DifferentShopInfo { get; set; }
        public DateTimeOffset? CompletedDt { get; set; } = null;

        public int RegistrationUserId { get; set; }
        [ForeignKey(nameof(RegistrationUserId))]
        public virtual RegistrationUserEntityModel RegistrationUser { get; set; }

        public int RegistrationAccountId { get; set; }
        [ForeignKey(nameof(RegistrationAccountId))]
        public virtual RegistrationAccountEntityModel RegistrationAccount { get; set; }

        public int RegistrationShopId { get; set; }
        [ForeignKey(nameof(RegistrationShopId))]
        public virtual RegistrationShopEntityModel RegistrationShop { get; set; }

        public Guid? AccountGuid { get; set; }
        [ForeignKey(nameof(AccountGuid))]
        public virtual AccountEntityModel Account { get; set; }

        public Guid? ShopGuid { get; set; }
        [ForeignKey(nameof(ShopGuid))]
        public virtual ShopEntityModel Shop { get; set; }

        public Guid? ClientUserGuid { get; set; }
        [ForeignKey(nameof(ClientUserGuid))]
        public UserEntityModel Client { get; set; }

        public Guid? StatusUpdateByUserGuid { get; set; }
        [ForeignKey(nameof(StatusUpdateByUserGuid))]
        public virtual UserEntityModel StatusUpdateBy { get; set; }
        public Guid? CompletedByUserGuid { get; set; }
        [ForeignKey(nameof(CompletedByUserGuid))]
        public virtual UserEntityModel CompletedBy { get; set; }

        public int? PassedStep { get; set; }
    }
}
