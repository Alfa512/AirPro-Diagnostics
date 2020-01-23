using System.Threading.Tasks;
using System.Web.Mvc;
using AirPro.Common.Enumerations;
using AirPro.Service;
using AirPro.Service.DTOs.Interface;
using AirPro.Site.Areas.Admin.Models.Decision;
using AirPro.Site.Attributes;
using AirPro.Site.Results;
using AutoMapper;
using UniMatrix.Common.Extensions;

namespace AirPro.Site.Areas.Admin.Controllers
{
    [AuthorizeRoles(ApplicationRoles.DecisionManageView, ApplicationRoles.DecisionManageEdit)]
    public class DecisionController : BaseController
    {
        public ActionResult Index()
        {
            return new RedirectResult("Management", true);
        }

        public ActionResult Management()
        {
            return View("DecisionManagement");
        }

        [HttpPost]
        public async Task<JsonCamelCaseResult> GetDecisionById(int decisionId)
        {
            // Load Decision.
            var decision = await Factory.GetByIdAsync<IDecisionDto>(decisionId.ToString());

            // Return.
            return new JsonCamelCaseResult(decision);
        }

        [HttpPost]
        [AuthorizeRoles(ApplicationRoles.DecisionManageEdit)]
        public async Task<JsonCamelCaseResult> SaveDecision(DecisionViewModel decision)
        {
            // Save Decision.
            var update = await Factory.SaveAsync(Mapper.Map<IDecisionDto>(decision));

            // Return.
            return new JsonCamelCaseResult(update);
        }

        [HttpPost]
        public async Task<JsonCamelCaseResult> GetDecisionsByGridPage(int current, int rowCount, string searchPhrase)
        {
            // Load Arguments.
            var args = new ServiceArgs();
            args.AddGridOptions(current, rowCount, Request.Form.GetDynamicSortString(), searchPhrase);

            // Load Result.
            var decisions = await Factory.GetAllByGridPageAsync<IDecisionDto>(args);
            return new JsonCamelCaseResult(decisions);
        }
    }
}