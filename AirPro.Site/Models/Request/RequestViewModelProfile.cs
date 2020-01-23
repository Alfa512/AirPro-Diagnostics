using AirPro.Service.DTOs.Interface;
using AutoMapper;

namespace AirPro.Site.Models.Request
{
    public class RequestViewModelProfile : Profile
    {
        public RequestViewModelProfile()
        {
            CreateMap<IRequestDto, RequestViewModel>().ReverseMap();
        }
    }
}