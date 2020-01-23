using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AirPro.Service.DTOs.Concrete;
using AirPro.Service.DTOs.Interface;
using AutoMapper;

namespace AirPro.Site.Areas.Admin.Models.TechProfile
{
    public class TechnicianCoverageViewModelProfile : Profile
    {
        public TechnicianCoverageViewModelProfile()
        {
            CreateMap<ITechnicianCoverageDto, TechnicianCoverageViewModel>();
            CreateMap<TechnicianCoverageRowItemDto, TechnicianCoverageRowItemViewModel>();
        }
    }
}