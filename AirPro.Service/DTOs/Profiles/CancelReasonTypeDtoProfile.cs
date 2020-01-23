using AirPro.Entities.Scan;
using AirPro.Service.DTOs.Concrete;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirPro.Service.DTOs.Profiles
{
    public class CancelReasonTypeDtoProfile : Profile
    {
        public CancelReasonTypeDtoProfile()
        {
            CreateMap<CancelReasonTypeDto, CancelReasonTypeEntityModel>().ReverseMap();
        }
    }
}
