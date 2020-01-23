using AirPro.Common.Enumerations;
using AirPro.Entities;
using AirPro.Entities.Repair;
using AirPro.Library;
using AirPro.Library.Models.Concrete;
using AirPro.Logging;
using AirPro.Parser;
using AirPro.Service;
using AirPro.Service.DTOs.Concrete;
using AirPro.Service.DTOs.Interface;
using AirPro.Site.Attributes;
using AirPro.Site.Helpers;
using AirPro.Site.Hubs;
using AirPro.Site.Models.Repairs;
using AirPro.Site.Models.Request;
using AirPro.Site.Results;
using AutoMapper;
using Dapper;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using AirPro.Storage;
using UniMatrix.Common.Extensions;

namespace AirPro.Site.Controllers
{
    [Authorize]
    public class RepairsController : BaseController
    {
        private EntityDbContext _db;
        private EntityDbContext Db => _db ?? (_db = new EntityDbContext());

        private RepairLibrary _repairLib;
        private RepairLibrary RepairLib => _repairLib ?? (_repairLib = new RepairLibrary(Db, User.Identity));

        #region Select Lists

        private IEnumerable<SelectListItem> GetVehicleMakeSelectList() => Db.RepairVehicleMakes
            ?.OrderBy(i => i.VehicleMakeName).AsNoTracking()
            .Select(m => new SelectListItem { Value = m.VehicleMakeId.ToString(), Text = m.VehicleMakeName }).ToList();

        private IEnumerable<SelectListItem> SelfScanListItems => (Session["SelfScanListItems"] ?? (Session["SelfScanListItems"] = GetSelfScanListItems())) as IEnumerable<SelectListItem>;

        private IEnumerable<SelectListItem> GetSelfScanListItems() =>
            Factory.GetDisplayList<IShopDto>(new ServiceArgs { { "AllowSelfScan", true } })
                .Select(s => new SelectListItem { Value = s.Key, Text = s.Value }).ToList();

        private IEnumerable<SelectListItem> ShopSelectListItems => (Session["ShopSelectListItems"] ?? (Session["ShopSelectListItems"] = GetShopSelectListItems())) as IEnumerable<SelectListItem>;

        private IEnumerable<SelectListItem> GetShopSelectListItems() =>
            Factory.GetDisplayList<IShopDto>()
                .Select(s => new SelectListItem { Value = s.Key, Text = s.Value }).ToList();

        private IEnumerable<SelectListItem> ScanAnalysisListItems => (Session["ScanAnalysisListItems"] ?? (Session["ScanAnalysisListItems"] = GetScanAnalysisListItems())) as IEnumerable<SelectListItem>;

        private IEnumerable<SelectListItem> GetScanAnalysisListItems() =>
            Factory.GetDisplayList<IShopDto>(new ServiceArgs { { "AllowScanAnalysis", true } })
                .Select(s => new SelectListItem { Value = s.Key, Text = s.Value }).ToList();

        #endregion

        private string RepairNotice
        {
            get
            {
                if (TempData.Keys.Contains("RepairNotice"))
                    return TempData["RepairNotice"].ToString();

                return null;
            }
            set
            {
                TempData["RepairNotice"] = value;
            }
        }

        [AuthorizeRoles(ApplicationRoles.RepairCreate, ApplicationRoles.RepairEdit, ApplicationRoles.RepairView)]
        public ActionResult Index()
        {
            return RedirectToAction("Dashboard");
        }

        /// <summary>
        /// Return Dashboard
        /// </summary>
        /// <param name="id">ShopID</param>
        /// <returns></returns>
        [AuthorizeRoles(ApplicationRoles.RepairCreate, ApplicationRoles.RepairEdit, ApplicationRoles.RepairView)]
        public async Task<ActionResult> Dashboard(Guid? id)
        {
            // Clear Cookie.
            if (Request.Cookies.AllKeys.Contains("RepairDashboardVersion"))
                Response.Cookies["RepairDashboardVersion"].Expires = DateTime.MinValue;

            // Set Notices.
            ViewBag.Notice = RepairNotice;

            // Set Select Lists.
            ViewBag.SelfScanShops = SelfScanListItems;
            ViewBag.ScanAnalysisShops = ScanAnalysisListItems;
            ViewBag.ShopSelectListItems = ShopSelectListItems;
            var agedRepairsUserPreference = GetAgedRepairsUserPreference()?.GetSettings<AgedRepairsUserPreferenceDto>();
            ViewBag.ShowAgingRepairs = agedRepairsUserPreference?.Date.Date != DateTime.UtcNow.Date;

            // Show Dashboard.
            return View("Dashboard");
        }

