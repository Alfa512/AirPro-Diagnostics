using AirPro.Common.Enumerations;
using AirPro.Library;
using AirPro.Service;
using AirPro.Service.DTOs.Interface;
using AirPro.Site.Areas.Admin.Models.Registration;
using AirPro.Site.Attributes;
using AirPro.Site.Results;
using AutoMapper;
using Microsoft.AspNet.Identity;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using AirPro.Logging;
using AirPro.Storage;
using Dapper;
using UniMatrix.Common.Extensions;

namespace AirPro.Site.Areas.Admin.Controllers
{
    [AuthorizeRoles(ApplicationRoles.RegistrationView, ApplicationRoles.RegistrationEdit, ApplicationRoles.RegistrationCreate)]
    public class RegistrationController : BaseController
    {
        // GET: Admin/Registration
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetRegistrationsByPage(int current, int rowCount, string searchPhrase, string status)
        {
            int registrationStatus = -1; // default value for all.
            int.TryParse(status, out registrationStatus);
            var args = new ServiceArgs();
            args.AddGridOptions(current, rowCount, Request.Form.GetDynamicSortString(), searchPhrase);
            args.Add("Status", registrationStatus);


            var res = Factory.GetAllByGridPage<IRegistrationDto>(args);
            // Load Grid.
            return new JsonCamelCaseResult(res, JsonRequestBehavior.DenyGet);
        }

        [HttpGet]
        public async Task<ActionResult> ManageRegistration(Guid? id = null)
        {
            var model = new ManageRegistrationViewModel();
            if(id != null)
            {
                var registrationDTO = await Factory.GetByIdAsync<IRegistrationDto>(id.ToString());
                model = Mapper.Map<ManageRegistrationViewModel>(registrationDTO);
            }
            await LoadSelections(model);
            return View("_ManageRegistration", model);
        }

        private async Task LoadSelections(ManageRegistrationViewModel model)
        {
            var options = await Factory.GetByIdAsync<IRegistrationOptionsDto>(null);
            model.StateSelection = options.StateSelection;
            model.RequestTypeSelection = options.RequestTypeSelection;
            model.BillingCycleSelection = options.BillingCycleSelection;
            model.CurrencySelection = options.CurrencySelection;
            model.EstimatePlanSelection = options.EstimatePlanSelection;
            model.GroupSelection = options.GroupSelection;
            model.PricingPlanSelection = options.PricingPlanSelection;
            model.AllVehicleMakes = options.AllVehicleMakes;
            model.InsuranceCompanies = options.InsuranceCompanies;
        }

        [HttpPost]
        public async Task<ActionResult> ManageRegistration(ManageRegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Determine Notification.
                var sendLink = model.RegistrationId == Guid.Empty || model.RegistrationStatus == RegistrationStatus.Resent;

                // Update Status.
                model.RegistrationStatus = model.RegistrationStatus == RegistrationStatus.Resent ? RegistrationStatus.Sent : model.RegistrationStatus;

                // Save Update.
                var registration = await Factory.SaveAsync<IRegistrationDto>(model);

                // Check Notifications.
                if (!sendLink && registration.RegistrationStatus != RegistrationStatus.Completed) return new EmptyResult();

                // Send Notifications.
                using (var queue = new MessageQueue())
                {
                    if (sendLink)
                        await queue.AddNotificationQueueMessageAsync(template: NotificationTemplate.RegistrationEmail, id: registration.RegistrationShop.RegistrationShopId, userGuid: Guid.Parse(User.Identity.GetUserId()));
                    else
                        await queue.AddNotificationQueueMessageAsync(template: NotificationTemplate.RegistrationWelcomeEmail, id: registration.RegistrationShop.RegistrationShopId, userGuid: Guid.Parse(User.Identity.GetUserId()));
                }

                // Check Callbacks.
                if (registration.RegistrationStatus != RegistrationStatus.Completed || string.IsNullOrWhiteSpace(registration.CallbackUrl)) return new EmptyResult();

                try
                {
                    // Perform Callbacks.
                    using (var conn = new SqlConnection(MvcApplication.ConnectionString))
                    {
                        // Load Registration Status.
                        var status = await conn.QueryFirstAsync<MitchellRegistrationStatusDto>("Service.usp_GetMitchellRegistration", new { registration.RegistrationId }, commandType: CommandType.StoredProcedure);

                        // Check Status.
                        if (status != null)
                        {
                            // Send Registration Status.
                            var client = new HttpClient();
                            var response = await client.PostAsJsonAsync(registration.CallbackUrl, status);

                            // Check Response.
                            if (response.IsSuccessStatusCode)
                            {
                                await conn.ExecuteAsync("Service.usp_UpdateMitchellRegistration", new { status.MitchellAccountId, UserEmail = registration.Email }, commandType: CommandType.StoredProcedure);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Logger.LogException(e);
                }

                return new EmptyResult();
            }

            await LoadSelections(model);

            return PartialView("_ManageRegistration", model);
        }

        [HttpPost]
        public async Task<JsonResult> IsEmailValid(string Email, Guid? RegistrationId = null)
        {
            if (RegistrationId == null) RegistrationId = Guid.Empty;

            var validDto = (await Factory.GetAllAsync<IClientValidationDto>(new ServiceArgs { { "UserEmail", Email }, { "RegistrationId", RegistrationId } })).FirstOrDefault();
            return Json(validDto.IsValid);
        }

        [HttpPost]
        public async Task<JsonResult> IsAccountNameValid([Bind(Prefix = "Account.Name")] string Name)
        {
            var validDto = (await Factory.GetAllAsync<IClientValidationDto>(new ServiceArgs { { "AccountName", Name }})).FirstOrDefault();
            return Json(validDto.IsValid);
        }

        [HttpPost]
        public async Task<JsonResult> IsShopNameValid([Bind(Prefix = "Shop.Name")] string Name, bool DifferentShopInfo = false)
        {
            if (DifferentShopInfo && string.IsNullOrWhiteSpace(Name)) return Json(false);

            var validDto = (await Factory.GetAllAsync<IClientValidationDto>(new ServiceArgs { { "ShopName", Name } })).FirstOrDefault();
            return Json(validDto.IsValid);
        }

        [HttpPost]
        public JsonResult IsValidShopName(bool DifferentShopInfo)
        {
            if (DifferentShopInfo) return Json(true);

            return Json(false);
        }
    }
}