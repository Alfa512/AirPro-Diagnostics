using AirPro.Entities.Repair;
using AirPro.Service.DTOs.Interface;
using AutoMapper;

namespace AirPro.Service.DTOs.Profiles
{
    public class FeedbackDtoProfile : Profile
    {
        public FeedbackDtoProfile()
        {
            CreateMap<IFeedbackDto, FeedbackEntityModel>()
                .ReverseMap();
        }
    }
}