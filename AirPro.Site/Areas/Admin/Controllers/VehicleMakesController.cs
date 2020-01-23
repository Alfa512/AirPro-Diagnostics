using System.Linq;
using AirPro.Common.Enumerations;
using AirPro.Site.Attributes;
using System.Web.Mvc;
using AirPro.Service;
using AirPro.Service.DTOs.Interface;
using AirPro.Site.Areas.Admin.Models.VehicleMakes;
using AirPro.Site.Results;
using AutoMapper;
using UniMatrix.Common.Extensions;

namespace AirPro.Site.Areas.Admin.Controllers
{
    [AuthorizeRoles(ApplicationRoles.VehicleMakeAdmin)]
    public class VehicleMakesController : Controller
    {
        private ServiceFactory _factory;
        private ServiceFactory Factory => _factory ?? (_factory = new ServiceFactory(MvcApplication.ConnectionString, User.Identity));

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetVehiclesByPage(int current, int rowCount, string searchPhrase)
        {
            var result = Factory.GetAllByGridPage<IVehicleMakeDto>(current, rowCount,
                Request.Form.GetDynamicSortString(), searchPhrase);

            return new JsonCamelCaseResult(result);
        }

        [HttpGet]
        public ActionResult ManageVehicleMake(string id)
        {
            var vehicle = !string.IsNullOrEmpty(id)
                ? Mapper.Map<VehicleMakeViewModel>(Factory.GetById<IVehicleMakeDto>(id))
                : new VehicleMakeViewModel();

            ViewBag.VehicleMakeTypes = Factory.GetDisplayList<IVehicleMakeTypeDto>()
                .Select(s => new SelectListItem
                {
                    Value = s.Key,
                    Text = s.Value
                }).OrderBy(s => s.Text).ToList();

            return PartialView("_VehicleMakeManage", vehicle);
        }

        [HttpPost]
        public ActionResult ManageVehicleMake(VehicleMakeViewModel vehicle)
        {
            // Check Model.
            if (ModelState.IsValid)
            {
                vehicle = Mapper.Map<VehicleMakeViewModel>(Factory.Save(vehicle as IVehicleMakeDto));
                SelectListItemCache.ClearCache(SelectListItemCacheType.InsuranceCompany);
            }

            return PartialView("_VehicleMakeManage", vehicle);
        }
    }
}