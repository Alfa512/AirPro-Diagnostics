using AirPro.Entities.Diagnostics;
using AirPro.Service.DTOs.Concrete;
using AirPro.Service.DTOs.Interface;
using AutoMapper;

namespace AirPro.Service.DTOs.Profiles
{
    public class DiagnosticToolProfile : Profile
    {
        public DiagnosticToolProfile()
        {
            CreateMap<DiagnosticToolEntityModel, DiagnosticToolDto>();
            CreateMap<IDiagnosticToolDto, DiagnosticToolEntityModel>();
        }
    }
}
