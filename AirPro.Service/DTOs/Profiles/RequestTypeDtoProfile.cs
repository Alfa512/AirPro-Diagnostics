using AirPro.Entities.Scan;
using AirPro.Service.DTOs.Concrete;
using AirPro.Service.DTOs.Interface;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirPro.Service.DTOs.Profiles
{
    public class RequestTypeDtoProfile : Profile
    {
        public RequestTypeDtoProfile()
        {
            CreateMap<RequestTypeEntityModel, RequestTypeDto>();
            CreateMap<IRequestTypeDto, RequestTypeEntityModel>();
        }
    }
}
