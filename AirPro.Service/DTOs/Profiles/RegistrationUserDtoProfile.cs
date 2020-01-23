using AirPro.Entities.Access;
using AirPro.Service.DTOs.Interface;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AirPro.Service.DTOs.Profiles
{
    public class RegistrationUserDtoProfile : Profile
    {
        public RegistrationUserDtoProfile()
        {
            CreateMap<RegistrationUserEntityModel, IRegistrationUserDto>()
                .ForMember(dest => dest.AccessGroupIds, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.AccessGroupIds) ? new List<Guid>() : src.AccessGroupIds.Split(',').Select(x => Guid.Parse(x))));
            CreateMap<IRegistrationUserDto, RegistrationUserEntityModel>()
                .ForMember(dest => dest.AccessGroupIds, opt => opt.MapFrom(src => string.Join(",", src.AccessGroupIds ?? new List<Guid>())));
        }
    }
}
