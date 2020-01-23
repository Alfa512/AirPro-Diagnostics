using System.Threading.Tasks;
using System.Web.Mvc;
using AirPro.Common.Enumerations;
using AirPro.Service;
using AirPro.Service.DTOs.Interface;
using AirPro.Site.Areas.Admin.Models.Recommendation;
using AirPro.Site.Attributes;
using AirPro.Site.Results;
using AutoMapper;
using UniMatrix.Common.Extensions;

namespace AirPro.Site.Areas.Admin.Controllers
{
    [AuthorizeRoles(ApplicationRoles.RecommendationManageView, ApplicationRoles.RecommendationManageEdit)]
    public class RecommendationController : BaseController
    {
        public ActionResult Index()
        {
            return RedirectToAction("Management");
        }

        public ActionResult Management()
        {
            return View("RecommendationManagement");
        }

        [HttpPost]
        public async Task<JsonCamelCaseResult> GetRecommendationsByGridPage(int current, int rowCount, string searchPhrase)
        {
            // Load Arguments.
            var args = new ServiceArgs();
            args.AddGridOptions(current, rowCount, Request.Form.GetDynamicSortString(), searchPhrase);

            // Load Result.
            var recommendations = await Factory.GetAllByGridPageAsync<ITroubleCodeRecommendationDto>(args);
            return new JsonCamelCaseResult(recommendations);
        }

        [HttpPost]
        public async Task<JsonCamelCaseResult> GetRecommendationById(int recommendationId)
        {
            // Check Recommendation.
            if (recommendationId == 0)
                return new JsonCamelCaseResult(new RecommendationViewModel {ActiveInd = true});

            // Load Recommendation.
            var recommendation = await Factory.GetByIdAsync<ITroubleCodeRecommendationDto>(recommendationId.ToString());

            // Return.
            return new JsonCamelCaseResult(recommendation);
        }

        [HttpPost]
        [AuthorizeRoles(ApplicationRoles.RecommendationManageEdit)]
        public async Task<JsonCamelCaseResult> SaveRecommendation(RecommendationViewModel recommendation)
        {
            // Save Recommendation.
            var update = await Factory.SaveAsync(Mapper.Map<ITroubleCodeRecommendationDto>(recommendation));

            // Return.
            return new JsonCamelCaseResult(update);
        }
    }
}