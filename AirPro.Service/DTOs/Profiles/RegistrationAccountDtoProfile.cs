using AirPro.Entities.Access;
using AirPro.Service.DTOs.Interface;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirPro.Service.DTOs.Profiles
{
    public class RegistrationAccountDtoProfile : Profile
    {
        public RegistrationAccountDtoProfile()
        {
            CreateMap<RegistrationAccountEntityModel, IRegistrationAccountDto>();
                //.ForMember(dest => dest.StateId, opt => opt.Ignore());

            CreateMap<IRegistrationAccountDto, RegistrationAccountEntityModel>();
                //.ForMember(dest => dest.StateId, opt => opt.Ignore());
        }
    }
}
