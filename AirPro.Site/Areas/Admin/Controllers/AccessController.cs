using AirPro.Common.Enumerations;
using AirPro.Entities.Access;
using AirPro.Service;
using AirPro.Service.DTOs.Interface;
using AirPro.Site.Areas.Admin.Models.Access;
using AirPro.Site.Attributes;
using AirPro.Site.Results;
using AutoMapper;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using UniMatrix.Common.Extensions;

namespace AirPro.Site.Areas.Admin.Controllers
{
    [AuthorizeRoles(ApplicationRoles.AccountCreate, ApplicationRoles.AccountDelete, ApplicationRoles.AccountEdit, ApplicationRoles.AccountView,
        ApplicationRoles.ShopCreate, ApplicationRoles.ShopDelete, ApplicationRoles.ShopEdit, ApplicationRoles.ShopView,
        ApplicationRoles.GroupCreate, ApplicationRoles.GroupDelete, ApplicationRoles.GroupEdit, ApplicationRoles.GroupView,
        ApplicationRoles.UserCreate, ApplicationRoles.UserDelete, ApplicationRoles.UserEdit, ApplicationRoles.UserView)]
    public class AccessController : BaseController
    {
        private ServiceFactory _factory;
        private ServiceFactory Factory => _factory ?? (_factory = new ServiceFactory(MvcApplication.ConnectionString, User.Identity));

        // GET: Admin/Access
        public async Task<ActionResult> Index()
        {
            var model = new AccessViewModel()
            {
                DisplayAccountTab = User.IsInRole(ApplicationRoles.AccountCreate.ToString())
                           || User.IsInRole(ApplicationRoles.AccountDelete.ToString())
                           || User.IsInRole(ApplicationRoles.AccountEdit.ToString())
                           || User.IsInRole(ApplicationRoles.AccountView.ToString()),

                DisplayShopTab = User.IsInRole(ApplicationRoles.ShopCreate.ToString())
                           || User.IsInRole(ApplicationRoles.ShopDelete.ToString())
                           || User.IsInRole(ApplicationRoles.ShopEdit.ToString())
                           || User.IsInRole(ApplicationRoles.ShopView.ToString()),

                DisplayGroupTab = User.IsInRole(ApplicationRoles.GroupCreate.ToString())
                           || User.IsInRole(ApplicationRoles.GroupDelete.ToString())
                           || User.IsInRole(ApplicationRoles.GroupEdit.ToString())
                           || User.IsInRole(ApplicationRoles.GroupView.ToString()),

                DisplayUserTab = User.IsInRole(ApplicationRoles.UserCreate.ToString())
                           || User.IsInRole(ApplicationRoles.UserDelete.ToString())
                           || User.IsInRole(ApplicationRoles.UserEdit.ToString())
                           || User.IsInRole(ApplicationRoles.UserView.ToString())

            };

            return View(model);
        }

        #region Accounts

        [HttpPost]
        [AuthorizeRoles(ApplicationRoles.AccountView, ApplicationRoles.AccountEdit)]
        public ActionResult GetAccountsByPage(int current, int rowCount, string searchPhrase)
        {
            var result = JsonConvert.SerializeObject(
                Factory.GetAllByGridPage<IAccountDto>(current, rowCount, Request.Form.GetDynamicSortString(), searchPhrase), Formatting.Indented,
                new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });

            return Content(result, "application/json");
        }

