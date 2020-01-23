using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AirPro.Logging;
using AirPro.Service.DTOs.Concrete;
using AirPro.Service.DTOs.Interface;
using AirPro.Service.Services.Abstract;
using AirPro.Service.Services.Interface;
using Dapper;
using Newtonsoft.Json;

namespace AirPro.Service.Services.Concrete
{
    internal class ReportService : ServiceBase, IService<IReportDto>
    {
        public ReportService(IServiceSettings settings) : base(settings)
        {
        }

        public IReportDto GetById(string id)
        {
            return Task.Run(async () => await GetByIdAsync(id)).Result;
        }

        public async Task<IReportDto> GetByIdAsync(string id)
        {
            // Trouble Code Mapping.
            var troubleCodeDictionary = new Dictionary<long, ReportTroubleCodeDto>();
            ReportTroubleCodeDto TroubleCodeMapping(ReportTroubleCodeDto tc, ReportTroubleCodeRecommendationDto tcr)
            {
                if (!troubleCodeDictionary.TryGetValue(tc.ReportOrderTroubleCodeId, out var code))
                {
                    code = tc;
                    code.Recommendations = new List<ReportTroubleCodeRecommendationDto>();
                    troubleCodeDictionary.Add(tc.ReportOrderTroubleCodeId, code);
                }

                if (tcr != null) (code.Recommendations as List<ReportTroubleCodeRecommendationDto>)?.Add(tcr);

                return code;
            }

            // Load Scan Request.
            ReportDto result;
            using (var x = await Conn.QueryMultipleAsync("Scan.usp_GetReportByRequestId",
                new { UserGuid = User.UserGuid, RequestId = id }, null, null, CommandType.StoredProcedure))
            {
                result = x.Read<ReportDto>().First();
                result.RequestTypeSelections = x.Read<ReportRequestTypeSelectionItemDto>().ToList();
                result.AirProToolSelections = x.Read<ReportAirProToolSelectionItemDto>().ToList();
                result.ResponsibilityHistory = x.Read<ReportResponsibilityHistoryListItemDto>().ToList();
                result.InternalNoteHistory = x.Read<ReportInternalNoteHistoryListItemDto>().ToList();
                result.WorkTypeSelections = x.Read<ReportWorkTypeSelectionItemDto>().ToList();
                result.DecisionSelections = x.Read<ReportDecisionSelectionItemDto>().ToList();
                result.DiagnosticResultSelections = x.Read<ReportDiagnosticResultSelectionItemDto>().ToList();
                result.FrequentRecommendationSelections = x.Read<ReportFrequentRecommendationSelectionItemDto>().ToList();
                result.TroubleCodeRecommendations = x.Read<ReportTroubleCodeDto, ReportTroubleCodeRecommendationDto, ReportTroubleCodeDto>(TroubleCodeMapping, splitOn: "RequestId", buffered: false).Distinct().ToList();
                result.PossibleMissingControllers = x.Read<ReportPossibleMissingControllerDto>().ToList();
                result.CancelReasonTypes = x.Read<CancelReasonTypeDto>().ToList();
                result.VehicleMakeTools = x.Read<ReportVehicleMakeToolDto>().ToList();
                result.ValidationRules = x.Read<ReportValidationRuleDto>().ToList();
            }

            // Return.
            return result;
        }

