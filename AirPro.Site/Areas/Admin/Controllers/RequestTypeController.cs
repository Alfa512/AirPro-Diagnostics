using System.Threading.Tasks;
using System.Web.Mvc;
using AirPro.Common.Enumerations;
using AirPro.Library.Models.Concrete;
using AirPro.Service.DTOs.Interface;
using AirPro.Site.Attributes;
using AirPro.Site.Results;

namespace AirPro.Site.Areas.Admin.Controllers
{
    [AuthorizeRoles(ApplicationRoles.RequestTypeAdmin)]
    public class RequestTypeController : BaseController
    {
        public ActionResult Index()
        {
            return View("RequestTypeDashboard");
        }

        public ActionResult RequestTypeDashboard()
        {
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<JsonCamelCaseResult> GetRequestTypes()
        {
            return new JsonCamelCaseResult(await Factory.GetAllAsync<IRequestTypeDto>(), JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public async Task<ActionResult> GetValidationRules()
        {
            return new JsonCamelCaseResult(await Factory.GetDisplayListAsync<IValidationRuleDto>(), JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public async Task<JsonCamelCaseResult> SaveRequestType(RequestTypeViewModel update)
        {
            var requestType = await Factory.SaveAsync<IRequestTypeDto>(update);

            SelectListItemCache.ClearCache(SelectListItemCacheType.RequestType);
            SelectListItemCache.ClearCache(SelectListItemCacheType.RequestTypeCategory);

            return new JsonCamelCaseResult(requestType, JsonRequestBehavior.DenyGet);
        }
    }
}
