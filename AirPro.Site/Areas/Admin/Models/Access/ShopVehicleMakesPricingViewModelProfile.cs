using AirPro.Service.DTOs.Interface;
using AutoMapper;

namespace AirPro.Site.Areas.Admin.Models.Access
{
    public class ShopVehicleMakesPricingViewModelProfile : Profile
    {
        public ShopVehicleMakesPricingViewModelProfile()
        {
            CreateMap<IShopVehicleMakesPricingDto, ShopVehicleMakesPricingViewModel>()
                .ReverseMap();
        }
    }
}