using AirPro.Service.DTOs.Interface;
using AutoMapper;

namespace AirPro.Site.Areas.Admin.Models.Access
{
    public class UserViewModelProfile : Profile
    {
        public UserViewModelProfile()
        {
            CreateMap<IUserDto, UserViewModel>();
        }
    }
}