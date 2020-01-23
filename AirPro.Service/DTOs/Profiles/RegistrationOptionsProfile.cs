using AirPro.Service.DTOs.Interface;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirPro.Service.DTOs.Profiles
{
    public class RegistrationOptionsProfile : Profile
    {
        public RegistrationOptionsProfile()
        {
            CreateMap<IRegistrationOptionsDto, IRegistrationOptionsDto>();
        }
    }
}
