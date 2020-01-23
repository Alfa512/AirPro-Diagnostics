using AirPro.Common.Enumerations;
using AirPro.Entities.Access;
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
using System.Linq.Dynamic;
using System.Threading.Tasks;
using AirPro.Logging;

namespace AirPro.Service.Services.Concrete
{
    internal class ShopService : ServiceBase, IService<IShopDto>
    {

        internal readonly IQueryable<ShopEntityModel> AllowedShops;

        public ShopService(IServiceSettings settings) : base(settings)
        {
            // Populate Allowed Shops.
            AllowedShops = UserHasRoles(ApplicationRoles.AccountShowAll, ApplicationRoles.ShopShowAll)
                ? Db.Shops
                : Db.UserShops.Where(us => us.UserGuid == User.UserGuid).Select(us => us.Shop)
                    .Union(Db.UserAccounts.Where(ua => ua.UserGuid == User.UserGuid)
                        .SelectMany(ua => ua.Account.Shops));
        }

        public IShopDto GetById(string id)
        {
            if (!UserHasRoles(ApplicationRoles.ShopView, ApplicationRoles.ShopEdit, ApplicationRoles.ShopCreate))
            {
                return null;
            }

            Guid shopGuid = Guid.Parse(id);

            ShopDto result = GetShops(shopGuid)?.FirstOrDefault();

            // Return.
            return result;
        }

        private List<ShopDto> GetShops(Guid? shopGuid = null, string search = null, string shopName = null, Guid? notShopGuid = null)
        {
            // Load Shop.
            List<ShopDto> result = new List<ShopDto>();
            using (var x = Conn.QueryMultiple("Access.usp_GetShops",
                new { User.UserGuid, ShopGuid = shopGuid, Search = search, ShopName = shopName, NotShopGuid = notShopGuid }, null, null, CommandType.StoredProcedure))
            {
                var shops = x.Read<ShopDto>();
                var users = x.Read<ShopUserDto>();
                var accountUsers = x.Read<ShopUserDto>();
                var airProTools = x.Read<ShopAirProToolDto>();
                var accountAirProTools = x.Read<ShopAirProToolDto>();
                var shopContacts = x.Read<ShopContactDto>();
                var shopVehicleMakes = x.Read<ShopGuidIdDto>();
                var shopInsuranceCompanies = x.Read<ShopGuidIdDto>();
                var shopInsuranceCompanyPricingPlans = x.Read<ShopInsuranceCompanyPlanDto>();
                var shopInsuranceCompanyEstimatePlans = x.Read<ShopInsuranceCompanyPlanDto>();
                var shopVehicleMakesPricingPlans = x.Read<ShopVehicleMakesPricingDto>();
                var shopRequestTypes = x.Read<ShopGuidIdDto>();

                foreach (var shop in shops)
                {
                    shop.Users = users.Where(y => y.ShopGuid == shop.ShopGuid).ToList<IUserDto>();
                    shop.AccountUsers = accountUsers.Where(y => y.ShopGuid == shop.ShopGuid).ToList<IUserDto>();
                    shop.AirProTools = airProTools.Where(y => y.ShopGuid == shop.ShopGuid).ToList<IAirProToolDto>();
                    shop.AccountAirProTools = accountAirProTools.Where(y => y.ShopGuid == shop.ShopGuid).ToList<IAirProToolDto>();
                    shop.ShopContacts = shopContacts.Where(y => y.ShopGuid == shop.ShopGuid).ToList<IShopContactDto>();
                    shop.ShopVehicleMakes = shopVehicleMakes.Where(y => y.ShopGuid == shop.ShopGuid).Select(y => y.Id).ToList();
                    shop.ShopInsuranceCompanies = shopInsuranceCompanies.Where(y => y.ShopGuid == shop.ShopGuid).Select(y => y.Id).ToList();
                    shop.ShopInsuranceCompanyPricingPlans = shopInsuranceCompanyPricingPlans.Where(y => y.ShopGuid == shop.ShopGuid).ToList<IShopInsuranceCompanyPlanDto>();
                    shop.ShopInsuranceCompanyEstimatePlans = shopInsuranceCompanyEstimatePlans.Where(y => y.ShopGuid == shop.ShopGuid).ToList<IShopInsuranceCompanyPlanDto>();
                    shop.ShopVehicleMakesPricingPlans = shopVehicleMakesPricingPlans.Where(y => y.ShopGuid == shop.ShopGuid).ToList<IShopVehicleMakesPricingDto>();
                    shop.ShopRequestTypes = shopRequestTypes.Where(rt => rt.ShopGuid == shop.ShopGuid).Select(rt => rt.Id).ToList();
                    result.Add(shop);
                }
            }

            return result;
        }

