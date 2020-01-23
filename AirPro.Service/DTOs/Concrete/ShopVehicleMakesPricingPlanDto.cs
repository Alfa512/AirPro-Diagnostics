using AirPro.Service.DTOs.Interface;
using System;

namespace AirPro.Service.DTOs.Concrete
{
    internal class ShopVehicleMakesPricingDto : IShopVehicleMakesPricingDto
    {
        public int VehicleMakeId { get; set; }
        public int PricingPlanId { get; set; }
        public Guid ShopGuid { get; set; }
    }
}