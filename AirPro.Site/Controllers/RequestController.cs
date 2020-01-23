using AirPro.Common.Enumerations;
using AirPro.Logging;
using AirPro.Parser;
using AirPro.Service;
using AirPro.Service.DTOs.Interface;
using AirPro.Site.Attributes;
using AirPro.Site.Hubs;
using AirPro.Site.Models.Request;
using AirPro.Site.Models.Shared;
using AirPro.Site.Results;
using AutoMapper;
using Dapper;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AirPro.Storage;
using UniMatrix.Common.Extensions;

namespace AirPro.Site.Controllers
{
    [AuthorizeRoles(ApplicationRoles.Technician, ApplicationRoles.ReportCreate, ApplicationRoles.ReportEdit, ApplicationRoles.ReportView)]
    public class RequestController : BaseController
    {
        private static List<SelectListItem> _diagnosticTools;
        private List<SelectListItem> DiagnosticTools => _diagnosticTools ?? (_diagnosticTools = Factory.GetAll<IDiagnosticToolDto>()
                                                            .Select(tool => new SelectListItem
                                                            {
                                                                Value = tool.DiagnosticToolId.ToString(),
                                                                Text = tool.DiagnosticToolName
                                                            }).ToList());

        public ActionResult Index()
        {
            return RedirectToAction("Queue");
        }

        #region Dashboard & Queue

        public ActionResult Dashboard()
        {
            return View("Dashboard");
        }

        public async Task<ActionResult> Queue()
        {
            var scanRequests = await Factory.GetAllAsync<IRequestDto>();
            var model = scanRequests.Select(Mapper.Map<RequestViewModel>).ToList();

            var userTimezone = TimeZoneInfo.FindSystemTimeZoneById(Factory.User.TimeZoneInfoId);
            var offset = userTimezone.BaseUtcOffset;
            ViewBag.UserOffset = userTimezone.IsDaylightSavingTime(DateTime.Now) ? offset.Add(TimeSpan.FromHours(1)) : offset;

            return View(model);
        }

        [HttpPost]
        public async Task<JsonCamelCaseResult> GetRequestsByPage(int current, int rowCount, string searchPhrase, string completedInd)
        {
            // Load Arguments.
            var args = new ServiceArgs();
            args.AddGridOptions(current, rowCount, Request.Form.GetDynamicSortString(), searchPhrase);
            args.Add("CompletedInd", bool.TryParse(completedInd, out var completed) && completed);

            // Load Requests.
            var requests = await Factory.GetAllByGridPageAsync<IRequestDto>(args);

            // Return Requests.
            return new JsonCamelCaseResult(requests);
        }

        #endregion

        #region Scan Report 

        [HttpGet]
        public async Task<ActionResult> Report(int? id)
        {
            // Get Return URL.
            var returnUrl = (Request.UrlReferrer?.AbsolutePath.Contains("Request/Dashboard") ?? false)
                ? Url.Action("Dashboard") : Url.Action("Queue");

            // Check Request Id.
            if (!id.HasValue) return Redirect(returnUrl);

            // Load Request.
            var request = Mapper.Map<RequestViewModel>(await Factory.GetByIdAsync<IRequestDto>(id.Value.ToString()));

            // Check Request.
            if (request == null) return new HttpNotFoundResult();

            // Set Return URL.
            request.ReturnUrl = returnUrl;

            // Load Diagnostic Tools.
            ViewBag.DiagnosticToolSelection = DiagnosticTools;

            // Return View.
            return View(request);
        }

        [HttpPost]
        public JsonCamelCaseResult LoadScanReport(int id)
        {
            // Load Report.
            var report = Mapper.Map<ReportViewModel>(Factory.GetById<IReportDto>(id.ToString()));

            // Return Report.
            return new JsonCamelCaseResult(report);
        }

        [HttpPost]
        public async Task<JsonCamelCaseResult> SaveScanReport(ReportViewModel report)
        {
            try
            {
                // Save Report.
                var update = Factory.Save(Mapper.Map<IReportDto>(report));

                // Check for Failure.
                if (update.UpdateResult?.Success != true) return new JsonCamelCaseResult(update);

                // Load Messengers.
                var clientMessenger = new ClientHubMessenger();
                var requestMessenger = new ScanRequestHubMessenger();

                // Notify Listeners.
                await (requestMessenger.NotifyScanUpdated(update.RequestId));
                await (clientMessenger.ScanRequestOutDated(Factory.User.UserGuid, update.RequestId));

                // Check for Completed.
                if (!update.CompleteReport) return new JsonCamelCaseResult(Mapper.Map<ReportViewModel>(update));

                // Notify Listeners.
                await (clientMessenger.ScanRequestCompleted());
                await (requestMessenger.NotifyScanRemoved(update.RequestId));

                // Load Notification Queue.
                var queue = new MessageQueue();

                // Check for Not Cancel.
                if (!update.CancelReport && report.UserCompletedInd)
                {
                    // Send Notifications.
                    await (queue.AddNotificationQueueMessageAsync(NotificationTemplate.ShopReportEmail, update.RequestId,
                        Factory.User.UserGuid));

                    // Send Completed Report to Mitchell.
                    await (queue.AddMitchellReportQueueMessageAsync(update.RequestId, Factory.User.UserGuid));

                    // Check for Auto Close Bypass.
                    if (!report.AutoRepairCloseBypass)
                    {
                        // Run Auto Close Logic.
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
                                    // Update Repair Complete.
                                    update.AllowEdit = false;
                                    update.RepairComplete = true;

                                    // Send Notifications.
                                    await (clientMessenger.RepairCompleted());
                                }
                            }
                        }
                    }
                }