        private IUserPreferenceDto GetAgedRepairsUserPreference()
        {
            var agedRepairsUserPreference = Factory.GetAll<IUserPreferenceDto>(new ServiceArgs
            {
                {"ControlId", "ShowAgedRepairs"},
                {"UserGuid", User.Identity.GetUserId()}
            }).FirstOrDefault();
            return agedRepairsUserPreference;
        }

        public ActionResult TrackAgingRepairsModalShown()
        {
            var agedRepairsUserPreference = GetAgedRepairsUserPreference();

            if (agedRepairsUserPreference == null)
            {
                var userPreferenceDto = new UserPreferenceDto
                {
                    ControlId = "ShowAgedRepairs",
                    UserGuid = Guid.Parse(User.Identity.GetUserId()),
                    SettingsJson = JsonConvert.SerializeObject(new AgedRepairsUserPreferenceDto { Date = DateTime.UtcNow.Date })
                };
                Factory.Save<IUserPreferenceDto>(userPreferenceDto);
            }
            else
            {
                var settings = agedRepairsUserPreference.GetSettings<AgedRepairsUserPreferenceDto>();
                if (settings.Date.Date != DateTime.UtcNow.Date)
                {
                    settings.Date = DateTime.UtcNow.Date;
                    agedRepairsUserPreference.SettingsJson = JsonConvert.SerializeObject(settings);
                    Factory.Save<IUserPreferenceDto>(agedRepairsUserPreference);
                }
            }

            return null;
        }

        public async Task<ActionResult> CloseAgingRepairs(int[] repairIds)
        {
            foreach (var repairId in repairIds)
            {
                await Factory.DeleteAsync<IRepairDto>(repairId.ToString());
            }

            return Json(true);
        }

        public async Task<JsonCamelCaseResult> GetAgingRepairsByPage(int current, int rowCount, string searchPhrase)
        {
            // Load Arguments.
            var args = new ServiceArgs();
            args.AddGridOptions(current, rowCount, Request.Form.GetDynamicSortString(), searchPhrase);
            args.Add("AgingRepairs", true);

            // Load Grid Page.
            var repairs = await Factory.GetAllByGridPageAsync<IRepairDto>(args);

            // Return Result.
            return new JsonCamelCaseResult(repairs, JsonRequestBehavior.DenyGet);
        }

