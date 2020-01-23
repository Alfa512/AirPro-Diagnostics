using AirPro.Common.Enumerations;
using AirPro.Library;
using AirPro.Service;
using AirPro.Service.DTOs.Interface;
using AirPro.Site.Models.Client;
using AutoMapper;
using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using UniMatrix.Common.Extensions;

namespace AirPro.Site.Controllers
{
    public class ClientController : BaseController
    {
        public async Task<ActionResult> Registration(string id = null)
        {
            if (id == null) return View("NotFound");

            var registrationDto = await Factory.GetByIdAsync<IRegistrationDto>(id.FromShortGuid().ToString());
            if (registrationDto == null || registrationDto.RegistrationStatus == RegistrationStatus.Completed || registrationDto.RegistrationStatus == RegistrationStatus.Withdrawn)
            {
                return View("NotFound");
            }

            switch (registrationDto.RegistrationStatus)
            {
                case RegistrationStatus.Contract:
                    return View("ThankYou");
                case RegistrationStatus.Sent:
                    registrationDto.RegistrationStatus = RegistrationStatus.Viewed;
                    await Factory.SaveAsync(registrationDto);
                    break;
            }

            var model = Mapper.Map<CreateViewModel>(registrationDto);
            model.AccountInformation.DifShopInfo = registrationDto.DifferentShopInfo;

            await LoadSelections(model);

            return View(model);
        }

        private async Task LoadSelections(CreateViewModel model)
        {
            var options = await Factory.GetByIdAsync<IRegistrationOptionsDto>(null);
            model.States = options.StateSelection;
            model.BillingCycles = options.BillingCycleSelection;
            model.DirectRepairPartners = options.DirectPartnersSelection;
            model.OEMCertifications = options.Programms;
        }

        [HttpPost]
        public async Task<JsonResult> Create(CreateViewModel model)
        {
            var result = new { UpdateResult = new { Success = false, Message = "" } };

            if (!ModelState.IsValid) return Json(result);

            IRegistrationDto registration = new CreateViewModel();

            if (model.RegistrationId != Guid.Empty)
            {
                registration = Mapper.Map<CreateViewModel>(await Factory.GetByIdAsync<IRegistrationDto>(model.RegistrationId.ToString()));
            }

            Mapper.Map(model, registration);

            registration.RegistrationShop.BillingCycleId = model.AccountInformation.BillingCycleId;
            registration.RegistrationShop.AverageVehiclesPerMonth = model.RepairInformation.AverageMoVolume;
            registration.RegistrationShop.InsuranceCompanies = model.RepairInformation.DirectRepairPartners?.ToList();
            registration.RegistrationShop.VehicleMakes = model.RepairInformation.OEMCertifications?.ToList();
            registration.RegistrationShop.CCCShopId = model.ExternalServices.CCCShopId;
            registration.RegistrationShop.SendToMitchellInd = model.ExternalServices.SendToMitchell;
            registration.RegistrationStatus = RegistrationStatus.Contract;
            registration.DifferentShopInfo = model.AccountInformation.DifShopInfo;

            if (!string.IsNullOrEmpty(model.UserDetails.Password))
                registration.RegistrationUser.PasswordHash = (new PasswordHasher()).HashPassword(model.UserDetails.Password);

            await Factory.SaveAsync(registration);

            result = new { UpdateResult = new { Success = true, Message = "" } };

            return Json(result);
        }

        public async Task<JsonResult> IsShopNameUnique(string name, bool? differentShopInfo = null)
        {
            if (differentShopInfo == true) return Json(true);

            var validDto = (await Factory.GetAllAsync<IClientValidationDto>(new ServiceArgs { { "ShopName", name } })).FirstOrDefault();
            return Json(validDto?.IsValid ?? true);
        }

        public async Task<JsonResult> IsAccountNameUnique(string name)
        {
            var validDto = (await Factory.GetAllAsync<IClientValidationDto>(new ServiceArgs { { "AccountName", name } })).FirstOrDefault();
            return Json(validDto?.IsValid ?? true);
        }
    }
}