                // Check for Cancelled.
                if (update.CancelReport)
                {
                    // Look for Cancel Reason Template.
                    var cancelReason = await Factory.GetByIdAsync<ICancelReasonTypeDto>(update.CancelReasonTypeId.ToString());
                    if (cancelReason?.NotificationTemplate != null)
                    {
                        // Send Notification.
                        await (queue.AddNotificationQueueMessageAsync(cancelReason.NotificationTemplate.Value, report.RequestId, Factory.User.UserGuid));
                    }
                }

                // Return Data.
                return new JsonCamelCaseResult(Mapper.Map<ReportViewModel>(update));
            }
            catch (Exception e)
            {
                // Log Exception.
                Logger.LogException(e);

                // Create Result.
                report.UpdateResult = new UpdateResultViewModel(false, e.Message);

                // Return Data.
                return new JsonCamelCaseResult(report);
            }
        }

        #endregion

        #region Report Searches

        [HttpPost]
        public async Task<JsonCamelCaseResult> SearchDecisions(int requestId, string search)
        {
            // Search Decisions.
            using (var conn = new SqlConnection(MvcApplication.ConnectionString))
            {
                var decisions = await conn.QueryAsync<ReportDecisionSelectionItemViewModel>("Scan.usp_GetDecisionsRanked @UserGuid, @RequestId, @SearchPhrase",
                    new
                    {
                        UserGuid = User.Identity.GetUserId(),
                        RequestId = requestId,
                        SearchPhrase = search
                    });

                return new JsonCamelCaseResult(decisions);
            }
        }

        [HttpPost]
        public async Task<JsonCamelCaseResult> SearchControllers(string search)
        {
            // Search Controllers.
            using (var conn = new SqlConnection(MvcApplication.ConnectionString))
            {
                var controllers = await conn.QueryAsync("Diagnostic.usp_GetControllersSearch @SearchPhrase",
                    new { SearchPhrase = search });

                return new JsonCamelCaseResult(controllers);
            }
        }

        [HttpPost]
        public async Task<JsonCamelCaseResult> SearchTroubleCodes(string search)
        {
            // Search Controllers.
            using (var conn = new SqlConnection(MvcApplication.ConnectionString))
            {
                var codes = await conn.QueryAsync("Diagnostic.usp_GetTroubleCodesSearch @SearchPhrase",
                    new { SearchPhrase = search });

                return new JsonCamelCaseResult(codes);
            }
        }

        [HttpPost]
        public async Task<JsonCamelCaseResult> SearchRecommendations(string search)
        {
            // Search Recommendations.
            using (var conn = new SqlConnection(MvcApplication.ConnectionString))
            {
                var controllers = await conn.QueryAsync("Scan.usp_GetRecommendationSearch @SearchPhrase",
                    new { SearchPhrase = search });

                return new JsonCamelCaseResult(controllers);
            }
        }

        #endregion

        #region Diagnostic Results

        [HttpGet]
        public ActionResult LoadDiagnosticResults(int id)
        {
            // Load Results.
            var model = Mapper.Map<DiagnosticResultViewModel>(Factory.GetById<IDiagnosticResultDto>(id.ToString()));

            // Return Partial.
            return PartialView("_Diagnostics", model);
        }

        [HttpPost]
        public JsonCamelCaseResult DeleteDiagnosticResult(int resultId)
        {
            return new JsonCamelCaseResult(Factory.Delete<IDiagnosticResultDto>(resultId.ToString()));
        }

        [HttpPost]
        public async Task<JsonCamelCaseResult> UploadDiagnosticResult(HttpPostedFileBase file, int requestId, int diagnosticToolId)
        {
            try
            {
                // Check File.
                if (file == null || file.ContentLength <= 0) throw new Exception("No File Found.");

                DiagnosticTool diagnosticTool =
                    (DiagnosticTool)Enum.Parse(typeof(DiagnosticTool), diagnosticToolId.ToString());

                // Parse Upload File.
                var diag = DiagnosticFileParser.ParseFile(file.InputStream, diagnosticTool, DiagnosticFileType.XML);

                // Assign Request.
                diag.RequestId = requestId;

                // Save Result.
                var result = Factory.Save(diag);

                // Notify Clients.
                var messenger = new ScanRequestHubMessenger();
                await messenger.NotifyScanUpdated(requestId);

                // Return Update Result.
                return new JsonCamelCaseResult(result.UpdateResult);
            }
            catch (Exception ex)
            {
                // Log Error.
                Logger.LogException(ex);

                // Prepare Result.
                var error = new
                {
                    Success = false,
                    Message = ex.Message
                };

                // Return Error Result.
                return new JsonCamelCaseResult(error);
            }
        }

        #endregion
    }
}