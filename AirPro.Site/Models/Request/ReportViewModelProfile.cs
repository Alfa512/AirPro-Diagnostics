using System.Linq;
using AirPro.Service;
using AirPro.Service.DTOs.Concrete;
using AirPro.Service.DTOs.Interface;
using AutoMapper;

namespace AirPro.Site.Models.Request
{
    public class ReportViewModelProfile : Profile
    {
        public ReportViewModelProfile()
        {
            CreateMap<IReportDto, ReportViewModel>()
                .ForMember(d => d.VehicleMakeTools, opt => opt.MapFrom(s => s.VehicleMakeTools.Select(x => new ReportVehicleMakeToolViewModel { CheckedInd = x.CheckedInd, VehicleMakeToolId = x.VehicleMakeToolId, ToolVersion = x.ToolVersion, Name = x.Name })))
                .ForMember(d => d.ReportPreviewHtml, opt => opt.MapFrom(s => s.GetReportDtoHtml()));

            CreateMap<ReportViewModel, IReportDto>()
                .ForMember(d => d.VehicleMakeTools, opt => opt.MapFrom(s => s.VehicleMakeTools.Select(x => new ReportVehicleMakeToolDto { CheckedInd =  x.CheckedInd, VehicleMakeToolId =  x.VehicleMakeToolId, ToolVersion = x.ToolVersion })));
        }
    }
}