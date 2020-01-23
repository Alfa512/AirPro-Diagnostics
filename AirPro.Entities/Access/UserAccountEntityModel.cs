using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AirPro.Entities.Interfaces;

namespace AirPro.Entities.Access
{
    [Table("UserAccounts", Schema = "Access")]
    public class UserAccountEntityModel : AuditBaseEntityModel, IUserAccountEntityModel
    {
        [Column(Order = 0), Key, Required, ForeignKey(nameof(User))]
        public Guid UserGuid { get; set; }
        public virtual UserEntityModel User { get; set; }

        [Column(Order = 1), Key, Required, ForeignKey(nameof(Account))]
        public Guid AccountGuid { get; set; }
        public virtual AccountEntityModel Account { get; set; }
    }
}
