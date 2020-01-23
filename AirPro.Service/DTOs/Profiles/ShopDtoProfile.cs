using System.Linq;
using AirPro.Entities;
using AirPro.Entities.Access;
using AirPro.Service.DTOs.Concrete;
using AirPro.Service.DTOs.Interface;
using AutoMapper;

namespace AirPro.Service.DTOs.Profiles
{
    public class ShopDtoProfile : Profile
    {
        public ShopDtoProfile()
        {
            EntityDbContext db = new EntityDbContext();

            CreateMap<ShopEntityModel, ShopDto>()
                .ForMember(d => d.State, opt => opt.MapFrom(s => s.State.Abbreviation))
                .ForMember(d => d.ShopRequestTypes, opt => opt.MapFrom(s => s.ShopRequestTypes.Select(x => x.RequestTypeId).ToList()))
                .ForMember(d => d.Users, opt => opt.MapFrom(s => s.ShopUsers.Select(r => new UserDto
                {
                    UserGuid = r.UserGuid,
                    FirstName = r.User.FirstName,
                    LastName = r.User.LastName,
                    Email = r.User.Email
                })))
                .ForMember(d => d.ShopInsuranceCompanies, opt => opt.MapFrom(s => s.InsuranceCompanies.Select(d => d.InsuranceCompanyId)))
                .ForMember(d => d.ShopInsuranceCompanyPricingPlans, opt => opt.MapFrom(s =>
                    s.InsuranceCompaniesPricingPlans.Select(d => new ShopInsuranceCompanyPlanDto
                    {
                        InsuranceCompanyId = d.InsuranceCompanyId,
                        PlanId = d.PricingPlanId
                    })))
                .ForMember(d => d.ShopInsuranceCompanyEstimatePlans, opt => opt.MapFrom(s =>
                    s.InsuranceCompaniesEstimatePlans.Select(d => new ShopInsuranceCompanyPlanDto
                    {
                        InsuranceCompanyId = d.InsuranceCompanyId,
                        PlanId = d.EstimatePlanId
                    })))
                .ForMember(d => d.ShopVehicleMakes, opt => opt.MapFrom(s => s.VehicleMakes.Select(m => m.VehicleMakeId)))
                .ForMember(d => d.ShopVehicleMakesPricingPlans, opt => opt.MapFrom(s =>
                    s.VehicleMakesPricingPlans.Select(d => new ShopVehicleMakesPricingDto
                    {
                        VehicleMakeId = d.VehicleMakeId,
                        PricingPlanId = d.PricingPlanId
                    })))
                .ForMember(d => d.DefaultInsuranceCompanyId, opt => opt.MapFrom(s => s.DefaultInsuranceCompanyId ?? 0))
                .ForMember(d => d.AccountUsers, opt => opt.MapFrom(s => s.Account.AccountUsers.Select(r => new UserDto
                {
                    UserGuid = r.UserGuid,
                    FirstName = r.User.FirstName,
                    LastName = r.User.LastName,
                    Email = r.User.Email
                })))
                .ForMember(d => d.AirProTools,
                    opt => opt.MapFrom(s =>
                        s.AirProTools.Select(d => new AirProToolDto
                        {
                            ToolId = d.ToolId,
                            ToolName = d.Tool.ToolName
                        })))
                .ForMember(d => d.AccountAirProTools,
                    opt => opt.MapFrom(s =>
                        s.Account.AirProTools.Select(d => new AirProToolDto
                        {
                            ToolId = d.ToolId,
                            ToolName = d.Tool.ToolName
                        })))
                .ForMember(d => d.ShopContacts, opt => opt.MapFrom(s => s.ShopContacts.Select(d => new ShopContactDto
                {
                    ShopContactGuid = d.ShopContactGuid,
                    FirstName = d.FirstName,
                    LastName = d.LastName,
                    PhoneNumber = d.PhoneNumber
                })));

            CreateMap<IShopDto, ShopEntityModel>()
                .ForMember(d => d.State, opt => opt.Ignore())
                .ForMember(d => d.StateId,
                    opt => opt.MapFrom(s => db.States.FirstOrDefault(l => l.Abbreviation == s.State).StateId))
                .ForMember(d => d.ShopRequestTypes, opt => opt.Ignore())
                .ForMember(d => d.DefaultInsuranceCompanyId, opt => opt.MapFrom(d => d.DefaultInsuranceCompanyId == 0 ? (int?) null : d.DefaultInsuranceCompanyId))
                .ForMember(d => d.InsuranceCompaniesPricingPlans, opt => opt.MapFrom(s =>
                    s.ShopInsuranceCompanyPricingPlans.Select(d =>
                        new Entities.Billing.ShopInsuranceCompanyPricingEntityModel
                        {
                            InsuranceCompanyId = d.InsuranceCompanyId,
                            PricingPlanId = d.PlanId
                        })))
                .ForMember(d => d.InsuranceCompaniesEstimatePlans, opt => opt.MapFrom(s =>
                    s.ShopInsuranceCompanyEstimatePlans.Select(d =>
                        new Entities.Billing.ShopInsuranceCompanyEstimateEntityModel()
                        {
                            InsuranceCompanyId = d.InsuranceCompanyId,
                            EstimatePlanId = d.PlanId
                        })))
                .ForMember(d => d.VehicleMakesPricingPlans, opt => opt.MapFrom(s =>
                    s.ShopVehicleMakesPricingPlans.Select(d =>
                        new Entities.Billing.ShopVehicleMakesPricingEntityModel
                        {
                            VehicleMakeId = d.VehicleMakeId,
                            PricingPlanId = d.PricingPlanId
                        })))
                .ForMember(d => d.InsuranceCompanies, opt => opt.MapFrom(s =>
                    s.ShopInsuranceCompanies.Select(d => new ShopInsuranceCompanyEntityModel
                    {
                        InsuranceCompanyId = d
                    })))
                .ForMember(d => d.VehicleMakes, opt => opt.MapFrom(s =>
                    s.ShopVehicleMakes.Select(d => new ShopVehicleMakeEntityModel
                    {
                        VehicleMakeId = d
                    })))
                .ForMember(d => d.ShopContacts, opt => opt.MapFrom(s => s.ShopContacts.Select(d => new ShopContactEntityModel
                {
                    FirstName = d.FirstName,
                    LastName = d.LastName,
                    PhoneNumber = d.PhoneNumber
                })));
        }
    }
}