        public string GetDisplayName(string id)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetDisplayNameAsync(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<KeyValuePair<string, string>> GetDisplayList(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<KeyValuePair<string, string>>> GetDisplayListAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IReportDto> GetAll(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<IReportDto>> GetAllAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IGridPageDto<IReportDto> GetAllByGridPage(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public Task<IGridPageDto<IReportDto>> GetAllByGridPageAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IGridPageDto<IReportDto> GetAllByGridPage(int pageNumber, int pageSize, string sort, string searchPhrase)
        {
            throw new NotImplementedException();
        }

        public IReportDto Save(IReportDto update)
        {
            UpdateResultDto result;
            var requestId = update?.RequestId ?? 0;

            try
            {
                // Check Update.
                if (update == null) throw new NullReferenceException("Update can NOT be NULL.");

                // Create Decisions.
                var decisions = new DataTable();
                decisions.Columns.Add("DecisionId", typeof(int));
                decisions.Columns.Add("DecisionText", typeof(string));
                decisions.Columns.Add("DecisionTextSeverity", typeof(int));

                // Load Decisions.
                foreach (var decision in update.DecisionSelections)
                {
                    if (decision.DecisionSelected)
                        decisions.Rows.Add(decision.DecisionId, decision.DecisionText, (int)decision.DecisionTextSeverity);
                }

                // Create Recommendations.
                var recommendations = new DataTable();
                recommendations.Columns.Add("ReportOrderTroubleCodeId", typeof(long));
                recommendations.Columns.Add("ControllerId", typeof(int));
                recommendations.Columns.Add("ControllerIdOrig", typeof(int));
                recommendations.Columns.Add("ControllerName", typeof(string));
                recommendations.Columns.Add("ControllerNameOrig", typeof(string));
                recommendations.Columns.Add("TroubleCodeId", typeof(int));
                recommendations.Columns.Add("TroubleCodeIdOrig", typeof(int));
                recommendations.Columns.Add("TroubleCode", typeof(string));
                recommendations.Columns.Add("TroubleCodeOrig", typeof(string));
                recommendations.Columns.Add("TroubleCodeDescription", typeof(string));
                recommendations.Columns.Add("TroubleCodeDescriptionOrig", typeof(string));
                recommendations.Columns.Add("ResultTroubleCodeId", typeof(long));
                recommendations.Columns.Add("InformCustomerInd", typeof(bool));
                recommendations.Columns.Add("AccidentRelatedInd", typeof(bool));
                recommendations.Columns.Add("ExcludeFromReportInd", typeof(bool));
                recommendations.Columns.Add("CodeClearedInd", typeof(bool));
                recommendations.Columns.Add("TroubleCodeNoteText", typeof(string));
                recommendations.Columns.Add("TroubleCodeRecommendationId", typeof(int));
                recommendations.Columns.Add("TroubleCodeRecommendationText", typeof(string));
                recommendations.Columns.Add("RecommendationTextSeverity", typeof(int));

                // Load Recommendations.
                foreach (var recommendation in update.TroubleCodeRecommendations
                    .Where(r => r.Recommendations.Any(c => c.CurrentRequestInd)))
                {
                    recommendations.Rows.Add
                        (recommendation.ReportOrderTroubleCodeId,
                        recommendation.ControllerId,
                        recommendation.ControllerIdOrig,
                        recommendation.ControllerName,
                        recommendation.ControllerNameOrig,
                        recommendation.TroubleCodeId,
                        recommendation.TroubleCodeIdOrig,
                        recommendation.TroubleCode,
                        recommendation.TroubleCodeOrig,
                        recommendation.TroubleCodeDescription,
                        recommendation.TroubleCodeDescriptionOrig,
                        recommendation.Recommendations.First(s => s.CurrentRequestInd)?.ResultTroubleCodeId,
                        recommendation.Recommendations.First(s => s.CurrentRequestInd)?.InformCustomerInd,
                        recommendation.Recommendations.First(s => s.CurrentRequestInd)?.AccidentRelatedInd,
                        recommendation.Recommendations.First(s => s.CurrentRequestInd)?.ExcludeFromReportInd,
                        recommendation.Recommendations.First(s => s.CurrentRequestInd)?.CodeClearedInd,
                        recommendation.Recommendations.First(s => s.CurrentRequestInd)?.TroubleCodeNoteText,
                        recommendation.Recommendations.First(s => s.CurrentRequestInd)?.TroubleCodeRecommendationId,
                        recommendation.Recommendations.First(s => s.CurrentRequestInd)?.TroubleCodeRecommendationText,
                        recommendation.Recommendations.First(s => s.CurrentRequestInd)?.RecommendationTextSeverity ?? 0);
                }

                // Create Vehicle Make Tools.
                var vehicleMakeTools = new DataTable();
                vehicleMakeTools.Columns.Add("VehicleMakeToolId", typeof(int));
                vehicleMakeTools.Columns.Add("ToolVersion", typeof(string));

                // Load Vehicle Make Tools.
                foreach (var item in update.VehicleMakeTools.Where(x => x.CheckedInd))
                {
                    vehicleMakeTools.Rows.Add(item.VehicleMakeToolId, item.ToolVersion);
                }

                // Create Parameter.
                var param = new
                {
                    update.RequestId,
                    update.RequestTypeId,
                    update.RequestCategoryId,
                    update.AirProToolId,
                    update.ReportFooterHTML,
                    update.ReportHeaderHTML,
                    update.TechnicianNotes,
                    update.CompleteReport,
                    update.CancelReport,
                    update.CancelNotes,
                    update.CancelReasonTypeId,
                    update.ResponsibleTechUserId,
                    update.ReportVersion,
                    WorkTypeIds = JsonConvert.SerializeObject(update.WorkTypeSelections.Where(t => t.WorkTypeSelected).Select(t => t.WorkTypeId).ToArray()),
                    ResultIds = JsonConvert.SerializeObject(update.DiagnosticResultSelections.Where(r => r.AssignedToRequestInd).Select(r => r.ResultId).ToArray()),
                    SelectedResultIds = JsonConvert.SerializeObject(update.DiagnosticResultSelections.Where(r => r.SelectedForReportInd).Select(r => r.ResultId).ToArray()),
                    ReviewedRuleIds = JsonConvert.SerializeObject(update.ValidationRules.Where(r => r.ResultAcknowledgedInd).Select(r => r.ValidationRuleId).ToArray()),
                    Recommendations = recommendations,
                    Decisions = decisions,
                    User.UserGuid,
                    ReportVehicleMakeTools = vehicleMakeTools
                };

                // Execute Update.
                requestId = Conn.Query<int>("Scan.usp_SaveReport", param, null, true, null, CommandType.StoredProcedure).First();

                // Set Result.
                result = new UpdateResultDto(true, "Scan Report Updated.");
            }
            catch (Exception e)
            {
                // Log Error.
                Logger.LogException(e);

                // Set Error Result.
                result = new UpdateResultDto(false, e.Message);
            }

            // Load Request.
            update = GetById(requestId.ToString());

            // Set Result.
            update.UpdateResult = result;

            return update;
        }

        public Task<IReportDto> SaveAsync(IReportDto update)
        {
            throw new NotImplementedException();
        }

        public bool Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}

