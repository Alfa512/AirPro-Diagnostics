using System;

namespace AirPro.Service.DTOs.Interface
{
    public interface IShopVehicleMakesPricingDto
    {
        int VehicleMakeId { get; set; }
        int PricingPlanId { get; set; }
        Guid ShopGuid { get; set; }
    }
}