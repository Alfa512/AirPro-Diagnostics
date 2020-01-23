using System;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Site.Areas.Admin.Models.Access
{
    public class ShopVehicleMakesPricingViewModel : IShopVehicleMakesPricingDto
    {
        public int VehicleMakeId { get; set; }
        public int PricingPlanId { get; set; }
        public Guid ShopGuid { get; set; }
    }
}