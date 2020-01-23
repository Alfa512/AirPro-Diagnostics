using AirPro.Service.DTOs.Interface;
using AutoMapper;

namespace AirPro.Site.Areas.Admin.Models.Recommendation
{
    public class RecommendationViewModelProfile : Profile
    {
        public RecommendationViewModelProfile()
        {
            CreateMap<RecommendationViewModel, ITroubleCodeRecommendationDto>().ReverseMap();
        }
    }
}