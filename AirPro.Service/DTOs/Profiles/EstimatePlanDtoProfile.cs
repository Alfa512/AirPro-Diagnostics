using AirPro.Entities.Billing;
using AirPro.Service.DTOs.Concrete;
using AirPro.Service.DTOs.Interface;
using AutoMapper;

namespace AirPro.Service.DTOs.Profiles
{
    public class EstimatePlanDtoProfile : Profile
    {
        public EstimatePlanDtoProfile()
        {
            CreateMap<EstimatePlanVehicleEntityModel, IEstimatePlanVehicleDto>()
                .ForMember(d => d.VehicleMakeName, opt => opt.MapFrom(d => d.VehicleMake.VehicleMakeName))
                .ReverseMap();

            CreateMap<EstimatePlanEntityModel, IEstimatePlanDto>()
                .ForMember(d => d.VehiclePlans, opt => opt.MapFrom(d => d.EstimatePlanVehicles));

            CreateMap<IEstimatePlanDto, EstimatePlanDto>();
        }
    }
}