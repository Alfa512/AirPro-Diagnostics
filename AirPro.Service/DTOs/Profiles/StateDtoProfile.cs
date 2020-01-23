using AirPro.Entities.Common;
using AirPro.Service.DTOs.Concrete;
using AutoMapper;

namespace AirPro.Service.DTOs.Profiles
{
    public class StateDtoProfile : Profile
    {
        public StateDtoProfile()
        {
            CreateMap<StateEntityModel, StateDto>();
        }
    }
}