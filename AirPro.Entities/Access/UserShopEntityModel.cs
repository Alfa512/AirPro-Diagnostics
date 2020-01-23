using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AirPro.Entities.Interfaces;

namespace AirPro.Entities.Access
{
    [Table("UserShops", Schema = "Access")]
    public class UserShopEntityModel : AuditBaseEntityModel, IUserShopEntityModel
    {
        [Column(Order = 0), Key, Required, ForeignKey(nameof(User))]
        public Guid UserGuid { get; set; }
        public virtual UserEntityModel User { get; set; }

        [Column(Order = 1), Key, Required, ForeignKey(nameof(Shop))]
        public Guid ShopGuid { get; set; }
        public virtual ShopEntityModel Shop { get; set; }
    }
}
