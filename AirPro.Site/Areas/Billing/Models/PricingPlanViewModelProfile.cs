using AirPro.Service.DTOs.Interface;
using AutoMapper;

namespace AirPro.Site.Areas.Billing.Models
{
    public class PricingPlanViewModelProfile : Profile
    {
        public PricingPlanViewModelProfile()
        {
            CreateMap<IPricingPlanDto, PricingPlanViewModel>().ReverseMap();
        }
    }
}