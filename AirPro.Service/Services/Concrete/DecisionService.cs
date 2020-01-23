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

namespace AirPro.Service.Services.Concrete
{
    internal class DecisionService : ServiceBase, IService<IDecisionDto>
    {
        public DecisionService(IServiceSettings settings) : base(settings)
        {
        }

        public IDecisionDto GetById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IDecisionDto> GetByIdAsync(string id)
        {
            // Load Decision.
            DecisionDto decision;
            using (var query = Conn.QueryMultipleAsync("Scan.usp_GetDecisionById", new { User.UserGuid, DecisionId = id }, null, null, CommandType.StoredProcedure).Result)
            {
                decision = query.Read<DecisionDto>().FirstOrDefault() ?? new DecisionDto { ActiveInd = true };
                decision.VehicleMakes = query.Read<DecisionVehicleMakeDto>().ToList();
                decision.RequestTypes = query.Read<DecisionRequestTypeDto>().ToList();
                decision.RequestCategories = query.Read<DecisionRequestCategoryDto>().ToList();
            }

            // Return.
            return Task.FromResult<IDecisionDto>(decision);
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

        public IEnumerable<IDecisionDto> GetAll(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IDecisionDto>> GetAllAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IGridPageDto<IDecisionDto> GetAllByGridPage(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public async Task<IGridPageDto<IDecisionDto>> GetAllByGridPageAsync(ServiceArgs args = null)
        {
            // Load Grid Items.
            var query = await Conn.QueryAsync<DecisionDto>("Scan.usp_GetDecisionsByGridPage",
                new
                {
                    UserGuid = User.UserGuid,
                    Search = args?["SearchPhrase"]?.ToString(),
                }, null, null, CommandType.StoredProcedure);

            // Set Default Sort.
            args?.SetDefaultSort("DecisionId ASC");

            // Load Grid Page.
            var result = query.ToList<IDecisionDto>().GetGridPageFromCollection(args);

            // Return.
            return result;
        }

        public IGridPageDto<IDecisionDto> GetAllByGridPage(int pageNumber, int pageSize, string sort, string searchPhrase)
        {
            throw new NotImplementedException();
        }

        public IDecisionDto Save(IDecisionDto update)
        {
            throw new NotImplementedException();
        }

        public async Task<IDecisionDto> SaveAsync(IDecisionDto update)
        {
            UpdateResultDto result;

            try
            {
                // Check Update.
                if (update == null) throw new NullReferenceException("Update can NOT be NULL.");

                // Create Vehicle Makes.
                var vehicleMakes = update.VehicleMakes.Where(m => m.SelectedInd)
                    .Select(m => new { TypeId = m.VehicleMakeId, TypePreSelectedInd = m.PreSelectedInd }).GetSettingsTable();

                // Create Request Types.
                var requestTypes = update.RequestTypes.Where(t => t.SelectedInd)
                    .Select(t => new { TypeId = t.RequestTypeId, TypePreSelectedInd = t.PreSelectedInd }).GetSettingsTable();

                // Create Request Categories.
                var requestCategories = update.RequestCategories.Where(c => c.SelectedInd)
                    .Select(c => new { TypeId = c.RequestCategoryId, TypePreSelectedInd = c.PreSelectedInd }).GetSettingsTable();

                // Create Parameter.
                var param = new
                {
                    User.UserGuid,
                    update.DecisionId,
                    update.DecisionText,
                    DefaultTextSeverity = (int)update.DefaultTextSeverity,
                    update.ActiveInd,
                    VehicleMakes = vehicleMakes,
                    RequestTypes = requestTypes,
                    RequestCategories = requestCategories
                };

                // Execute Update.
                var decisionId = (await Conn.QueryAsync<int>("Scan.usp_SaveDecision", param, null, null, CommandType.StoredProcedure)).First();

                // Set Result.
                result = new UpdateResultDto(true, "Decision Updated.");

                // Load Request.
                update = await GetByIdAsync(decisionId.ToString());
            }
            catch (Exception e)
            {
                // Log Error.
                Logger.LogException(e);

                // Set Error Result.
                result = new UpdateResultDto(false, e.Message);
            }

            // Set Result.
            if (update == null) update = new DecisionDto();
            update.UpdateResult = result;

            // Return.
            return update;
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

    internal static class DecisionServiceExtensions
    {
        public static DataTable GetSettingsTable<T>(this IEnumerable<T> settings)
        {
            // Create Result.
            var result = new DataTable();
            result.Columns.Add("TypeId", typeof(int));
            result.Columns.Add("TypePreSelectedInd", typeof(bool));

            // Load Settings.
            foreach (var setting in settings.ToList())
            {
                Type type = setting.GetType();
                var typeId = type.GetProperty("TypeId");
                var preSel = type.GetProperty("TypePreSelectedInd");
                if (typeId != null && preSel != null)
                {
                    result.Rows.Add(typeId.GetValue(setting), preSel.GetValue(setting));
                }
            }

            // Return Result.
            return result;
        }
    }
}
