using AirPro.Entities.Access;
using AirPro.Service.DTOs.Concrete;
using AirPro.Service.DTOs.Interface;
using AutoMapper;

namespace AirPro.Service.DTOs.Profiles
{
    public class RegistrationDtoProfile : Profile
    {
        public RegistrationDtoProfile()
        {
            CreateMap<RegistrationEntityModel, IRegistrationDto>()
                .ForMember(d => d.CreatedUser, opt => opt.MapFrom(src => src.RegistrationUser.GetDisplayName))
                .ForMember(d => d.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy.DisplayName))
                .ForMember(d => d.CompletedBy, opt => opt.MapFrom(src => src.CompletedBy.DisplayName))
                .ForMember(d => d.StatusUpdateBy, opt => opt.MapFrom(src => src.StatusUpdateBy.DisplayName));

            CreateMap<IRegistrationDto, RegistrationEntityModel>()
                .ForMember(d => d.Client, opt => opt.Ignore())
                .ForMember(d => d.UpdatedDt, opt => opt.Ignore())
                .ForMember(d => d.CompletedBy, opt => opt.Ignore())
                .ForMember(d => d.CreatedBy, opt => opt.Ignore())
                .ForMember(d => d.StatusUpdateBy, opt => opt.Ignore())
                .ForMember(d => d.UpdatedBy, opt => opt.Ignore());
        }
    }
}
