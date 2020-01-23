using AirPro.Common.Enumerations;
using AirPro.Service;
using AirPro.Service.DTOs.Interface;
using AirPro.Site.Areas.Admin.Models.InsuranceCompanies;
using AirPro.Site.Attributes;
using AirPro.Site.Results;
using AutoMapper;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using UniMatrix.Common.Extensions;

namespace AirPro.Site.Areas.Admin.Controllers
{
    [AuthorizeRoles(ApplicationRoles.InsuranceCoAdmin)]
    public class InsuranceCompaniesController : Controller
    {
        private ServiceFactory _factory;
        private ServiceFactory Factory => _factory ?? (_factory = new ServiceFactory(MvcApplication.ConnectionString, User.Identity));

        public ActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult GetCompaniesByPage(int current, int rowCount, string searchPhrase)
        {
            var result = Factory.GetAllByGridPage<IInsuranceCompanyDto>(current, rowCount,
                Request.Form.GetDynamicSortString(), searchPhrase);

            return new JsonCamelCaseResult(result);
        }

        private async Task LoadSelections(InsuranceCompanyViewModel company)
        {
            company.CccInsuranceCompanies = (await Factory.GetAllAsync<ICCCInsuranceCompanyDto>()).Select(x => new SelectListItem { Disabled = x.RepairInsuranceCompanyId != null && x.RepairInsuranceCompanyId != company.InsuranceCompanyId, Value = x.CCCInsuranceCompanyId, Text = $@"{x.CCCInsuranceCompanyName} ({x.CCCInsuranceCompanyId?.Trim()})" }).OrderBy(o => o.Text);
        }

        [HttpGet]
        public async Task<ActionResult> ManageCompany(string id)
        {
            var company = !string.IsNullOrEmpty(id)
                ? Mapper.Map<InsuranceCompanyViewModel>(Factory.GetById<IInsuranceCompanyDto>(id))
                : new InsuranceCompanyViewModel();
            await LoadSelections(company);

            return PartialView("_InsuranceCompanyManage", company);
        }

        [HttpPost]
        public async Task<ActionResult> ManageCompany(InsuranceCompanyViewModel company)
        {
            // Check Model.
            if (ModelState.IsValid)
            {
                company = Mapper.Map<InsuranceCompanyViewModel>(Factory.Save(company as IInsuranceCompanyDto));
                SelectListItemCache.ClearCache(SelectListItemCacheType.InsuranceCompany);
            }
            await LoadSelections(company);

            return PartialView("_InsuranceCompanyManage", company);
        }
    }
}