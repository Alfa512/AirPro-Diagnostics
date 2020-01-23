using AirPro.Service.DTOs.Interface;
using AutoMapper;

namespace AirPro.Site.Models.Request
{
    public class DiagnosticResultViewModelProfile : Profile
    {
        public DiagnosticResultViewModelProfile()
        {
            CreateMap<IDiagnosticResultDto, DiagnosticResultViewModel>();
            CreateMap<IDiagnosticControllerDto, DiagnosticControllerViewModel>();
            CreateMap<IDiagnosticTroubleCodeDto, DiagnosticTroubleCodeViewModel>();
            CreateMap<IDiagnosticFreezeFrameDto, DiagnosticFreezeFrameViewModel>();
            CreateMap<IDiagnosticFreezeFrameSensorGroupDto, DiagnosticFreezeFrameSensorGroupViewModel>();
            CreateMap<IDiagnosticFreezeFrameSensorDto, DiagnosticFreezeFrameSensorViewModel>();
        }
    }
}