using AirPro.Service.DTOs.Interface;
using AutoMapper;

namespace AirPro.Site.Areas.Billing.Models
{
    public class PricingPlanLineItemViewModelProfile : Profile
    {
        public PricingPlanLineItemViewModelProfile()
        {
            CreateMap<IPricingPlanLineItemDto, PricingPlanLineItemViewModel>().ReverseMap();
        }
    }
}