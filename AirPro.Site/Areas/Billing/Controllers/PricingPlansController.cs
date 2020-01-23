using AirPro.Common.Enumerations;
using AirPro.Entities;
using AirPro.Service;
using AirPro.Service.DTOs.Interface;
using AirPro.Site.Areas.Billing.Models;
using AirPro.Site.Attributes;
using AutoMapper;
using Dapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using UniMatrix.Common.Extensions;

namespace AirPro.Site.Areas.Billing.Controllers
{
    [AuthorizeRoles(ApplicationRoles.PricingPlanView, ApplicationRoles.PricingPlanCreate, ApplicationRoles.PricingPlanEdit)]
    public class PricingPlansController : Controller
    {
        private ServiceFactory _factory;
        private ServiceFactory Factory => _factory ?? (_factory = new ServiceFactory(MvcApplication.ConnectionString, User.Identity));

        public ActionResult Index()
        {
            return View("PricingPlansHome");
        }

        [HttpPost]
        public ActionResult GetPricingPlansByPage(int current, int rowCount, string searchPhrase)
        {
            var result = JsonConvert.SerializeObject(
                Factory.GetAllByGridPage<IPricingPlanDto>(current, rowCount, Request.Form.GetDynamicSortString(), searchPhrase), Formatting.Indented,
                new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });

            return Content(result, "application/json");
        }

        [HttpGet]
        public ActionResult ManagePricingPlan(string id)
        {
            var pricingPlan = GetPricingPlan(id);
            ViewBag.Currencies = SelectListItemCache.BillingCurrencySelectItems();

            return PartialView("_PricingPlanManage", pricingPlan);
        }

        private PricingPlanViewModel GetPricingPlan(string id)
        {
            var pricingPlan = !string.IsNullOrEmpty(id)
                ? Mapper.Map<PricingPlanViewModel>(Factory.GetById<IPricingPlanDto>(id))
                : new PricingPlanViewModel
                {
                    PricingPlanActiveInd = true,
                    LineItems = GetPricingPlanTemplate().ToList()
                };
            return pricingPlan;
        }

        [HttpPost]
        [AuthorizeRoles(ApplicationRoles.PricingPlanCreate, ApplicationRoles.PricingPlanEdit)]
        public ActionResult ManagePricingPlan(PricingPlanViewModel pricingPlan)
        {
            // Check Model.
            if (ModelState.IsValid)
            {
                pricingPlan = Mapper.Map<PricingPlanViewModel>(Factory.Save(Mapper.Map<IPricingPlanDto>(pricingPlan)));
            }

            ViewBag.Currencies = SelectListItemCache.BillingCurrencySelectItems();
            return PartialView("_PricingPlanManage", pricingPlan);
        }

        private IEnumerable<PricingPlanLineItemViewModel> GetPricingPlanTemplate()
        {
            // Create Result.
            List<PricingPlanLineItemViewModel> result = new List<PricingPlanLineItemViewModel>();

            // Load Template.
            using (EntityDbContext db = new EntityDbContext())
            {
                var conn = db.Database.Connection;
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                result.AddRange(conn.Query<PricingPlanLineItemViewModel>("Billing.usp_GetPricingPlanTemplate").ToList());

                conn.Close();
            }

            // Return.
            return result;
        }

        [HttpPost]
        [AuthorizeRoles(ApplicationRoles.PricingPlanCreate)]
        public ActionResult ClonePlan(string id)
        {
            var errorResponse = new
            {
                success = false,
                message = "Pricing Plan does not exists"
            };

            if (string.IsNullOrEmpty(id))
            {
                return Json(errorResponse);
            }

            //Get the pricing plan
            var pricingPlan = GetPricingPlan(id.ToString());
            if (pricingPlan == null)
            {
                return Json(errorResponse);
            }

            pricingPlan.PricingPlanName += " (Copy)";
            pricingPlan.PricingPlanId = 0;

            pricingPlan = Mapper.Map<PricingPlanViewModel>(Factory.Save(Mapper.Map<IPricingPlanDto>(pricingPlan)));

            return Json(new
            {
                success = pricingPlan != null,
                data = new
                {
                    name = pricingPlan?.PricingPlanName
                },
                message = ""
            });
        }
    }
}