using AirPro.Service.DTOs.Interface;
using AutoMapper;

namespace AirPro.Site.Areas.Billing.Models
{
    public class EstimatePlanViewModelProfile : Profile
    {
        public EstimatePlanViewModelProfile()
        {
            CreateMap<IEstimatePlanVehicleDto, EstimatePlanVehicleViewModel>()
                .ReverseMap();

            CreateMap<IEstimatePlanDto, EstimatePlanViewModel>()
                .ForMember(d => d.VehiclePlans, opt => opt.MapFrom(d => d.VehiclePlans))
                .ReverseMap();
        }
    }
}