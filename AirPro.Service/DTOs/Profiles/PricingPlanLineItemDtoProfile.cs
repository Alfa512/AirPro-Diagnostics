using AirPro.Service.DTOs.Concrete;
using AirPro.Service.DTOs.Interface;
using AutoMapper;

namespace AirPro.Service.DTOs.Profiles
{
    public class PricingPlanLineItemDtoProfile : Profile
    {
        public PricingPlanLineItemDtoProfile()
        {
            CreateMap<IPricingPlanLineItemDto, PricingPlanLineItemDto>();
        }
    }
}