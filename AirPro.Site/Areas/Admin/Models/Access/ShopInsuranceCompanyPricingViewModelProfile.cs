using AirPro.Service.DTOs.Interface;
using AutoMapper;

namespace AirPro.Site.Areas.Admin.Models.Access
{
    public class ShopInsuranceCompanyPricingViewModelProfile: Profile
    {
        public ShopInsuranceCompanyPricingViewModelProfile()
        {
            CreateMap<IShopInsuranceCompanyPlanDto, ShopInsuranceCompanyPlanViewModel>()
                .ReverseMap();
        }
    }
}