        public Task<IShopDto> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public string GetDisplayName(string id)
        {
            // Test Guid.
            Guid shopGuid;
            if (!Guid.TryParse(id, out shopGuid)) return "Invalid ID";

            return Db.Shops?.FirstOrDefault(s => s.ShopGuid == shopGuid)?.DisplayName ?? "Unknown";
        }

        public Task<string> GetDisplayNameAsync(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<KeyValuePair<string, string>> GetDisplayList(ServiceArgs args = null)
        {
            return Task.Run(async () => await GetDisplayListAsync(args)).Result;
        }

        public async Task<IEnumerable<KeyValuePair<string, string>>> GetDisplayListAsync(ServiceArgs args = null)
        {
            // Load Arguments.
            bool? selfScan = null;
            if (args != null && args.ContainsKey("AllowSelfScan") && bool.TryParse(args["AllowSelfScan"].ToString(), out var self))
                selfScan = self;

            bool? selfAnalysis = null;
            if (args != null && args.ContainsKey("AllowScanAnalysis") && bool.TryParse(args["AllowScanAnalysis"].ToString(), out var analysis))
                selfAnalysis = analysis;

            bool? hideFromReports = null;
            if (args != null && args.ContainsKey("HideFromReports") && bool.TryParse(args["HideFromReports"].ToString(), out var hide))
                hideFromReports = hide;

            // Load Parameter.
            var param = new
            {
                User.UserGuid,
                AllowSelfScan = selfScan,
                AllowScanAnalysis = selfAnalysis,
                HideFromReports = hideFromReports
            };

            // Return Display List.
            return (await Conn.QueryAsync(sql: "Access.usp_GetShopDisplayList", param: param, commandType: CommandType.StoredProcedure))
                .Select(s => new KeyValuePair<string, string>(s.ShopGuid.ToString(), s.ShopName)).ToList();
        }

        public IEnumerable<IShopDto> GetAll(ServiceArgs args = null)
        {
            var result = new List<IShopDto>();

            if (!UserHasRoles(ApplicationRoles.ShopView, ApplicationRoles.ShopEdit)) return result;

            if (AllowedShops != null)
            {
                string shopName = null;
                Guid? notShopGuid = null;
                if (args != null && args.ContainsKey("ShopName") && args["ShopName"] != null)
                {
                    shopName = args["ShopName"].ToString();
                }

                if (args != null && args.ContainsKey("NotShopGuid") && Guid.TryParse(args["NotShopGuid"].ToString(), out var nsg))
                {
                    notShopGuid = nsg;
                }

                result.AddRange(GetShops(shopName: shopName, notShopGuid: notShopGuid));
            }

            return result;
        }

        public Task<IEnumerable<IShopDto>> GetAllAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IGridPageDto<IShopDto> GetAllByGridPage(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public Task<IGridPageDto<IShopDto>> GetAllByGridPageAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IGridPageDto<IShopDto> GetAllByGridPage(int pageNumber, int pageSize, string sort, string searchPhrase)
        {
            // Create Result.
            var result = new GridPageDto<IShopDto>
            {
                Current = pageNumber,
                Rows = new List<IShopDto>()
            };

            if (!UserHasRoles(ApplicationRoles.ShopView, ApplicationRoles.ShopEdit)) return result;

            // Search Shops.
            var shops = GetShops(search: searchPhrase);

            // Count Results.
            result.Total = shops.Count();

            // Sort Dataset.
            var sorted = string.IsNullOrEmpty(sort) ? shops?.OrderBy(u => u.Name) : shops?.OrderBy(sort);

            // Get Page.
            var page = pageSize < 0 ? sorted : sorted.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            // Set Page.
            result.RowCount = page.Count();
            result.Rows = page;


            return result;
        }

        public IShopDto Save(IShopDto shop)
        {
            try
            {
                IShopDto update;
                UpdateResultDto result = new UpdateResultDto(false, "Unknown Error Occured.");

                // Verify CCC Shop Id.
                if (!string.IsNullOrEmpty(shop.CCCShopId))
                {
                    if (Db.Shops?.Any(s => s.CCCShopId == shop.CCCShopId && (shop.ShopGuid == Guid.Empty || shop.ShopGuid != s.ShopGuid)) ?? false)
                    {
                        // Duplicate CCC Shop Id.
                        shop.UpdateResult = new UpdateResultDto(false, "CCC Shop ID already Exists for another Shop.");
                        return shop;
                    }
                }

                // Limit Discount Percentage.
                if (shop.DiscountPercentage > 100) shop.DiscountPercentage = 100;

                // Check for New Shop.
                if (shop.ShopGuid == Guid.Empty)
                {
                    // Verify Access for Add.
                    var accountService = new AccountService(Settings);
                    if (!UserHasRoles(ApplicationRoles.ShopCreate) || !accountService.AllowedAccounts.Any(a => a.AccountGuid == shop.AccountGuid))
                    {
                        // No Ability to Add to Account.
                        shop.UpdateResult = new UpdateResultDto(false, "You do not have access to create a record.");
                        return shop;
                    }

                    // Set Update.
                    update = shop;

                    // Set Result.
                    result = new UpdateResultDto(true, "Shop Created Successfully.");
                }
                else
                {
                    // Verify Access to Edit.
                    if (!UserHasRoles(ApplicationRoles.ShopEdit) || (!AllowedShops?.Any(s => s.ShopGuid == shop.ShopGuid) ?? true))
                    {
                        // No Ability to Edit.
                        shop.UpdateResult = new UpdateResultDto(false, "You do not have access to modify this record.");
                        return shop;
                    }

                    // Load Shop for Update.
                    update = GetById(shop.ShopGuid.ToString());

                    // Check Shop.
                    if (update != null)
                    {
                        var activeIndCache = update.ActiveInd;
                        var estimatePlanIdCache = update.EstimatePlanId;
                        var discountPercentageCache = update.DiscountPercentage;
                        var pricingPlanIdCache = update.PricingPlanId;

                        // Set Update.
                        update = shop;

                        // Check Active.
                        if (!shop.ActiveInd) update.ActiveInd = activeIndCache;

                        // Check Estimate Plan Permission.
                        if (!UserHasRoles(ApplicationRoles.EstimatePlanView))
                        {
                            update.EstimatePlanId = estimatePlanIdCache;
                        }

                        // Check Payment Create Permission.
                        if (!UserHasRoles(ApplicationRoles.PaymentCreate))
                        {
                            update.DiscountPercentage = discountPercentageCache;
                            update.PricingPlanId = pricingPlanIdCache;
                        }

                        // Set Result.
                        result = new UpdateResultDto(true, "Shop Updated Successfully.");
                    }
                }

                // Load Insurance Company Pricing Plans.
                var insuranceCompaniesPricingPlans = new DataTable();
                insuranceCompaniesPricingPlans.Columns.Add("ShopGuid", typeof(Guid));
                insuranceCompaniesPricingPlans.Columns.Add("InsuranceCompanyId", typeof(int));
                insuranceCompaniesPricingPlans.Columns.Add("PricingPlanId", typeof(int));

                foreach (var item in update?.ShopInsuranceCompanyPricingPlans ?? new List<IShopInsuranceCompanyPlanDto>())
                {
                    insuranceCompaniesPricingPlans.Rows.Add(item.ShopGuid, item.InsuranceCompanyId, item.PlanId);
                }

                // Load Insurance Company Estimate Plans.
                var insuranceCompaniesEstimatePlans = new DataTable();
                insuranceCompaniesEstimatePlans.Columns.Add("ShopGuid", typeof(Guid));
                insuranceCompaniesEstimatePlans.Columns.Add("InsuranceCompanyId", typeof(int));
                insuranceCompaniesEstimatePlans.Columns.Add("EstimatePlanId", typeof(int));

                foreach (var item in update?.ShopInsuranceCompanyEstimatePlans ?? new List<IShopInsuranceCompanyPlanDto>())
                {
                    insuranceCompaniesEstimatePlans.Rows.Add(item.ShopGuid, item.InsuranceCompanyId, item.PlanId);
                }

                // Load Vehicle Make Pricing Plans.
                var vehicleMakesPricingPlans = new DataTable();
                vehicleMakesPricingPlans.Columns.Add("ShopGuid", typeof(Guid));
                vehicleMakesPricingPlans.Columns.Add("VehicleMakeId", typeof(int));
                vehicleMakesPricingPlans.Columns.Add("PricingPlanId", typeof(int));

                foreach (var item in update?.ShopVehicleMakesPricingPlans ?? new List<IShopVehicleMakesPricingDto>())
                {
                    vehicleMakesPricingPlans.Rows.Add(item.ShopGuid, item.VehicleMakeId, item.PricingPlanId);
                }

                // Load Shop Contacts.
                var shopContacts = new DataTable();
                shopContacts.Columns.Add("ShopContactGuid", typeof(Guid));
                shopContacts.Columns.Add("ShopGuid", typeof(Guid));
                shopContacts.Columns.Add("FirstName", typeof(string));
                shopContacts.Columns.Add("LastName", typeof(string));
                shopContacts.Columns.Add("PhoneNumber", typeof(string));

                foreach (var item in update?.ShopContacts.Where(x =>
                    !(string.IsNullOrWhiteSpace(x.FirstName) && string.IsNullOrWhiteSpace(x.LastName) &&
                      string.IsNullOrWhiteSpace(x.PhoneNumber))).ToList() ?? new List<IShopContactDto>())
                {
                    shopContacts.Rows.Add(item.ShopContactGuid, item.ShopGuid, item.FirstName, item.LastName, item.PhoneNumber);
                }

                // Create Parameter.
                var param = new DynamicParameters(new
                {
                    update.Name,
                    update.AccountGuid,
                    update.Address1,
                    update.Address2,
                    update.City,
                    update.State,
                    update.Zip,
                    update.Phone,
                    update.Fax,
                    update.Notes,
                    update.CCCShopId,
                    update.AverageVehiclesPerMonth,
                    update.AllowAutoRepairClose,
                    update.AllowScanAnalysis,
                    update.AllowDemoScan,
                    update.DefaultInsuranceCompanyId,
                    update.AllowSelfScanAssessment,
                    update.PricingPlanId,
                    update.ShopFixedPriceInd,
                    update.FirstScanCost,
                    update.AdditionalScanCost,
                    update.AutomaticRepairCloseDays,
                    update.HideFromReports,
                    update.CurrencyId,
                    update.BillingCycleId,
                    update.AllowScanAnalysisAutoClose,
                    update.SendToMitchellInd,
                    update.AllowAllRepairAutoClose,
                    update.DisableShopStatementNotification,
                    update.DisableShopBillingNotification,
                    update.ActiveInd,
                    update.EstimatePlanId,
                    update.DiscountPercentage,
                    update.AllowSelfScan,
                    User.UserGuid,
                    ShopContacts = shopContacts,
                    ShopInsurancePlansPricingPlans = insuranceCompaniesPricingPlans.AsTableValuedParameter(),
                    ShopInsurancePlansEstimatePlans = insuranceCompaniesEstimatePlans,
                    ShopVehicleMakesPricing = vehicleMakesPricingPlans,
                    InsuranceCompanyIds = JsonConvert.SerializeObject(update.ShopInsuranceCompanies?.Select(t => t).ToArray() ?? new int[] {}),
                    VehicleMakeIds = JsonConvert.SerializeObject(update.ShopVehicleMakes?.Select(r => r).ToArray() ?? new int[] { }),
                    RequestTypeIds = JsonConvert.SerializeObject(update.ShopRequestTypes?.Select(t => t).ToArray() ?? new int[] { }),
                    update.EmployeeGuid,
                    update.AutomaticInvoicesInd
                });

                // Add Output Parameter.
                param.Add(nameof(update.ShopGuid), update.ShopGuid, DbType.Guid, ParameterDirection.InputOutput);

                // Execute Update.
                Conn.Execute("Access.usp_SaveShop", param, commandType: CommandType.StoredProcedure);
                var shopGuid = param.Get<Guid>(nameof(update.ShopGuid));

                // Load Shop.
                shop = GetById(shopGuid.ToString());

                // Set Update Result.
                shop.UpdateResult = result;
            }
            catch (Exception e)
            {
                // Log Exception.
                Logger.LogException(e);

                // Set Update Result.
                shop.UpdateResult = new UpdateResultDto(false, e.Message);
            }

            // Done.
            return shop;
        }

        public Task<IShopDto> SaveAsync(IShopDto update)
        {
            throw new NotImplementedException();
        }

        public bool Delete(string id)
        {
            var result = false;

            try
            {
                if (Guid.TryParse(id, out var shopGuid))
                {
                    var param = new
                    {
                        ShopGuid = shopGuid,
                        UserGuid = User.UserGuid
                    };

                    Conn.Execute("Access.usp_DeleteShop", param, null, null, CommandType.StoredProcedure);
                }

                result = true;
            }
            catch (Exception e)
            {
                Logger.LogException(e);
            }

            return result;
        }

        public Task<bool> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}
