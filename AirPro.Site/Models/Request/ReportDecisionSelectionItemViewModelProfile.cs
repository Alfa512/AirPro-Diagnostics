using AirPro.Service.DTOs.Interface;
using AutoMapper;

namespace AirPro.Site.Models.Request
{
    public class ReportDecisionSelectionItemViewModelProfile : Profile
    {
        public ReportDecisionSelectionItemViewModelProfile()
        {
            CreateMap<IReportDecisionSelectionItemDto, ReportDecisionSelectionItemViewModel>().ReverseMap();
        }
    }
}