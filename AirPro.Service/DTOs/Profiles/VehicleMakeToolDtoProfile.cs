using AirPro.Entities.Repair;
using AirPro.Service.DTOs.Interface;
using AutoMapper;

namespace AirPro.Service.DTOs.Profiles
{
    public class VehicleMakeToolDtoProfile : Profile
    {
        public VehicleMakeToolDtoProfile()
        {
            CreateMap<IVehicleMakeToolDto, VehicleMakeToolEntityModel>()
                .ReverseMap();
        }
    }
}
