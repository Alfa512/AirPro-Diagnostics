using AirPro.Common.Enumerations;
using AirPro.Service.DTOs.Concrete;
using AirPro.Service.DTOs.Interface;
using AirPro.Service.Services.Abstract;
using AirPro.Service.Services.Interface;
using Dapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace AirPro.Service.Services.Concrete
{
    internal class RegistrationService : ServiceBase, IService<IRegistrationDto>
    {
        public RegistrationService(IServiceSettings settings) : base(settings)
        {
        }

        public bool Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IRegistrationDto> GetAll(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IRegistrationDto>> GetAllAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IGridPageDto<IRegistrationDto> GetAllByGridPage(ServiceArgs args = null)
        {
            var query = Conn.Query<RegistrationDto>("Access.usp_GetRegistrationGrid",
               new
               {
                   User.UserGuid,
                   Search = args?["SearchPhrase"]?.ToString(),
                   Status = args?["Status"] ?? 0
               }, null, true,null,  CommandType.StoredProcedure);

            var result = query.ToList<IRegistrationDto>().GetGridPageFromCollection(args);

            return result;
        }

        public IGridPageDto<IRegistrationDto> GetAllByGridPage(int pageNumber, int pageSize, string sort, string searchPhrase)
        {
            var args = new ServiceArgs();
            args.AddGridOptions(pageNumber, pageSize, sort, searchPhrase);

            return GetAllByGridPage(args);
        }

        public async Task<IGridPageDto<IRegistrationDto>> GetAllByGridPageAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IRegistrationDto GetById(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IRegistrationDto> GetByIdAsync(string id)
        {
            var result = await Conn.QueryAsync<RegistrationDto, RegistrationAccountDto, RegistrationShopDto, RegistrationUserDto, RegistrationDto>("Access.usp_GetRegistration @UserGuid, @RegistrationId", (link, account, shop, user) =>
            {
                link.RegistrationAccount = account;
                link.RegistrationShop = shop;
                link.RegistrationUser = user;
                return link;
            }, new { User.UserGuid, RegistrationId = Guid.Parse(id) }, splitOn: "RegistrationAccountId,RegistrationShopId,RegistrationUserId");

            return result.FirstOrDefault();
        }

        public IEnumerable<KeyValuePair<string, string>> GetDisplayList(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<KeyValuePair<string, string>>> GetDisplayListAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public string GetDisplayName(string id)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetDisplayNameAsync(string id)
        {
            throw new NotImplementedException();
        }

        public IRegistrationDto Save(IRegistrationDto update)
        {
            throw new NotImplementedException();
        }

        public async Task<IRegistrationDto> SaveAsync(IRegistrationDto update)
        {
                var param = new DynamicParameters(new
                {
                    update.RegistrationId
                    ,update.RegistrationAccount.RegistrationAccountId
                    ,update.RegistrationShop.RegistrationShopId
                    ,update.RegistrationUser.RegistrationUserId
                    ,update.RegistrationStatus
                    ,update.CallbackUrl
                    ,update.Email
                    ,update.DifferentShopInfo
                    ,update.StatusUpdateDt
                    ,update.CompletedDt
                    ,update.CreatedDt
                    ,update.UpdatedDt
                    ,update.StatusUpdateByUserGuid
                    ,update.CompletedByUserGuid
                    ,update.CreatedByUserGuid

                    ,update.RegistrationUser.FirstName
                    ,update.RegistrationUser.LastName
                    ,update.RegistrationUser.JobTitle
                    ,update.RegistrationUser.ContactNumber
                    ,update.RegistrationUser.PhoneNumber
                    ,update.RegistrationUser.PasswordHash
                    ,update.RegistrationUser.TimeZoneInfoId
                    ,AccessGroupIds = string.Join(",", update.RegistrationUser.AccessGroupIds ?? new List<Guid>())
                    ,update.RegistrationUser.ShopBillingNotification
                    ,update.RegistrationUser.ShopReportNotification

                    ,AccountAddress1 = update.RegistrationAccount.Address1
                    ,AccountAddress2 = update.RegistrationAccount.Address2
                    ,AccountCity = update.RegistrationAccount.City
                    ,AccountFax = update.RegistrationAccount.Fax
                    ,AccountName = update.RegistrationAccount.Name
                    ,AccountPhone = update.RegistrationAccount.Phone
                    ,AccountState = update.RegistrationAccount.StateId
                    ,AccountZip = update.RegistrationAccount.Zip
                    ,update.RegistrationAccount.DiscountPercentage

                    ,ShopAddress1 = update.RegistrationShop.Address1
                    ,ShopAddress2 = update.RegistrationShop.Address2
                    ,update.RegistrationShop.AdditionalScanCost
                    ,update.RegistrationShop.AllowAllRepairAutoClose
                    ,update.RegistrationShop.AllowAutoRepairClose
                    ,AllowedRequestTypeIds = string.Join(",", update.RegistrationShop.AllowedRequestTypes ?? new List<int>())
                    ,update.RegistrationShop.AllowScanAnalysisAutoClose
                    ,DisableShopBillingNotification = !update.RegistrationShop.AllowShopBillingNotification
                    ,DisableShopStatementNotification = !update.RegistrationShop.AllowShopStatementNotification
                    ,update.RegistrationShop.AutomaticRepairCloseDays
                    ,update.RegistrationShop.AverageVehiclesPerMonth
                    ,update.RegistrationShop.BillingCycleId
                    ,update.RegistrationShop.CCCShopId
                    ,ShopCity = update.RegistrationShop.City
                    ,update.RegistrationShop.CurrencyId
                    ,DefaultInsuranceCompanyId = update.RegistrationShop.DefaultInsuranceCompanyId == 0 ? null : update.RegistrationShop.DefaultInsuranceCompanyId
                    ,ShopDiscountPercentage = update.RegistrationShop.DiscountPercentage
                    ,update.RegistrationShop.EstimatePlanId
                    ,ShopFax = update.RegistrationShop.Fax
                    ,update.RegistrationShop.FirstScanCost
                    ,update.RegistrationShop.HideFromReports
                    ,InsuranceCompaniesIds = string.Join(",", update.RegistrationShop.InsuranceCompanies ?? new List<int>())
                    ,ShopName = update.RegistrationShop.Name
                    ,ShopPhone = update.RegistrationShop.Phone
                    ,update.RegistrationShop.PricingPlanId
                    ,update.RegistrationShop.SendToMitchellInd
                    ,update.RegistrationShop.ShopFixedPriceInd
                    ,InsuranceCompaniesEstimatePlansJson = JsonConvert.SerializeObject(update.RegistrationShop.ShopInsuranceCompanyEstimatePlans ?? new List<IShopInsuranceCompanyPlanDto>())
                    ,InsuranceCompaniesPricingPlansJson = JsonConvert.SerializeObject(update.RegistrationShop.ShopInsuranceCompanyPlans ?? new List<IShopInsuranceCompanyPlanDto>())
                    ,VehicleMakesPricingPlansJson = JsonConvert.SerializeObject(update.RegistrationShop.ShopVehicleMakesPricingPlans ?? new List<IShopVehicleMakesPricingDto>())
                    ,ShopState = update.RegistrationShop.StateId
                    ,VehicleMakesIds = string.Join(",", update.RegistrationShop.VehicleMakes ?? new List<int>())
                    ,ShopZip = update.RegistrationShop.Zip
                    ,User.UserGuid
                    ,update.PassedStep
                    ,update.RegistrationShop.AllowSelfScanAssessment
                });

                var registrationId = (await Conn.QueryAsync<Guid>("Access.usp_SaveRegistration", param, commandType: CommandType.StoredProcedure)).FirstOrDefault();

                if (update.RegistrationStatus == RegistrationStatus.Completed && update.ShopGuid == null && update.ClientUserGuid == null && update.AccountGuid == null)
                {
                    var result = await Conn.QueryAsync<CompleteRegistrationResultDto>("Access.usp_CompleteRegistration", param: new { RegistrationId = registrationId, UserGuid = User.UserGuid }, commandType: CommandType.StoredProcedure);
                }

                return await GetByIdAsync(registrationId.ToString());
        }
    }
}
