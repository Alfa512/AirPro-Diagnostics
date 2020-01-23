using System;

namespace AirPro.Entities.Interfaces
{
    public interface IUserShopEntityModel
    {
        Guid ShopGuid { get; set; }
        Guid UserGuid { get; set; }
    }
}