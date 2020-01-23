using AirPro.Entities.Scan;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirPro.Entities.Access
{
    [Table("ShopRequestTypes", Schema = "Access")]
    public class ShopRequestTypeEntityModel
    {
        [Column(Order = 0), Key, Required, ForeignKey(nameof(Shop))]
        public Guid ShopGuid { get; set; }

        [Column(Order = 1), Key, Required, ForeignKey(nameof(RequestType))]
        public int RequestTypeId { get; set; }

        public virtual ShopEntityModel Shop { get; set; }
        public virtual RequestTypeEntityModel RequestType { get; set; }
    }
}
