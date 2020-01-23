using AirPro.Entities.Service;
using AirPro.Service.DTOs.Concrete;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirPro.Service.DTOs.Profiles
{
    public class CCCInsuranceCompanyDtoProfile : Profile
    {
        public CCCInsuranceCompanyDtoProfile()
        {
            CreateMap<CCCInsuranceCompanyDto, CCCInsuranceCompanyEntityModel>().ReverseMap();
        }
    }
}
