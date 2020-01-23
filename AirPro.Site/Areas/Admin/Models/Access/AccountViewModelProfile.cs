using AirPro.Service.DTOs.Interface;
using AutoMapper;

namespace AirPro.Site.Areas.Admin.Models.Access
{
    public class AccountViewModelProfile : Profile
    {
        public AccountViewModelProfile()
        {
            CreateMap<IAccountDto, AccountViewModel>();
        }
    }
}