        [AuthorizeRoles(ApplicationRoles.RepairView)]
        public async Task<ActionResult> RequestScan(int? id)
        {
            var repair = PopulateRequestScanActionViewBag(id.Value);

            if (repair == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (repair.Status != RepairStatuses.Active)
            {
                return View("RepairClosed");
            }

            // Load Position Statements.
            var args = new ServiceArgs
            {
                {"UploadTypeId", (int) UploadType.VehicleMakes},
                {"UploadKey", repair.Vehicle.VehicleMakeId}
            };
            var positionStatements = Factory.GetDisplayList<IUploadDto>(args);

            var request = new RepairRequestScanViewModel
            {
                OrderId = id.Value,
                PositionStatementLinks = positionStatements.ToList()
            };
            PopulateToolId(request);

            ViewBag.RequestTypesInstructions = (await Factory.GetAllAsync<IRequestTypeDto>())
                .Select(t => new {RequestTypeId = t.RequestTypeId.ToString(), t.Instructions}).ToList();

            return View("RequestScan", request);
        }

        private void PopulateToolId(RepairRequestScanViewModel request)
        {
            var airProTools = ((IEnumerable<KeyValuePair<string, string>>)ViewBag.AirProTools);
            if (airProTools.Count() == 1)
            {
                request.ToolId = int.Parse(airProTools.FirstOrDefault().Key);
            }
        }

        private OrderEntityModel PopulateRequestScanActionViewBag(int id)
        {
            var repair = Db.RepairOrders.First(d => d.OrderId == id);
            if (repair == null) return null;

            // Load Tools.
            var tools = Factory.GetDisplayList<IAirProToolDto>(new ServiceArgs { { "ShopGuid", repair.ShopGuid }, { "ToolTypes", string.Join(",", new ToolType[] { ToolType.AirPro, ToolType.EuroPro }) } });
            ViewBag.AirProTools = tools;

            return repair;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("RequestScan")]
        [AuthorizeRoles(ApplicationRoles.RepairView)]
        public async Task<ActionResult> SubmitRequestScan(RepairRequestScanViewModel scanRequest)
        {
            // Create Scan Request.
            if (ModelState.IsValid)
            {
                var newRequestId = await RepairLib.CreateScanRequest(scanRequest);
                if (newRequestId > 0)
                {
                    // Scan Created.
                    TempData["RepairNotice"] = "Scan Request Created!";

                    // Update Technician Dashboard.
                    await new ClientHubMessenger().ScanRequestCreated(newRequestId)
                            .ConfigureAwait(continueOnCapturedContext: false);

                    var messenger = new ScanRequestHubMessenger();
                    await messenger.NotifyScanCreated(newRequestId);

                    // Send to Dashboard.
                    return RedirectToAction("Dashboard");
                }
            }

            // Not Created.
            TempData["RepairNotice"] = "Scan Request NOT Created! (Do you have an open scan request?)";

            PopulateRequestScanActionViewBag(scanRequest.OrderId);

            return View("RequestScan", scanRequest);
        }

        [HttpPost]
        public async Task<ActionResult> GetRequestTypes(string id)
        {
            var cachedCategoryItems = SelectListItemCache.RequestTypeCategorySelectItems().ToList();
            var filteredCategoryItems = cachedCategoryItems.Where(d => d.Group.Name == id).ToList();
            if (!filteredCategoryItems.Any())
            {
                filteredCategoryItems = new List<SelectListItem>();
                filteredCategoryItems.Add(new SelectListItem
                {
                    Value = "0",
                    Text = "Other",
                    Group = new SelectListGroup
                    {
                        Name = "0"
                    }
                });
            }

            var mappedCategories = filteredCategoryItems.GetWithoutGroups();

            return new JsonCamelCaseResult(mappedCategories);
        }

        [HttpGet]
        public async Task<ActionResult> Clone(string id)
        {
            var repair = await Factory.GetByIdAsync<IRepairDto>(id);
            repair.RepairId = 0;

            Factory.Save(repair);

            return RedirectToAction(nameof(Dashboard));
        }

        #region File Downloads

        [AuthorizeRoles(ApplicationRoles.RepairCreate, ApplicationRoles.RepairEdit, ApplicationRoles.RepairView)]
        public ActionResult ScanReport(int id)
        {
            return RedirectToAction("ScanReport", "Download", new { id });
        }

        [AuthorizeRoles(ApplicationRoles.RepairCreate, ApplicationRoles.RepairEdit, ApplicationRoles.RepairView)]
        public ActionResult Invoice(int id)
        {
            return RedirectToAction("Invoice", "Download", new { id });
        }

        #endregion

        #region Scan Analysis

        [AuthorizeRoles(ApplicationRoles.RepairCreate)]
        public ActionResult StartScanAnalysis(Guid id)
        {
            // Build View Model.
            var scanAnalysis = new ScanAnalysisViewModel
            {
                ShopGuid = id,
                ShopName = Factory.GetDisplayName<IShopDto>(id.ToString()),
                VehicleSelectList = GetVehicleMakeSelectList(),
                AvailableScans = Factory.GetAll<IDiagnosticQueueDto>(new ServiceArgs { { "ShopGuid", id.ToString() } }).ToList()
            };

            // Set Default.
            var defaultInsuranceCompanyId = Db.Shops.Find(id)?.DefaultInsuranceCompanyId;
            if (defaultInsuranceCompanyId.HasValue)
                scanAnalysis.InsuranceCompanyId = defaultInsuranceCompanyId.Value;

            return PartialView("_ScanAnalysisCreate", scanAnalysis);
        }

        [AuthorizeRoles(ApplicationRoles.RepairCreate)]
        public async Task<ActionResult> ScanAnalysisUpload(Guid id)
        {
            // Create Response.
            var response = new ScanAnalysisViewModel
            {
                ShopGuid = id,
                VehicleSelectList = GetVehicleMakeSelectList()
            };

            // Check Upload Files.
            if (Request?.Files?[0] == null) return Json(response);

            // Parse VIN Number.
            var diagnosticResult = DiagnosticFileParser
                .ParseFile(Request.Files[0].InputStream, DiagnosticTool.AutoEnginuity, DiagnosticFileType.XML);
            var vin = diagnosticResult?.VehicleVin;

            // Check VIN Number.
            if (string.IsNullOrEmpty(vin) || vin == "Not Reported") return Json(response);

            response.ShowAssistedScanRecommended = NeedShowAssistedScanRecommended(diagnosticResult);

            await QuickRequestSearch(id, response, vin);

            return Json(response);
        }

        [AuthorizeRoles(ApplicationRoles.RepairCreate)]
        public async Task<ActionResult> ScanAnalysisUploadSelect(Guid id, int resultId)
        {
            // Create Response.
            var response = new ScanAnalysisViewModel
            {
                ShopGuid = id,
                DiagnosticResultId = resultId,
                VehicleSelectList = GetVehicleMakeSelectList()
            };

            // Lookup VIN for Result.
            var diagnosticResult = Factory.GetById<IDiagnosticResultDto>(resultId.ToString());
            var vin = diagnosticResult?.VehicleVin;

            // Check VIN Number.
            if (string.IsNullOrEmpty(vin) || vin == "Not Reported") return Json(response);

            response.ShowAssistedScanRecommended = NeedShowAssistedScanRecommended(diagnosticResult);

            await QuickRequestSearch(id, response, vin);

            return Json(response);
        }

        [AuthorizeRoles(ApplicationRoles.RepairCreate)]
        public async Task<ActionResult> ScanAnalysisVehicleSearch(Guid id, string vin)
        {
            var response = new ScanAnalysisViewModel();

            await QuickRequestSearch(id, response, vin);

            return Json(response);
        }

        [AuthorizeRoles(ApplicationRoles.RepairCreate)]
        public async Task<ActionResult> ScanAnalysisCreate(ScanAnalysisViewModel scanAnalysis)
        {
            try
            {
                // Create Request.
                var request = new RepairRequestScanViewModel
                {
                    RequestTypeID = 7,
                    RequestTypeCategoryId = 1,
                    ProblemDescription = "Scan Analysis"
                };

                // Create.
                var newRequestId = await QuickRequestSave(scanAnalysis, request);

                // Update Scan Queue.
                var messenger = new ScanRequestHubMessenger();
                await messenger.NotifyScanCreated(newRequestId);
            }
            catch (Exception e)
            {
                Logger.LogException(e);

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, e.Message);
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        #endregion

        #region Self Scan

        [AuthorizeRoles(ApplicationRoles.RepairCreate)]
        public ActionResult StartSelfScan(Guid id)
        {
            // Load Model.
            var cachedCategoryItems = SelectListItemCache.RequestTypeCategorySelectItems().ToList();
            var filteredCategoryItems = cachedCategoryItems.Where(d => d.Group.Name == "6").ToList();
            if (!filteredCategoryItems.Any())
            {
                filteredCategoryItems = cachedCategoryItems.Where(d => d.Group.Name == "0").ToList();
            }

            var mappedCategories = filteredCategoryItems.GetWithoutGroups();

            var selfScan = new SelfScanViewModel
            {
                ShopGuid = id,
                ShopName = Factory.GetDisplayName<IShopDto>(id.ToString()),
                VehicleSelectList = GetVehicleMakeSelectList(),
                CategoryTypesSelectList = mappedCategories,
                AvailableScans = Factory.GetAll<IDiagnosticQueueDto>(new ServiceArgs { { "ShopGuid", id.ToString() } }).ToList()
            };

            return PartialView("_SelfScanCreate", selfScan);
        }

        [AuthorizeRoles(ApplicationRoles.RepairCreate)]
        public async Task<ActionResult> SelfScanUpload(Guid id)
        {
            // Create Response.
            var response = new SelfScanViewModel
            {
                ShopGuid = id,
                VehicleSelectList = GetVehicleMakeSelectList()
            };

            // Check Upload Files.
            if (Request?.Files?[0] == null) return Json(response);

            // Parse VIN Number.
            var diagnosticResult = DiagnosticFileParser.ParseFile(Request.Files[0].InputStream, DiagnosticTool.AutoEnginuity, DiagnosticFileType.XML);
            var vin = diagnosticResult?.VehicleVin;

            // Check VIN Number.
            if (string.IsNullOrEmpty(vin) || vin == "Not Reported") return Json(response);

            response.ShowAssistedScanRecommended = NeedShowAssistedScanRecommended(diagnosticResult);

            await QuickRequestSearch(id, response, vin);

            return Json(response);
        }

        [AuthorizeRoles(ApplicationRoles.RepairCreate)]
        public async Task<ActionResult> SelfScanUploadSelect(Guid id, int resultId)
        {
            // Create Response.
            var response = new SelfScanViewModel
            {
                ShopGuid = id,
                DiagnosticResultId = resultId,
                VehicleSelectList = GetVehicleMakeSelectList()
            };

            // Lookup VIN for Result.
            var diagnosticResult = Factory.GetById<IDiagnosticResultDto>(resultId.ToString());
            var vin = diagnosticResult?.VehicleVin;

            // Check VIN Number.
            if (string.IsNullOrEmpty(vin) || vin == "Not Reported") return Json(response);

            response.ShowAssistedScanRecommended = NeedShowAssistedScanRecommended(diagnosticResult);

            await QuickRequestSearch(id, response, vin);

            return Json(response);
        }

        private static bool NeedShowAssistedScanRecommended(IDiagnosticResultDto diagnosticResult) => diagnosticResult.Controllers.Any()
                                                                                              && diagnosticResult.Controllers.Any(x => x.ControllerName.Contains("Generic Powertrain"))
                                                                                              && diagnosticResult.Controllers.All(c => !c.ControllerName.Contains("Enhanced Powertrain"));

        [AuthorizeRoles(ApplicationRoles.RepairCreate)]
        public async Task<ActionResult> SelfScanVehicleSearch(Guid id, string vin)
        {
            var response = new SelfScanViewModel();

            await QuickRequestSearch(id, response, vin);

            return Json(response);
        }

        [AuthorizeRoles(ApplicationRoles.RepairCreate)]
        public async Task<ActionResult> SelfScanCreate(SelfScanViewModel selfScan)
        {
            try
            {
                // Create Request.
                var request = new RepairRequestScanViewModel
                {
                    RequestTypeID = 6,
                    ProblemDescription = "Self Scan",
                    RequestTypeCategoryId = selfScan.CategoryId,
                    SeatRemovedInd = selfScan.SeatRemovedInd
                };

                // Create Request.
                var requestId = await QuickRequestSave(selfScan, request);

                // Load Report.
                var report = Factory.GetById<IReportDto>(requestId.ToString());

                // Generate Report Html.
                if (report != null && selfScan.DiagnosticResultId.HasValue)
                {
                    // Set Diagnostic.
                    report.DiagnosticResultSelections = new List<IReportDiagnosticResultSelectionItemDto>
                    {
                        new ReportDiagnosticResultSelectionItemViewModel
                        {
                            ResultId = selfScan.DiagnosticResultId.Value,
                            AssignedToRequestInd = true,
                            SelectedForReportInd = true
                        }
                    };

                    // Set User.
                    report.ResponsibleTechUserId = Factory.User.UserGuid;

                    // Set Header.
                    report.ReportHeaderHTML = "Self Scan Report<br/>";

                    if (selfScan.IsAssistedScanRecommended)
                    {
                        report.ReportHeaderHTML += "<b style='color:red;'>All modules were not read on this Vehicle, recommend an Assisted Scan be performed.</b><br/>";
                    }

                    // Complete Report.
                    report.CompleteReport = true;

                    // Save Report.
                    Factory.Save(report);

                    // Create Notification Tasks.
                    var notifications = new List<Task>();

                    // Generate Notifications.
                    using (var queue = new MessageQueue())
                    {
                        notifications.Add(queue.AddNotificationQueueMessageAsync(NotificationTemplate.ShopReportEmail, report.RequestId, Factory.User.UserGuid));
                        notifications.Add(queue.AddMitchellReportQueueMessageAsync(report.RequestId, Factory.User.UserGuid));
                    }

                    // Open Connection.
                    using (var conn = new SqlConnection(MvcApplication.ConnectionString))
                    {
                        // Get Repair Id.
                        var repairId = await conn.QueryFirstOrDefaultAsync<int>("Repair.usp_CloseRepairByRequestId",
                            new { report.RequestId }, null, null, CommandType.StoredProcedure);

                        // Check Repair.
                        if (repairId > 0)
                        {
                            // Close Repair.
                            if (await Factory.DeleteAsync<IRepairDto>(repairId.ToString()))
                            {
                                // Send Notifications.
                                var clientMessenger = new ClientHubMessenger();
                                notifications.Add(clientMessenger.RepairCompleted());
                            }
                        }
                    }

                    // Execute Notifications.
                    await Task.WhenAll(notifications.ToArray());
                }
            }
            catch (Exception e)
            {
                Logger.LogException(e);

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, e.Message);
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        #endregion

        #region Support Methods

        private async Task<int> QuickRequestSave(IQuickRequest quickRequest, RepairRequestScanViewModel repairRequest)
        {
            var requestId = 0;

            try
            {
                // Check Vehicle Entry.
                if (quickRequest.VehicleMakeId > 0)
                {
                    // Create Vehicle Model.
                    var vehicle = new VehicleViewModel
                    {
                        VehicleVIN = quickRequest.VehicleVIN,
                        VehicleMakeId = quickRequest.VehicleMakeId,
                        VehicleModel = quickRequest.VehicleModel,
                        VehicleYear = quickRequest.VehicleYear,
                        VehicleTransmission = quickRequest.VehicleTransmission
                    };

                    // Update Vehicle.
                    Factory.Save((IVehicleDto)vehicle);
                }

                // Check Repair.
                OrderEntityModel repair;
                var repairs = Db.RepairOrders.Where(o => o.ShopGuid == quickRequest.ShopGuid && o.VehicleVIN == quickRequest.VehicleVIN && o.Status == RepairStatuses.Active).ToList();
                if (repairs.Count == 1)
                {
                    // Load Repair.
                    repair = repairs.First();

                    await RepairLib.UpdateRepairOrder(new RepairEditViewModel
                    {
                        ShopGuid = quickRequest.ShopGuid,
                        RepairOrderID = repair.OrderId,
                        ShopReferenceNumber = quickRequest.ShopReferenceNumber,
                        InsuranceCompany = new InsuranceCompanyEntityModel
                        {
                            InsuranceCompanyId = quickRequest.InsuranceCompanyId
                        },
                        InsuranceCompanyOther = quickRequest.InsuranceCompanyOther,
                        Odometer = quickRequest.Odometer,
                        AirBagsDeployed = quickRequest.AirBagsDeployed,
                        AirBagsVisualDeployments = quickRequest.AirBagsVisualDeployments,
                        DrivableInd = quickRequest.DrivableInd,
                        ImpactPoints = quickRequest.ImpactPoints
                    });
                }
                else
                {
                    // Create Repair.
                    repair = new OrderEntityModel
                    {
                        ShopGuid = quickRequest.ShopGuid,
                        VehicleVIN = quickRequest.VehicleVIN,
                        ShopReferenceNumber = quickRequest.ShopReferenceNumber,
                        InsuranceCompanyId = quickRequest.InsuranceCompanyId,
                        InsuranceCompanyOther = quickRequest.InsuranceCompanyOther,
                        Odometer = quickRequest.Odometer,
                        AirBagsDeployed = quickRequest.AirBagsDeployed,
                        AirBagsVisualDeployments = quickRequest.AirBagsVisualDeployments,
                        DrivableInd = quickRequest.DrivableInd,
                        ImpactPoints = quickRequest.ImpactPoints
                    };
                    await RepairLib.CreateRepairOrder(repair);
                }

            // Update Request.
            repairRequest.OrderId = repair.OrderId;
            repairRequest.ContactOtherFirstName = quickRequest.ContactOtherFirstName;
            repairRequest.ContactOtherLastName = quickRequest.ContactOtherLastName;
            repairRequest.ContactOtherPhone = quickRequest.ContactOtherPhone;
            repairRequest.ContactUserGuid = quickRequest.ContactUserGuid;
            if (repairRequest.RequestTypeID == 0)
                repairRequest.RequestTypeID = 6; // Self Scan if Null.
            if (repairRequest.RequestTypeCategoryId == 0)
                repairRequest.RequestTypeCategoryId = 1; // Pre Request if Null.

                // Save Request.
                requestId = await RepairLib.CreateScanRequest(repairRequest);

                // Check Result Id.
                if (quickRequest.DiagnosticResultId.HasValue)
                {
                    // Load Diagnostic.
                    var diag = Factory.GetById<IDiagnosticResultDto>(quickRequest.DiagnosticResultId.Value.ToString());

                    // Create Update.
                    diag.RequestId = requestId;

                    // Save Update.
                    Factory.Save(diag);
                }
                else
                {
                    // Upload File.
                    if (Request?.Files[0] != null)
                    {
                        // Parse Upload File.
                        var diag = DiagnosticFileParser.ParseFile(Request.Files[0].InputStream,
                            DiagnosticTool.AutoEnginuity,
                            DiagnosticFileType.XML);

                        // Assign Request.
                        diag.RequestId = requestId;

                        // Save Result.
                        diag = Factory.Save(diag);

                        // Update Result Id.
                        quickRequest.DiagnosticResultId = diag.ResultId;
                    }
                }
            }
            catch (Exception e)
            {
                Logger.LogException(e, new { quickRequest, repairRequest });
            }

            return requestId;
        }

        private async Task QuickRequestSearch(Guid shopGuid, IQuickRequest quickRequest, string vin)
        {
            // Lookup Vehicle.
            var vehicle = Mapper.Map<VehicleViewModel>(await Factory.GetByIdAsync<IVehicleDto>(vin));

            // Check Vehicle.
            if (vehicle != null)
            {
                quickRequest.VehicleFound = !vehicle.ManualEntryInd;
                quickRequest.VehicleVIN = vehicle.VehicleVIN;
                quickRequest.VehicleMakeId = vehicle.VehicleMakeId;
                quickRequest.VehicleModel = vehicle.VehicleModel;
                quickRequest.VehicleYear = vehicle.VehicleYear;
                quickRequest.VehicleTransmission = vehicle.VehicleTransmission;
                quickRequest.VehicleSelectList = GetVehicleMakeSelectList();
            }

            // Check Repair.
            var repairs = Db.RepairOrders.Where(o => o.ShopGuid == shopGuid && o.VehicleVIN == vin && o.Status == RepairStatuses.Active).ToList();
            if (repairs.Count != 1) return;

            var repair = repairs[0];
            quickRequest.RepairFound = true;
            quickRequest.CccSourceInd = repairs.Any(d => d.CCCDocumentGuid.HasValue);
            quickRequest.RepairOrderId = repair.OrderId;
            quickRequest.ShopReferenceNumber = repair.ShopReferenceNumber;
            quickRequest.InsuranceCompanyId = repair.InsuranceCompanyId;
            quickRequest.InsuranceCompanyOther = repair.InsuranceCompanyOther;
            quickRequest.Odometer = repair.Odometer;
            quickRequest.AirBagsDeployed = repair.AirBagsDeployed;
            quickRequest.AirBagsVisualDeployments = repair.AirBagsVisualDeployments;
            quickRequest.DrivableInd = repair.DrivableInd;
            quickRequest.ImpactPoints = repair.PointsOfImpact.Select(d => d.PointOfImpactId).ToList();

            var scanRequest = repair.ScanRequests.OrderByDescending(x => x.RequestId).FirstOrDefault();
            if (scanRequest == null) return;

            quickRequest.ContactOther = scanRequest.Contact;
            quickRequest.ContactUserGuid = scanRequest.ContactUserGuid?.ToString();
        }

        #endregion

        #region RepairV2

        [HttpPost]
        [AuthorizeRoles(ApplicationRoles.RepairCreate, ApplicationRoles.RepairEdit, ApplicationRoles.RepairView)]
        public async Task<ActionResult> GetRepairsByPage(int current, int rowCount, string searchPhrase, string statusFilter, string shopGuid)
        {
            // Load Arguments.
            var args = new ServiceArgs();
            args.AddGridOptions(current, rowCount, Request.Form.GetDynamicSortString(), searchPhrase);
            args.Add("StatusFilter", Enum.TryParse(statusFilter, out RepairStatuses status) ? status : (object)null);
            args.Add("ShopGuid", Guid.TryParse(shopGuid, out Guid shop) ? shop : Guid.Empty);

            // Load Grid Page.
            var repairs = await Factory.GetAllByGridPageAsync<IRepairDto>(args);

            // Return Result.
            return new JsonCamelCaseResult(repairs, JsonRequestBehavior.DenyGet);
        }

        [AuthorizeRoles(ApplicationRoles.RepairCreate)]
        public ActionResult CreateRepair()
        {
            return PartialView("_Create", new CreateRepairViewModel
            {
                Vehicle = new VehicleViewModel(),
                RepairOrder = new RepairViewModel(),
                VehicleSelectList = GetVehicleMakeSelectList(),
                ShopSelectListItems = ShopSelectListItems
            });
        }

        [HttpPost]
        [AuthorizeRoles(ApplicationRoles.RepairCreate, ApplicationRoles.RepairEdit)]
        public async Task<JsonResult> EditRepair(int id, bool editMode)
        {
            var repairDto = await Factory.GetByIdAsync<IRepairDto>(id.ToString());
            var vehicleDto = await Factory.GetByIdAsync<IVehicleDto>(repairDto.VehicleVIN);

            var repairViewModel = Mapper.Map<RepairViewModel>(repairDto);
            var vehicleViewModel = Mapper.Map<VehicleViewModel>(vehicleDto);

            var vm = new CreateRepairViewModel
            {
                Vehicle = vehicleViewModel,
                RepairOrder = repairViewModel,
                VehicleSelectList = GetVehicleMakeSelectList(),
                ShopSelectListItems = ShopSelectListItems,
                ReadOnly = editMode == false
            };

            return Json(vm);
        }

        [HttpPost]
        [AuthorizeRoles(ApplicationRoles.RepairCreate)]
        public async Task<JsonResult> CreateRepair(CreateRepairViewModel model)
        {
            // Update/Load Vehicle.
            if (model.Vehicle.ManualEntryInd)
            {
                Factory.Save(model.Vehicle as IVehicleDto);
            }

            var repairDto = Mapper.Map<IRepairDto>(model.RepairOrder);
            var result = Factory.Save(repairDto);

            if (model.RepairOrder.RepairId != 0 || !result.UpdateResult.Success) return Json(result);

            var requestUrl = Url.Action("Dashboard", "Repairs", null, Request?.Url?.Scheme);
            await
                new ClientHubMessenger().RepairCreated(result.RepairId, requestUrl)
                    .ConfigureAwait(continueOnCapturedContext: false);

            return Json(result);
        }

        [HttpPost]
        [AuthorizeRoles(ApplicationRoles.RepairCreate, ApplicationRoles.RepairEdit)]
        public async Task<JsonResult> VehicleVinSearch(string vin)
        {
            var vehicle = Mapper.Map<VehicleViewModel>(await Factory.GetByIdAsync<IVehicleDto>(vin));

            return Json(vehicle);
        }

        [AuthorizeRoles(ApplicationRoles.RepairCreate, ApplicationRoles.RepairEdit)]
        public async Task<JsonResult> CompleteRepair(int id, string status, FeedbackViewModel feedbackVm)
        {
            var result = await Factory.DeleteAsync<IRepairDto>(id.ToString());
            if (result && feedbackVm != null)
            {
                Factory.Save<IFeedbackDto>(feedbackVm);

                if (!string.IsNullOrWhiteSpace(feedbackVm.AdditionalFeedback) ||
                    feedbackVm.ConcernsAddressedRate < 5 ||
                    feedbackVm.ReportCompletionRate < 5 ||
                    feedbackVm.ReportCompletionRate < 5 ||
                    feedbackVm.ResponseTimeRate < 5 ||
                    feedbackVm.TechnicianCommunicationRate < 5 ||
                    feedbackVm.TechnicianKnowledgeRate < 5)
                {
                    using (var queue = new MessageQueue())
                    {
                        var userGuid = Guid.Parse(User.Identity.GetUserId());
                        await queue.AddNotificationQueueMessageAsync(template: NotificationTemplate.RepairFeedbackEmail, id: feedbackVm.RepairId, userGuid: userGuid);
                    }
                }
            }

            if (!result)
            {
                return Json(result);
            }

            if (status == "complete")
            {
                string billingUrl = Url.Action("Invoice", "Invoicing", new { Area = "Billing", ID = id }, Request?.Url?.Scheme);
                var task = new ClientHubMessenger().RepairCompleted(id, billingUrl);

                Task.WhenAll(task);
            }

            return Json(new
            {
                message = "Repair Updated Successfully.",
                success = true
            });
        }

        [HttpGet]
        public JsonResult ValidateRepairVin(string vin, Guid shopGuid)
        {
            var lib = new RepairLibrary(Db, User.Identity);
            var exists = lib.FindActiveRepairsByVinAndShop(vin, shopGuid);

            return Json(new
            {
                Exists = exists
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> ValidateShop(Guid shopGuid)
        {
            var dto = await Factory.GetByIdAsync<IRepairCreateValidateShopDto>(shopGuid.ToString());
            return Json(new { dto.CanCreateRequest }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}