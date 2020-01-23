using AirPro.Common.Enumerations;
using AirPro.Logging;
using AirPro.Service.DTOs.Interface;
using AirPro.Site.Attributes;
using AirPro.Site.Results;
using System;
using System.Data.SqlClient;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AirPro.Site.Areas.Admin.Controllers
{
    [AuthorizeRoles(ApplicationRoles.SupportMoveRequest, ApplicationRoles.SupportChangeRepairVin)]
    public class SupportController : BaseController
    {
        public ActionResult Index()
        {
            return View("SupportHome");
        }

        [HttpGet]
        [AuthorizeRoles(ApplicationRoles.SupportMoveRequest)]
        public ActionResult ManageRequestMove()
        {
            return View("_ManageRequestMove");
        }

        [HttpPost]
        [AuthorizeRoles(ApplicationRoles.SupportMoveRequest)]
        public async Task<ActionResult> ManageRequestMove(int requestId, int repairId)
        {
            try
            {
                var scanResult = await Factory.GetByIdAsync<IRequestDto>(requestId.ToString());
                scanResult.RepairId = repairId;
                await Factory.SaveAsync<IRequestDto>(scanResult);
            }
            catch (Exception e)
            {
                Logger.LogException(e);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Move request error");
            }
            return Json(new { success = true });
        }

        [HttpGet]
        [AuthorizeRoles(ApplicationRoles.SupportChangeRepairVin)]
        public ActionResult ManageVinChange()
        {
            return View("_ManageVinChange");
        }

        [HttpPost]
        [AuthorizeRoles(ApplicationRoles.SupportChangeRepairVin)]
        public async Task<ActionResult> ManageVinChange(string oldRepairId, string newVIN)
        {
            try
            {
                var repair = await Factory.GetByIdAsync<IRepairDto>(oldRepairId);
                repair.VehicleVIN = newVIN;
                Factory.Save<IRepairDto>(repair);
            }
            catch (SqlException e)
            {
                Logger.LogException(e);
                return Json(new { success = false, message = e.Message });
            }
            catch (Exception e)
            {
                Logger.LogException(e);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Move request error");
            }
            return Json(new { success = true });
        }


        #region Lookups

        [HttpPost]
        public async Task<ActionResult> GetVehicleDetailsByVIN(string vin)
        {
            try
            {
                var vehicle = await Factory.GetByIdAsync<IVehicleDto>(vin);

                if (vehicle == null) throw new Exception("Not Found");
                return new JsonCamelCaseResult(vehicle);
            }
            catch (Exception e)
            {
                Logger.LogException(e);
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Not found");
        }

        [HttpPost]
        public async Task<ActionResult> GetRequestDetails(int id)
        {
            try
            {
                var request = await Factory.GetByIdAsync<IRequestDto>(id.ToString());

                if (request == null) throw new Exception("Not Found");
                return new JsonCamelCaseResult(request);
            }
            catch (Exception e)
            {
                Logger.LogException(e);
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Not found");

        }

        [HttpPost]
        public async Task<ActionResult> GetRepairDetails(int id)
        {
            try
            {
                var repair = await Factory.GetByIdAsync<IRepairDto>(id.ToString());

                if (repair == null) throw new Exception("Not Found");
                return new JsonCamelCaseResult(repair);
            }
            catch (Exception e)
            {
                Logger.LogException(e);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Not found");
        }

        #endregion

    }
}