        [HttpGet]
        [AuthorizeRoles(ApplicationRoles.AccountCreate, ApplicationRoles.AccountEdit, ApplicationRoles.AccountView)]
        public async Task<ActionResult> AccountManage(Guid? id)
        {
            // Load State Selection.
            ViewBag.StateSelection = GetStateSelectList();
            ViewBag.EmployeeSelection = await Factory.GetDisplayListAsync<IEmployeeDto>();

            if (!id.HasValue) return PartialView("_AccountManage", new AccountViewModel { AllowEntry = User.IsInRole(ApplicationRoles.AccountCreate.ToString()) });

            var account = Mapper.Map<AccountViewModel>(Factory.GetById<IAccountDto>(id.Value.ToString()));
            account.AllowEntry = User.IsInRole(ApplicationRoles.AccountEdit.ToString());

            return PartialView("_AccountManage", account);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(ApplicationRoles.AccountCreate, ApplicationRoles.AccountEdit)]
        public async Task<ActionResult> AccountManage(AccountViewModel account)
        {
            // Load State Selection.
            ViewBag.StateSelection = GetStateSelectList();
            ViewBag.EmployeeSelection = await Factory.GetDisplayListAsync<IEmployeeDto>();

            if (!ModelState.IsValid)
            {
                account.AllowEntry = User.IsInRole(ApplicationRoles.AccountCreate.ToString()) ||
                                     User.IsInRole(ApplicationRoles.AccountEdit.ToString());

                return PartialView("_AccountManage", account);
            }

            var update = Mapper.Map<AccountViewModel>(Factory.Save(account as IAccountDto));
            update.AllowEntry = User.IsInRole(ApplicationRoles.AccountEdit.ToString());

            ModelState.Clear();

            return PartialView("_AccountManage", update);
        }

        [HttpPost]
        [AuthorizeRoles(ApplicationRoles.AccountDelete)]
        public ActionResult AccountDelete(Guid? id)
        {
            var result = false;

            if (id.HasValue)
            {
                result = Factory.Delete<IAccountDto>(id.ToString());
            }

            return new JsonCamelCaseResult(new { Success = result }, JsonRequestBehavior.DenyGet);
        }

        private IEnumerable<KeyValuePair<string,string>> GetAccountSelectList(Guid? currentAccountGuid)
        {
            // Load Account Selection List.
            var accountList = Factory.GetDisplayList<IAccountDto>().ToList();
            accountList.Insert(0, new KeyValuePair<string, string>("", @"<-- Select Account -->"));

            // Check for Current Account.
            if (currentAccountGuid.HasValue && accountList.All(a => a.Key != currentAccountGuid.Value.ToString()))
            {
                // Add Current Account as Option.
                accountList.Add(new KeyValuePair<string, string>(currentAccountGuid.Value.ToString(), Factory.GetDisplayName<IAccountDto>(currentAccountGuid.Value.ToString())));
            }

            return accountList.OrderBy(a => a.Value);
        }

        #endregion

        #region Shops

        [HttpPost]
        [AuthorizeRoles(ApplicationRoles.ShopView, ApplicationRoles.ShopEdit)]
        public ActionResult GetShopsByPage(int current, int rowCount, string searchPhrase)
        {
            var result = JsonConvert.SerializeObject(
                Factory.GetAllByGridPage<IShopDto>(current, rowCount, Request.Form.GetDynamicSortString(), searchPhrase), Formatting.Indented,
                new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });

