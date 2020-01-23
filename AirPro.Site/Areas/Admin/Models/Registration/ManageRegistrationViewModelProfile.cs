using AirPro.Service.DTOs.Interface;
using AutoMapper;

namespace AirPro.Site.Areas.Admin.Models.Registration
{
    public class ManageRegistrationViewModelProfile : Profile
    {
        public ManageRegistrationViewModelProfile()
        {
            CreateMap<IRegistrationAccountDto, AccountInformationViewModel>();
            CreateMap<IRegistrationShopDto, ShopInformationViewModel>();
            CreateMap<IRegistrationUserDto, UserDetailsViewModel>();
            CreateMap<IRegistrationOptionsDto, ManageRegistrationViewModel>();

            CreateMap<IRegistrationDto, ManageRegistrationViewModel>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.RegistrationUser))
                .ForMember(dest => dest.Account, opt => opt.MapFrom(src => src.RegistrationAccount))
                .ForMember(dest => dest.Shop, opt => opt.MapFrom(src => src.RegistrationShop))

                .ForMember(dest => dest.RegistrationAccount, opt => opt.Ignore())
                .ForMember(dest => dest.RegistrationShop, opt => opt.Ignore())
                .ForMember(dest => dest.RegistrationUser, opt => opt.Ignore());
        }
    }
}