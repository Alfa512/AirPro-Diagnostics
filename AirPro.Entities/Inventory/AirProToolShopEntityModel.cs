using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AirPro.Entities.Access;

namespace AirPro.Entities.Inventory
{
    [Table("AirProToolShops", Schema = "Inventory")]
    public class AirProToolShopEntityModel : AuditBaseEntityModel
    {
        [Key, Column(Order = 0), ForeignKey(nameof(Tool))]
        public int ToolId { get; set; }
        public virtual AirProToolEntityModel Tool { get; set; }

        [Key, Column(Order = 1), ForeignKey(nameof(Shop))]
        public Guid ShopGuid { get; set; }
        public virtual ShopEntityModel Shop { get; set; }
    }
}
