using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using AirPro.Common.Enumerations;
using AirPro.Service;
using AirPro.Service.DTOs.Interface;
using AirPro.Site.Areas.Admin.Models.Inventory;
using AirPro.Site.Attributes;
using AirPro.Site.Results;
using AutoMapper;
using Dapper;
using Microsoft.Ajax.Utilities;
using UniMatrix.Common.Extensions;

namespace AirPro.Site.Areas.Admin.Controllers
{
    [AuthorizeRoles(ApplicationRoles.InventoryDeviceView, ApplicationRoles.InventoryDeviceEdit, ApplicationRoles.InventoryDeviceCreate, ApplicationRoles.InventoryAssignmentEdit, ApplicationRoles.InventoryAssignmentView, ApplicationRoles.InventoryDepositEdit, ApplicationRoles.InventoryDepositView, ApplicationRoles.InventorySubscriptionEdit, ApplicationRoles.InventorySubscriptionView)]
    public class InventoryController : Controller
    {
        private ServiceFactory _factory;
        private ServiceFactory Factory => _factory ?? (_factory = new ServiceFactory(MvcApplication.ConnectionString, User.Identity));

        public ActionResult Index()
        {
            return View("InventoryHome");
        }

        [HttpPost]
        public ActionResult GetAirProToolStats()
        {
            using (var conn = new SqlConnection(MvcApplication.ConnectionString))
            {
                var stats = conn.Query<AirProToolStatsViewModel>("Inventory.usp_GetAirProToolStats", null, null, true, null, CommandType.StoredProcedure).ToList();

                return new JsonCamelCaseResult(stats, JsonRequestBehavior.DenyGet);
            }
        }

        [HttpPost]
        public ActionResult GetAirProToolsByPage(int current, int rowCount, string searchPhrase)
        {
            // Load Grid.
            return new JsonCamelCaseResult(Factory.GetAllByGridPage<IAirProToolDto>(current, rowCount, Request.Form.GetDynamicSortString(), searchPhrase), JsonRequestBehavior.DenyGet);
        }

        [HttpGet]
        [AuthorizeRoles(ApplicationRoles.InventoryDeviceEdit, ApplicationRoles.InventoryDeviceCreate, ApplicationRoles.InventoryAssignmentEdit, ApplicationRoles.InventoryAssignmentView, ApplicationRoles.InventoryDepositEdit, ApplicationRoles.InventoryDepositView, ApplicationRoles.InventorySubscriptionEdit, ApplicationRoles.InventorySubscriptionView, ApplicationRoles.InventoryDeviceView)]
        public ActionResult ManageAirProTool(string id)
        {
            // Load AirPro Tool.
            var airProTool = !string.IsNullOrEmpty(id)
                ? Mapper.Map<AirProToolViewModel>(Factory.GetById<IAirProToolDto>(id))
                : new AirProToolViewModel
                {
                    TabletTab = new AirProToolTabletTabViewModel(),
                    DepositsTab = new AirProToolDepositsTabViewModel(),
                    HardwareTab = new AirProToolHardwareTabViewModel(),
                    SubscriptionsTab = new AirProToolSubscriptionsTabViewModel(),
                    J2534Tab = new AirProToolJ2534TabViewModel()
                };

            InitManageSelectLists(airProTool);

            return PartialView("_AirProToolManage", airProTool);
        }

        private Dictionary<ToolType, string> GetToolTypeList()
        {
            return ((ToolType[])Enum.GetValues(typeof(ToolType))).ToDictionary(key => key, value => value.ToString());
        }

        private static Dictionary<int, string> GetJ2534BrandList()
        {
            return new Dictionary<int, string> { 
                {(int)J2534BrandEnum.DG, "DG"},
                {(int)J2534BrandEnum.DrewTech, "DrewTech" }
            };
        }

        private static Dictionary<int, string> GetJ2534ModelList()
        {
            return new Dictionary<int, string> {
                {(int)J2534ModelEnum.CarDaqPlus2, "CarDaq Plus 2"},
                {(int)J2534ModelEnum.CarDaqPlus3, "CarDaq Plus 3" },
                {(int)J2534ModelEnum.DbriDGePro, "D-briDGe Pro" },
                {(int)J2534ModelEnum.VSIJ2534, "VSI-J2534" }
            };
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(ApplicationRoles.InventoryDeviceEdit, ApplicationRoles.InventoryDeviceCreate, ApplicationRoles.InventoryAssignmentEdit, ApplicationRoles.InventoryAssignmentView, ApplicationRoles.InventoryDepositEdit, ApplicationRoles.InventoryDepositView, ApplicationRoles.InventorySubscriptionEdit, ApplicationRoles.InventorySubscriptionView, ApplicationRoles.InventoryDeviceView)]
        public ActionResult ManageAirProTool(AirProToolViewModel airProTool)
        {
            // Check Model.
            if (ModelState.IsValid)
            {
                var r = Mapper.Map<IAirProToolDto>(airProTool);
                r.Subscriptions = r.Subscriptions.Where(d => !d.Vendor.IsNullOrWhiteSpace()
                                                             && !d.Username.IsNullOrWhiteSpace()
                                                             && !d.Password.IsNullOrWhiteSpace());
                // Save Updates.
                airProTool = Mapper.Map<AirProToolViewModel>(Factory.Save(r));
            }

            InitManageSelectLists(airProTool);

            // Reset Session.
            if (Session["AirProToolStats"] != null)
                Session.Remove("AirProToolStats");

            return PartialView("_AirProToolManage", airProTool);
        }

        private void InitManageSelectLists(AirProToolViewModel airProTool)
        {
            airProTool.J2534BrandList = GetJ2534BrandList();
            airProTool.J2534ModelList = GetJ2534ModelList();
            airProTool.ToolTypeList = GetToolTypeList();
        }
    }
}