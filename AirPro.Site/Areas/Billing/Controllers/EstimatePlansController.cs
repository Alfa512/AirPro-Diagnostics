using System.Web.Mvc;
using AirPro.Common.Enumerations;
using AirPro.Service;
using AirPro.Service.DTOs.Interface;
using AirPro.Site.Areas.Billing.Models;
using AirPro.Site.Attributes;
using AirPro.Site.Results;
using AutoMapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UniMatrix.Common.Extensions;

namespace AirPro.Site.Areas.Billing.Controllers
{
    [AuthorizeRoles(ApplicationRoles.EstimatePlanView, ApplicationRoles.EstimatePlanCreate, ApplicationRoles.EstimatePlanEdit)]
    public class EstimatePlansController : Controller
    {
        private ServiceFactory _factory;
        private ServiceFactory Factory => _factory ?? (_factory = new ServiceFactory(MvcApplication.ConnectionString, User.Identity));

        public ActionResult Index()
        {
            return View("EstimatePlansHome");
        }

        [HttpPost]
        public ActionResult GetEstimatePlansByPage(int current, int rowCount, string searchPhrase)
        {
            var result = JsonConvert.SerializeObject(
                Factory.GetAllByGridPage<IEstimatePlanDto>(current, rowCount, Request.Form.GetDynamicSortString(), searchPhrase), Formatting.Indented,
                new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });

            return new JsonCamelCaseResult(result);
        }

        [HttpGet]
        public ActionResult ManageEstimatePlan(string id)
        {
            var estimatePlan = GetEstimatePlan(id);

            return PartialView("_EstimatePlanManage", estimatePlan);
        }

        private EstimatePlanViewModel GetEstimatePlan(string id)
        {
            var model = Mapper.Map<EstimatePlanViewModel>(Factory.GetById<IEstimatePlanDto>(id));

            return model;
        }

        [HttpPost]
        [AuthorizeRoles(ApplicationRoles.EstimatePlanCreate, ApplicationRoles.EstimatePlanEdit)]
        public ActionResult ManageEstimatePlan(EstimatePlanViewModel estimatePlan)
        {
            // Check Model.
            if (ModelState.IsValid)
            {
                estimatePlan = Mapper.Map<EstimatePlanViewModel>(Factory.Save(Mapper.Map<IEstimatePlanDto>(estimatePlan)));
            }

            return PartialView("_EstimatePlanManage", estimatePlan);
        }

        [HttpPost]
        [AuthorizeRoles(ApplicationRoles.EstimatePlanCreate)]
        public ActionResult CloneEstimatePlan(string id)
        {
            var errorResponse = new
            {
                success = false,
                message = "Estimate Plan does not exists"
            };

            if (string.IsNullOrEmpty(id))
            {
                return Json(errorResponse);
            }

            //Get the pricing plan
            var estimatePlan = GetEstimatePlan(id);
            if (estimatePlan == null)
            {
                return Json(errorResponse);
            }

            estimatePlan.Name += " (Copy)";
            estimatePlan.EstimatePlanId = 0;

            estimatePlan = Mapper.Map<EstimatePlanViewModel>(Factory.Save(Mapper.Map<IEstimatePlanDto>(estimatePlan)));

            return Json(new
            {
                success = estimatePlan != null,
                data = new
                {
                    name = estimatePlan?.Name
                },
                message = ""
            });
        }
    }
}