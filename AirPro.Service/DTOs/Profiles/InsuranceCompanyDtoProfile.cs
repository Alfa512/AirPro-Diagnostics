using AirPro.Entities.Repair;
using AirPro.Service.DTOs.Concrete;
using AirPro.Service.DTOs.Interface;
using AutoMapper;

namespace AirPro.Service.DTOs.Profiles
{
    public class InsuranceCompanyDtoProfile : Profile
    {
        public InsuranceCompanyDtoProfile()
        {
            CreateMap<IInsuranceCompanyDto, InsuranceCompanyDto>();
            CreateMap<InsuranceCompanyEntityModel, IInsuranceCompanyDto>().ReverseMap();

            CreateMap<InsuranceCompanyEntityModel, InsuranceCompanyDto>();

            CreateMap<IGroupDto, InsuranceCompanyEntityModel>();
        }
    }
}