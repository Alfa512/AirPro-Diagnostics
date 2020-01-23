using AirPro.Service.DTOs.Concrete;
using AirPro.Service.DTOs.Interface;
using AutoMapper;

namespace AirPro.Service.DTOs.Profiles
{
    public class WorkTypeGroupDtoProfile : Profile
    {
        public WorkTypeGroupDtoProfile()
        {
            CreateMap<IWorkTypeGroupDto, WorkTypeGroupDto>();
        }
    }
}