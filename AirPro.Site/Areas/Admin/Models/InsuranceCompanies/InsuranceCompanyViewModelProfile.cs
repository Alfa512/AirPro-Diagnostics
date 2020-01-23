using AirPro.Service.DTOs.Interface;
using AutoMapper;

namespace AirPro.Site.Areas.Admin.Models.InsuranceCompanies
{
    public class InsuranceCompanyViewModelProfile : Profile
    {
        public InsuranceCompanyViewModelProfile()
        {
            CreateMap<IInsuranceCompanyDto, InsuranceCompanyViewModel>();
        }
    }
}