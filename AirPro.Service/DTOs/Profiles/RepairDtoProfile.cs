using AirPro.Entities.Repair;
using AirPro.Service.DTOs.Interface;
using AutoMapper;

namespace AirPro.Service.DTOs.Profiles
{
    public class RepairDtoProfile : Profile
    {
        public RepairDtoProfile()
        {
            CreateMap<OrderEntityModel, IRepairDto>().ReverseMap();
        }
    }
}