            return Content(result, "application/json");
        }

        [HttpGet]
        [AuthorizeRoles(ApplicationRoles.ShopCreate, ApplicationRoles.ShopEdit, ApplicationRoles.ShopView)]
        public async Task<ActionResult> ShopManage(Guid? id)
        {
            // Set Default.
            var shop = new ShopViewModel { AllowEntry = User.IsInRole(ApplicationRoles.ShopCreate.ToString()) };

            // Load Shop.
            if (id.HasValue)
            {
                var shopDto = Factory.GetById<IShopDto>(id.Value.ToString());
                shop = Mapper.Map<ShopViewModel>(shopDto);
                shop.AllowEntry = User.IsInRole(ApplicationRoles.ShopEdit.ToString());
            }
            else
            {
                // Default Pricing Plan.
                shop.PricingPlanId = 1;
            }

            // Load Selection Lists.
            await LoadShopSelectLists(shop.AccountGuid);

            // Return Partial.
            return PartialView("_ShopManage", shop);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(ApplicationRoles.ShopCreate, ApplicationRoles.ShopEdit)]
        public async Task<ActionResult> ShopManage(ShopViewModel shop)
        {
            // Load Selection Lists.
            await LoadShopSelectLists(shop.AccountGuid);

            if (!ModelState.IsValid)
            {
                // Set Allow Entry.
                shop.AllowEntry = User.IsInRole(ApplicationRoles.ShopCreate.ToString()) || User.IsInRole(ApplicationRoles.ShopEdit.ToString());

                return PartialView("_ShopManage", shop);
            }

            var shopDto = Mapper.Map<IShopDto>(shop);
            var update = Mapper.Map<ShopViewModel>(Factory.Save(shopDto));

            Session["SelfScanListItems"] = null;
            Session["ScanAnalysisListItems"] = null;

            update.AllowEntry = User.IsInRole(ApplicationRoles.ShopEdit.ToString());

            ModelState.Clear();
            SelectListItemCache.ClearCache(SelectListItemCacheType.RequestType);

            return PartialView("_ShopManage", update);
        }

        [HttpPost]
        [AuthorizeRoles(ApplicationRoles.ShopCreate, ApplicationRoles.ShopEdit)]
        public JsonResult IsShopNameExist(string Name, Guid? ShopGuid)
        {
            var shops = Factory.GetAll<IShopDto>(new ServiceArgs { { "ShopName", Name }, { "Light", true }, { "NotShopGuid", ShopGuid } });
            return Json(shops.Count() == 0);

        }

        [HttpPost]
        [AuthorizeRoles(ApplicationRoles.AccountCreate, ApplicationRoles.AccountEdit)]
        public JsonResult IsAccountNameExist(string Name, Guid? AccountGuid)
        {
            var shops = Factory.GetAll<IAccountDto>(new ServiceArgs { { "AccountName", Name }, { "Light", true }, { "NotAccountGuid", AccountGuid } });
            return Json(shops.Count() == 0);

        }

        [HttpPost]
        [AuthorizeRoles(ApplicationRoles.ShopDelete)]
        public ActionResult ShopDelete(Guid? id)
        {
            var result = false;

            if (id.HasValue)
            {
                result = Factory.Delete<IShopDto>(id.ToString());
            }

            return new JsonCamelCaseResult(new { Success = result }, JsonRequestBehavior.DenyGet);
        }

        private async Task LoadShopSelectLists(Guid? accountGuid)
        {
            ViewBag.EmployeeSelection = await Factory.GetDisplayListAsync<IEmployeeDto>();
            ViewBag.StateSelection = GetStateSelectList().OrderBy(m => m.Value);
            ViewBag.AccountSelection = GetAccountSelectList(accountGuid).OrderBy(m => m.Value);
            ViewBag.PricingPlanSelection = Factory.GetDisplayList<IPricingPlanDto>().OrderBy(m => m.Value);
            ViewBag.EstimatePlanSelection = Factory.GetDisplayList<IEstimatePlanDto>().OrderBy(m => m.Value);
            ViewBag.InsuranceCompaniesSelection = Factory.GetDisplayList<IInsuranceCompanyDto>().OrderBy(m => m.Value);
            ViewBag.BillingCycles = GetBillingCycleSelectList();
            ViewBag.RequestTypes = await Factory.GetDisplayListAsync<IRequestTypeDto>();

            ViewBag.InsuranceCompaniesDrpSelection = Factory.GetAll<IInsuranceCompanyDto>()?
                .Where(d => !d.ProgramName.IsNullOrWhiteSpace())
                .OrderBy(p => p.ProgramName)
                .Select(p => new KeyValuePair<string, string>(p.InsuranceCompanyId.ToString(), p.ProgramName))
                .ToList();

            var allVehicleMakes = Factory.GetAll<IVehicleMakeDto>().ToList();

            ViewBag.VehicleMakes = allVehicleMakes
                .Where(m => !string.IsNullOrEmpty(m.ProgramName))
                .Select(d => new SelectListItem
                {
                    Value = d.VehicleMakeId.ToString(),
                    Text = d.ProgramName
                }).OrderBy(m => m.Text).ToList();

            ViewBag.AllVehicleMakes = allVehicleMakes
                .Select(d => new SelectListItem
                {
                    Value = d.VehicleMakeId.ToString(),
                    Text = d.VehicleMakeName
                }).OrderBy(m => m.Text).ToList();

            
            ViewBag.DefaultInsuranceCompanies = GetInsuranceCompanies();

            ViewBag.Currencies = SelectListItemCache.BillingCurrencySelectItems();
        }

        #endregion

        #region Groups

        [HttpPost]
        [AuthorizeRoles(ApplicationRoles.GroupView, ApplicationRoles.GroupEdit)]
        public ActionResult GetGroupsByPage(int current, int rowCount, string searchPhrase)
        {
            var result = JsonConvert.SerializeObject(
                Factory.GetAllByGridPage<IGroupDto>(current, rowCount, Request.Form.GetDynamicSortString(), searchPhrase), Formatting.Indented,
                new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });

            return Content(result, "application/json");
        }

        [HttpGet]
        [AuthorizeRoles(ApplicationRoles.GroupCreate, ApplicationRoles.GroupEdit, ApplicationRoles.GroupView)]
        public ActionResult GroupManage(Guid? id)
        {
            if (!id.HasValue) return PartialView("_GroupManage", new GroupViewModel() { AllowEntry = User.IsInRole(ApplicationRoles.GroupCreate.ToString())} );

            var model = Mapper.Map<GroupViewModel>(Factory.GetById<IGroupDto>(id.Value.ToString()));
            model.AllowEntry = User.IsInRole(ApplicationRoles.GroupEdit.ToString());

            return PartialView("_GroupManage", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(ApplicationRoles.GroupCreate, ApplicationRoles.GroupEdit)]
        public ActionResult GroupManage(GroupViewModel group)
        {
            if (!ModelState.IsValid)
            {
                // Set Allow Entry.
                group.AllowEntry = User.IsInRole(ApplicationRoles.GroupCreate.ToString()) || User.IsInRole(ApplicationRoles.GroupEdit.ToString());

                return PartialView("_GroupManage", group);
            }

            // Update Group Roles.
            group.Roles = group.RoleGuids.Select(r => new KeyValuePair<Guid, string>(r, string.Empty)).ToList();

            // Perform Update.
            var update = Mapper.Map<GroupViewModel>(Factory.Save((IGroupDto) group));
            update.AllowEntry = User.IsInRole(ApplicationRoles.GroupEdit.ToString());

            ModelState.Clear();

            // Check for Group Membership.
            RefreshMyRoles();

            return PartialView("_GroupManage", update);
        }

        #endregion

        #region Users

        [HttpPost]
        [AuthorizeRoles(ApplicationRoles.UserView, ApplicationRoles.UserEdit)]
        public ActionResult GetUsersByPage(int current, int rowCount, string searchPhrase)
        {
            var result = JsonConvert.SerializeObject(
                Factory.GetAllByGridPage<IUserDto>(current, rowCount, Request.Form.GetDynamicSortString(), searchPhrase), Formatting.Indented,
                new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });

            return Content(result, "application/json");
        }

        [HttpGet]
        [AuthorizeRoles(ApplicationRoles.UserCreate, ApplicationRoles.UserEdit, ApplicationRoles.UserView)]
        public ActionResult UserManage(Guid? id)
        {
            // Set Default.
            var view = new UserViewModel { AllowEntry = User.IsInRole(ApplicationRoles.UserCreate.ToString()) };

            // Load User.
            if (id.HasValue)
            {
                view = Mapper.Map<UserViewModel>(Factory.GetById<IUserDto>(id.Value.ToString()));
                view.AllowEntry = User.IsInRole(ApplicationRoles.UserEdit.ToString());
            }
            GetUserManageSelections(ref view);

            ViewBag.GroupsSelectList = GetTypeSelection(view.AvailableGroups, view.GroupMemberships);

            return PartialView("_UserManage", view);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(ApplicationRoles.UserCreate, ApplicationRoles.UserEdit)]
        public async Task<ActionResult> UserManage(UserViewModel user)
        {
            if (!ModelState.IsValid)
            {
                GetUserManageSelections(ref user);

                user.AllowEntry = User.IsInRole(ApplicationRoles.UserCreate.ToString()) || User.IsInRole(ApplicationRoles.UserEdit.ToString());

                ViewBag.GroupsSelectList = GetTypeSelection(user.AvailableGroups, user.GroupMemberships);

                return PartialView("_UserManage", user);
            }

            if (!string.IsNullOrEmpty(user.Password))
                user.PasswordHash = (new PasswordHasher()).HashPassword(user.Password);

            var update = Mapper.Map<UserViewModel>(Factory.Save((IUserDto) user));
            update.AllowEntry = User.IsInRole(ApplicationRoles.UserEdit.ToString());

            if (user.UserGuid == Guid.Empty && update.UserGuid != Guid.Empty)
            {
                var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

                var code = await userManager.GenerateEmailConfirmationTokenAsync(update.UserGuid);
                var callbackUrl = Url.Action("ConfirmEmail", "Account", new { Area = "", userId = update.UserGuid, code }, protocol: Request.Url?.Scheme);
                await userManager.SendEmailAsync(update.UserGuid, "AirPro Diagnostics: Confirm Your Account", $"Please use the following link to confirm your AirPro Diagnostics Account.<br/><br/><a href=\"{ callbackUrl }\">Confirm Account</a>");
            }

            ModelState.Clear();

            GetUserManageSelections(ref update);

            // Check for Current User.
            if (user.UserGuid == Guid.Parse(User.Identity.GetUserId()))
            {
                RefreshMyRoles();
            }

            return PartialView("_UserManage", update);
        }

        [HttpDelete]
        [ActionName("User")]
        [AuthorizeRoles(ApplicationRoles.UserDelete)]
        public ActionResult UserLock(Guid userGuid)
        {
            string message = "";
            bool result = false;
            try
            {
                result = Factory.Delete<IUserDto>(userGuid.ToString());
            }
            catch (Exception e)
            {
                message = e.Message;
            }

            return Json(new
            {
                success = result,
                message = message
            });
        }

        [HttpPost]
        [AuthorizeRoles(ApplicationRoles.UserEdit)]
        public ActionResult DeleteShopUserAssociation(string userGuid, string shopGuid)
        {
            bool result = false;

            var currentUserId = User.Identity.GetUserId();
            if (!(currentUserId == userGuid && !User.IsInRole(ApplicationRoles.ShopShowAll.ToString())))
            {
                result = Factory.Delete<IUserShopDto>($"{userGuid}|{shopGuid}");
            }

            return Json(new
            {
                success = result,
                message = ""
            });
        }

        [HttpPost]
        [AuthorizeRoles(ApplicationRoles.UserEdit)]
        public ActionResult DeleteAccountUserAssociation(string userGuid, string accountGuid)
        {
            bool result = false;

            var currentUserId = User.Identity.GetUserId();
            if (!(currentUserId == userGuid && !User.IsInRole(ApplicationRoles.AccountShowAll.ToString())))
            {
                result = Factory.Delete<IUserAccountDto>($"{userGuid}|{accountGuid}");
            }

            return Json(new
            {
                success = result,
                message = ""
            });
        }

        private void GetUserManageSelections(ref UserViewModel user)
        {
            user.AvailableGroups = Factory.GetDisplayList<IGroupDto>().ToList();

            var allowedShops = Factory.GetDisplayList<IShopDto>().Select(x => Guid.Parse(x.Key)).ToList();
            if (user?.ShopMemberships != null)
                user.ShopMemberships = allowedShops.Intersect(user.ShopMemberships).ToList();

            var allowedAccounts = Factory.GetDisplayList<IAccountDto>().Select(x => Guid.Parse(x.Key)).ToList();
            if (user?.AccountMemberships != null)
                user.AccountMemberships = allowedAccounts.Intersect(user.AccountMemberships).ToList();
        }

        #endregion

        private void RefreshMyRoles()
        {
            // Load SignIn Manager.
            using (var signinMgr = HttpContext.GetOwinContext().Get<ApplicationSignInManager>())
            {
                var user = new UserEntityModel
                {
                    Id = Factory.User.UserGuid,
                    UserName = Factory.User.UserName
                };

                // Reauthenticate to Reload Current User Roles.
                signinMgr.SignIn(user, false, false);
            }
        }
    }
}