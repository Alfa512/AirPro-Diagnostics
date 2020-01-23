using System.Linq.Dynamic;
using AirPro.Entities.Repair;
using AirPro.Service.DTOs.Interface;
using AutoMapper;
using System.Linq;
using AirPro.Service.DTOs.Concrete;

namespace AirPro.Service.DTOs.Profiles
{
    public class VehicleMakeDtoProfile : Profile
    {
        public VehicleMakeDtoProfile()
        {
            CreateMap<IVehicleMakeDto, VehicleMakeEntityModel>()
                .ForMember(dest => dest.ProgrammTools, opt => opt.MapFrom(src => src.ProgramTools.Select(x => new VehicleMakeToolEntityModel() { Name = x.Name })));

            CreateMap<VehicleMakeEntityModel, IVehicleMakeDto>();
            CreateMap<VehicleMakeEntityModel, VehicleMakeDto>()
                .ForMember(dest => dest.ProgramTools, opt => opt.MapFrom(src => src.ProgrammTools.Select(x => new VehicleMakeToolDto { Name = x.Name, VehicleMakeToolId = x.VehicleMakeToolId })));
        }
    }
}
