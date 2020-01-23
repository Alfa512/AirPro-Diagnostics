using AirPro.Service.DTOs.Interface;
using AutoMapper;

namespace AirPro.Site.Models.Client
{
    public class CreateViewModelProfile : Profile
    {
        public CreateViewModelProfile()
        {
            CreateMap<IShopInsuranceCompanyPlanDto, ShopInsuranceCompanyPlanViewModel>();
            CreateMap<IShopVehicleMakesPricingDto, ShopVehicleMakesPricingViewModel>();
            CreateMap<IRegistrationUserDto, UserDetailsViewModel>();
            CreateMap<IRegistrationAccountDto, AccountInformationViewModel>();
            CreateMap<IRegistrationShopDto, ShopInformationViewModel>()
                .ForMember(dest => dest.ShopInsuranceCompanyPlans, opt => opt.MapFrom(src => src.ShopInsuranceCompanyPlans))
                .ForMember(dest => dest.ShopInsuranceCompanyEstimatePlans, opt => opt.MapFrom(src => src.ShopInsuranceCompanyEstimatePlans))
                .ForMember(dest => dest.ShopVehicleMakesPricingPlans, opt => opt.MapFrom(src => src.ShopVehicleMakesPricingPlans));

            CreateMap<IRegistrationDto, CreateViewModel>()
                .ForMember(dest => dest.UserDetails, opt => opt.MapFrom(src => src.RegistrationUser))
                .ForMember(dest => dest.AccountInformation, opt => opt.MapFrom(src => src.RegistrationAccount))
                .ForMember(dest => dest.ShopInformation, opt => opt.MapFrom(src => src.RegistrationShop))
                .ForMember(dest => dest.DifferentShopInfo, opt => opt.MapFrom(src => src.DifferentShopInfo))
                .ForMember(dest => dest.RegistrationUser, opt => opt.Ignore())
                .ForMember(dest => dest.RegistrationAccount, opt => opt.Ignore())
                .ForMember(dest => dest.RegistrationShop, opt => opt.Ignore());
            

            CreateMap<UserDetailsViewModel, IRegistrationUserDto>()
                .ForMember(dest => dest.RegistrationUserId, opt => opt.Ignore())
                .ForMember(dest => dest.AccessGroupIds, opt => opt.Ignore());
            CreateMap<AccountInformationViewModel, IRegistrationAccountDto>()
                .ForMember(dest => dest.RegistrationAccountId, opt => opt.Ignore())
                .ForMember(dest => dest.DiscountPercentage, opt => opt.Ignore());
            CreateMap<ShopInformationViewModel, IRegistrationShopDto>()
                .ForMember(dest => dest.RegistrationShopId, opt => opt.Ignore())
                .ForMember(dest => dest.AdditionalScanCost, opt => opt.Ignore())
                .ForMember(dest => dest.AllowAllRepairAutoClose, opt => opt.Ignore())
                .ForMember(dest => dest.AllowAutoRepairClose, opt => opt.Ignore())
                .ForMember(dest => dest.AllowedRequestTypes, opt => opt.Ignore())
                .ForMember(dest => dest.AllowScanAnalysisAutoClose, opt => opt.Ignore())
                .ForMember(dest => dest.AllowShopBillingNotification, opt => opt.Ignore())
                .ForMember(dest => dest.AllowShopStatementNotification, opt => opt.Ignore())
                .ForMember(dest => dest.AutomaticRepairCloseDays, opt => opt.Ignore())
                .ForMember(dest => dest.BillingCycleId, opt => opt.Ignore())
                .ForMember(dest => dest.CurrencyId, opt => opt.Ignore())
                .ForMember(dest => dest.DefaultInsuranceCompanyId, opt => opt.Ignore())
                .ForMember(dest => dest.DiscountPercentage, opt => opt.Ignore())
                .ForMember(dest => dest.EstimatePlanId, opt => opt.Ignore())
                .ForMember(dest => dest.FirstScanCost, opt => opt.Ignore())
                .ForMember(dest => dest.HideFromReports, opt => opt.Ignore())
                .ForMember(dest => dest.ShopInsuranceCompanyEstimatePlans, opt => opt.Ignore())
                .ForMember(dest => dest.ShopInsuranceCompanyPlans, opt => opt.Ignore())
                .ForMember(dest => dest.PricingPlanId, opt => opt.Ignore())
                .ForMember(dest => dest.ShopFixedPriceInd, opt => opt.Ignore())
                .ForMember(dest => dest.ShopInsuranceCompanyEstimatePlans, opt => opt.Ignore())
                .ForMember(dest => dest.ShopInsuranceCompanyPlans, opt => opt.Ignore())
                .ForMember(dest => dest.ShopVehicleMakesPricingPlans, opt => opt.Ignore());

            CreateMap<CreateViewModel, IRegistrationDto>()
                .ForMember(dest => dest.RegistrationUser, opt => opt.MapFrom(src => src.UserDetails))
                .ForMember(dest => dest.RegistrationAccount, opt => opt.MapFrom(src => src.AccountInformation))
                .ForMember(dest => dest.RegistrationShop, opt => opt.MapFrom(src => src.ShopInformation))
                .ForMember(dest => dest.DifferentShopInfo, opt => opt.MapFrom(src => src.AccountInformation.DifShopInfo))
                .ForMember(dest => dest.RegistrationId, opt => opt.Ignore());
        }
    }
}