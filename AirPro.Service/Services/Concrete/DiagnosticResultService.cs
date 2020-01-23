using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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
    internal class DiagnosticResultService : ServiceBase, IService<IDiagnosticResultDto>
    {
        public DiagnosticResultService(IServiceSettings settings) : base(settings)
        {
        }

        public IDiagnosticResultDto GetById(string id)
        {
            // Load Diagnostics.
            var resultDictionary = new Dictionary<int, DiagnosticResultDto>();
            var result = Conn
                .Query<DiagnosticResultDto, DiagnosticControllerDto, DiagnosticTroubleCodeDto, dynamic,
                    DiagnosticResultDto>("Diagnostic.usp_GetDiagnosticResults",
                (r, c, tc, ff) => MapDiagnosticResult(resultDictionary, r, c, tc, ff),
                param: new { ResultId = id },
                transaction: null,
                buffered: true,
                splitOn: "ControllerId,DiagnosticTroubleCodeId,FreezeFrameDiagnosticTroubleCode",
                commandTimeout: null,
                commandType: CommandType.StoredProcedure)
                .FirstOrDefault<IDiagnosticResultDto>();

            // Return.
            return result;
        }

        public Task<IDiagnosticResultDto> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
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

        public IEnumerable<IDiagnosticResultDto> GetAll(ServiceArgs args = null)
        {
            // Check Args.
            if (args == null) return null;

            try
            {
                // Shop Guid.
                Guid shopGuid = new Guid();
                if (args.ContainsKey("ShopGuid"))
                    Guid.TryParse(args["ShopGuid"].ToString(), out shopGuid);

                // Request Id.
                int requestId = 0;
                if (args.ContainsKey("RequestId"))
                    int.TryParse(args["RequestId"].ToString(), out requestId);

                // Load Params.
                var param = new
                {
                    ShopGuid = shopGuid,
                    RequestId = requestId
                };

                // Load Diagnostics.
                var resultDictionary = new Dictionary<int, DiagnosticResultDto>();
                var result = Conn
                    .Query<DiagnosticResultDto, DiagnosticControllerDto, DiagnosticTroubleCodeDto, dynamic,
                        DiagnosticResultDto>("Diagnostic.usp_GetDiagnosticResults",
                        (r, c, tc, ff) => MapDiagnosticResult(resultDictionary, r, c, tc, ff),
                        param: param,
                        transaction: null,
                        buffered: true,
                        splitOn: "ControllerId,DiagnosticTroubleCodeId,FreezeFrameDiagnosticTroubleCode",
                        commandTimeout: null,
                        commandType: CommandType.StoredProcedure)
                    .Distinct().ToList<IDiagnosticResultDto>();

                // Return Results.
                return result;
            }
            catch (Exception e)
            {
                Logger.LogException(e);
            }

            return null;
        }

        public Task<IEnumerable<IDiagnosticResultDto>> GetAllAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IGridPageDto<IDiagnosticResultDto> GetAllByGridPage(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public Task<IGridPageDto<IDiagnosticResultDto>> GetAllByGridPageAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IGridPageDto<IDiagnosticResultDto> GetAllByGridPage(int pageNumber, int pageSize, string sort, string searchPhrase)
        {
            throw new NotImplementedException();
        }

        public IDiagnosticResultDto Save(IDiagnosticResultDto update)
        {
            try
            {
                // Check Update.
                if (update == null) throw new NullReferenceException("Update can NOT be NULL.");

                // Lookup Vehicle.
                if (!string.IsNullOrEmpty(update?.VehicleVin))
                {
                    var vs = new VehicleService(Settings);
                    vs.GetById(update.VehicleVin);
                }

                // Create Trouble Codes.
                var troubleCodes = new DataTable();
                troubleCodes.Columns.Add("ControllerId", typeof(int));
                troubleCodes.Columns.Add("ControllerName", typeof(string));
                troubleCodes.Columns.Add("TroubleCodeId", typeof(int));
                troubleCodes.Columns.Add("TroubleCode", typeof(string));
                troubleCodes.Columns.Add("TroubleCodeDescription", typeof(string));
                troubleCodes.Columns.Add("TroubleCodeInformation", typeof(string));

                // Create Freeze Frames.
                var freezeFrames = new DataTable();
                freezeFrames.Columns.Add("ControllerId", typeof(int));
                freezeFrames.Columns.Add("ControllerName", typeof(string));
                freezeFrames.Columns.Add("FreezeFrameId", typeof(int));
                freezeFrames.Columns.Add("FreezeFrameTroubleCode", typeof(string));
                freezeFrames.Columns.Add("SensorGroupsJson", typeof(string));

                // Process Controllers.
                foreach (var c in update?.Controllers ?? new List<IDiagnosticControllerDto>())
                {
                    // Load Trouble Codes.
                    foreach (var tc in c.TroubleCodes)
                    {
                        if (!string.IsNullOrEmpty(tc.DiagnosticTroubleCodeDescription))
                            troubleCodes.Rows.Add(c.ControllerId, c.ControllerName, tc.DiagnosticTroubleCodeId,
                                tc.DiagnosticTroubleCode, tc.DiagnosticTroubleCodeDescription,
                                JsonConvert.SerializeObject(tc.DiagnosticTroubleCodeInformationList));
                    }

                    // Load Freeze Frames.
                    foreach (var ff in c.FreezeFrames)
                    {
                        freezeFrames.Rows.Add(c.ControllerId, c.ControllerName, null, ff.FreezeFrameDiagnosticTroubleCode,
                            JsonConvert.SerializeObject(ff.FreezeFrameSensorGroups));
                    }
                }

                // Create Parameter.
                var param = new
                {
                    User.UserGuid,
                    update.DiagnosticTool,
                    update.DiagnosticFileType,
                    update.DiagnosticFileText,
                    update.ResultId,
                    update.RequestId,
                    update.CustomerFirstName,
                    update.CustomerLastName,
                    update.CustomerRo,
                    update.ScanDateTime,
                    update.ShopName,
                    update.ShopAddress,
                    update.ShopEmail,
                    update.ShopFax,
                    update.ShopPhone,
                    update.VehicleVin,
                    update.VehicleMake,
                    update.VehicleModel,
                    update.VehicleYear,
                    TestabilityIssues = JsonConvert.SerializeObject(update.TestabilityIssuesList),
                    DeletedInd = false,
                    TroubleCodes = troubleCodes.AsTableValuedParameter("Diagnostic.udt_ResultTroubleCodes"),
                    FreezeFrames = freezeFrames.AsTableValuedParameter("Diagnostic.udt_ResultFreezeFrames")
                };

                // Execute Update.
                update.ResultId = Conn.Query<int>("Diagnostic.usp_SaveDiagnosticResult", param, null, true, null, CommandType.StoredProcedure).First();
            }
            catch (Exception e)
            {
                Logger.LogException(e, update);
            }

            // Update Result.
            if (update == null) update = new DiagnosticResultDto();
            update.UpdateResult = update.ResultId > 0 ? new UpdateResultDto(true, "Result Saved.") : new UpdateResultDto(false, "Result Save Failed.");

            // Return.
            return update;
        }

        public Task<IDiagnosticResultDto> SaveAsync(IDiagnosticResultDto update)
        {
            throw new NotImplementedException();
        }

        public bool Delete(string id)
        {
            try
            {
                // Check Identity.
                if (!int.TryParse(id, out var resultId)) return false;

                // Find Result.
                var result = Db.DiagnosticResults.Find(resultId);

                // Check Result.
                if (result == null) return false;

                // Flip Deleted Ind.
                result.DeletedInd = !result.DeletedInd;
                result.UpdatedByUserGuid = User.UserGuid;
                result.UpdatedDt = DateTimeOffset.UtcNow;

                // Modify State.
                Db.Entry(result).State = EntityState.Modified;

                // Save Changes.
                Db.SaveChanges();

                // Result.
                return true;
            }
            catch (Exception e)
            {
                // Log Error.
                Logger.LogException(e);
            }

            // Default.
            return false;
        }

        public Task<bool> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        #region Support Methods

        private static DiagnosticResultDto MapDiagnosticResult(Dictionary<int, DiagnosticResultDto> resultDictionary,
            DiagnosticResultDto r, DiagnosticControllerDto c, DiagnosticTroubleCodeDto tc, dynamic ff)
        {
            if (!r.ResultId.HasValue) return null;

            if (!resultDictionary.TryGetValue(r.ResultId.Value, out var result))
            {
                result = r;
                result.Controllers = new List<IDiagnosticControllerDto>();
                resultDictionary.Add(r.ResultId.Value, result);
            }

            if (c == null) return result;

            var controller = result.Controllers.FirstOrDefault(rc => rc.ControllerId == c.ControllerId);
            if (controller == null)
            {
                controller = c;
                controller.TroubleCodes = new List<IDiagnosticTroubleCodeDto>();
                controller.FreezeFrames = new List<IDiagnosticFreezeFrameDto>();
                result.Controllers.Add(controller);
            }

            if (controller.TroubleCodes.All(ctc => ctc.DiagnosticTroubleCodeId != tc.DiagnosticTroubleCodeId))
            {
                controller.TroubleCodes.Add(tc);
            }

            if (string.IsNullOrEmpty(ff?.SensorGroupsJson)) return result;
            if (controller.FreezeFrames.Any(cff => cff.FreezeFrameDiagnosticTroubleCode == ff.FreezeFrameDiagnosticTroubleCode)) return result;

            List<DiagnosticFreezeFrameSensorGroupDto> groups = JsonConvert.DeserializeObject<ICollection<DiagnosticFreezeFrameSensorGroupDto>>(ff.SensorGroupsJson);

            var freezeFrame = new DiagnosticFreezeFrameDto
            {
                FreezeFrameDiagnosticTroubleCode = ff.FreezeFrameDiagnosticTroubleCode,
                FreezeFrameSensorGroups = groups.ToList<IDiagnosticFreezeFrameSensorGroupDto>()
            };
            controller.FreezeFrames.Add(freezeFrame);

            return result;
        }

        #endregion
    }
}
