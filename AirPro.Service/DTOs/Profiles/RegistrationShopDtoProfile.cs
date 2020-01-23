using AirPro.Entities.Access;
using AirPro.Service.DTOs.Concrete;
using AirPro.Service.DTOs.Interface;
using AutoMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AirPro.Service.DTOs.Profiles
{
    public class RegistrationShopDtoProfile : Profile
    {
        public RegistrationShopDtoProfile()
        {
            CreateMap<RegistrationShopEntityModel, IRegistrationShopDto>()
                .ForMember(dest => dest.ShopInsuranceCompanyEstimatePlans, opt => opt.MapFrom(src => 
                    !string.IsNullOrWhiteSpace(src.InsuranceCompaniesEstimatePlansJson) ? 
                    JsonConvert.DeserializeObject<List<ShopInsuranceCompanyPlanDto>>(src.InsuranceCompaniesEstimatePlansJson).ToList<IShopInsuranceCompanyPlanDto>() : 
                    new List<IShopInsuranceCompanyPlanDto>()
                 ))
                .ForMember(dest => dest.ShopInsuranceCompanyPlans, opt => opt.MapFrom(src => 
                    !string.IsNullOrWhiteSpace(src.InsuranceCompaniesPricingPlansJson) ? 
                    JsonConvert.DeserializeObject<List<ShopInsuranceCompanyPlanDto>>(src.InsuranceCompaniesPricingPlansJson).ToList<IShopInsuranceCompanyPlanDto>() : 
                    new List<IShopInsuranceCompanyPlanDto>()
                ))
                .ForMember(dest => dest.ShopVehicleMakesPricingPlans, opt => opt.MapFrom(src => 
                    !string.IsNullOrWhiteSpace(src.VehicleMakesPricingPlansJson) ? 
                    JsonConvert.DeserializeObject<List<ShopVehicleMakesPricingDto>>(src.VehicleMakesPricingPlansJson).ToList<IShopVehicleMakesPricingDto>() : 
                    new List<IShopVehicleMakesPricingDto>()
                ))
                .ForMember(dest => dest.AllowShopBillingNotification, opt => opt.MapFrom(src => !src.DisableShopBillingNotification))
                .ForMember(dest => dest.AllowShopStatementNotification, opt => opt.MapFrom(src => !src.DisableShopStatementNotification))
                .ForMember(dest => dest.VehicleMakes, opt => opt.MapFrom(src => src.VehicleMakesIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x))))
                .ForMember(dest => dest.AllowedRequestTypes, opt => opt.MapFrom(src => src.AllowedRequestTypeIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x))))
                .ForMember(dest => dest.InsuranceCompanies, opt => opt.MapFrom(src => src.InsuranceCompaniesIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x))));

            CreateMap<IRegistrationShopDto, RegistrationShopEntityModel>()
                .ForMember(dest => dest.DefaultInsuranceCompanyId, opt => opt.MapFrom(src => src.DefaultInsuranceCompanyId == 0 ? null : src.DefaultInsuranceCompanyId))
                .ForMember(dest => dest.InsuranceCompaniesEstimatePlansJson, opt => opt.MapFrom(src => JsonConvert.SerializeObject(src.ShopInsuranceCompanyEstimatePlans ?? new List<IShopInsuranceCompanyPlanDto>())))
                .ForMember(dest => dest.InsuranceCompaniesPricingPlansJson, opt => opt.MapFrom(src => JsonConvert.SerializeObject(src.ShopInsuranceCompanyPlans ?? new List<IShopInsuranceCompanyPlanDto>())))
                .ForMember(dest => dest.VehicleMakesPricingPlansJson, opt => opt.MapFrom(src => JsonConvert.SerializeObject(src.ShopVehicleMakesPricingPlans ?? new List<IShopVehicleMakesPricingDto>())))
                .ForMember(dest => dest.VehicleMakesIds, opt => opt.MapFrom(src => string.Join(",", src.VehicleMakes ?? new List<int>())))
                .ForMember(dest => dest.AllowedRequestTypeIds, opt => opt.MapFrom(src => string.Join(",", src.AllowedRequestTypes ?? new List<int>())))
                .ForMember(dest => dest.DisableShopBillingNotification, opt => opt.MapFrom(src => !src.AllowShopBillingNotification))
                .ForMember(dest => dest.DisableShopStatementNotification, opt => opt.MapFrom(src => !src.AllowShopStatementNotification))
                .ForMember(dest => dest.InsuranceCompaniesIds, opt => opt.MapFrom(src => string.Join(",", src.InsuranceCompanies ?? new List<int>())));
        }
    }
}
