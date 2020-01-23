using AirPro.Service.DTOs.Interface;
using AutoMapper;

namespace AirPro.Site.Areas.Admin.Models.Decision
{
    public class DecisionViewModelProfile : Profile
    {
        public DecisionViewModelProfile()
        {
            CreateMap<DecisionViewModel, IDecisionDto>().ReverseMap();
        }
    }
}