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
    internal class TroubleCodeRecommendationService : ServiceBase, IService<ITroubleCodeRecommendationDto>
    {
        public TroubleCodeRecommendationService(IServiceSettings settings) : base(settings)
        {
        }

        public ITroubleCodeRecommendationDto GetById(string id)
        {
            return GetByIdAsync(id).GetAwaiter().GetResult();
        }

        public async Task<ITroubleCodeRecommendationDto> GetByIdAsync(string id)
        {
            // Load Item.
            var recommendationDictionary = new Dictionary<int, TroubleCodeRecommendationDto>();
            var query = await Conn.QueryAsync<TroubleCodeRecommendationDto, TroubleCodeRecommendationUsageDto, TroubleCodeRecommendationDto>("Scan.usp_GetTroubleCodeRecommendations",
                (r, u) =>
                {
                    if (!recommendationDictionary.TryGetValue(r.TroubleCodeRecommendationId, out var recommendation))
                    {
                        recommendation = r;
                        recommendation.RecommendationUsage = new List<ITroubleCodeRecommendationUsageDto>();
                        recommendationDictionary.Add(recommendation.TroubleCodeRecommendationId, recommendation);
                    }

                    if (recommendation != null && u != null)
                    {
                        (recommendation.RecommendationUsage as List<ITroubleCodeRecommendationUsageDto>)?.Add(u);
                    }

                    return recommendation;
                },
                new
                {
                    UserGuid = User.UserGuid,
                    RecommendationId = id
                }, null, true, "VehicleMakeId", null, CommandType.StoredProcedure);

            // Return Item.
            return query.First();
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

        public IEnumerable<ITroubleCodeRecommendationDto> GetAll(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ITroubleCodeRecommendationDto>> GetAllAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IGridPageDto<ITroubleCodeRecommendationDto> GetAllByGridPage(ServiceArgs args = null)
        {
            return GetAllByGridPageAsync(args).GetAwaiter().GetResult();
        }

        public async Task<IGridPageDto<ITroubleCodeRecommendationDto>> GetAllByGridPageAsync(ServiceArgs args = null)
        {
            // Load Grid Items.
            var query = await Conn.QueryAsync<TroubleCodeRecommendationDto>("Scan.usp_GetTroubleCodeRecommendations",
                new
                {
                    UserGuid = User.UserGuid,
                    Search = args?["SearchPhrase"]?.ToString(),
                }, null, null, CommandType.StoredProcedure);

            // Select Distinct.
            var distinct = query.GroupBy(q => q.TroubleCodeRecommendationId).Select(q => q.First()).ToList<ITroubleCodeRecommendationDto>();

            // Set Default Sort.
            args?.SetDefaultSort("TroubleCodeRecommendationId ASC");

            // Load Grid Page.
            var result = distinct.GetGridPageFromCollection(args);

            // Return.
            return result;
        }

        public IGridPageDto<ITroubleCodeRecommendationDto> GetAllByGridPage(int pageNumber, int pageSize, string sort, string searchPhrase)
        {
            // Load Arguments.
            var args = new ServiceArgs();
            args.AddGridOptions(pageNumber, pageSize, sort, searchPhrase);

            return GetAllByGridPageAsync(args).GetAwaiter().GetResult();
        }

        public ITroubleCodeRecommendationDto Save(ITroubleCodeRecommendationDto update)
        {
            return SaveAsync(update).GetAwaiter().GetResult();
        }

        public async Task<ITroubleCodeRecommendationDto> SaveAsync(ITroubleCodeRecommendationDto update)
        {
            UpdateResultDto result;

            try
            {
                // Check Update.
                if (update == null) throw new NullReferenceException("Update can NOT be NULL.");

                // Create Parameter.
                var param = new
                {
                    User.UserGuid,
                    update.TroubleCodeRecommendationId,
                    update.TroubleCodeRecommendationText,
                    update.ActiveInd
                };

                // Execute Update.
                var recommendationId = (await Conn.QueryAsync<int>("Scan.usp_SaveTroubleCodeRecommendation", param, null, null, CommandType.StoredProcedure)).First();

                // Set Result.
                result = new UpdateResultDto(true, "Recommendation Updated.");

                // Load Request.
                update = await GetByIdAsync(recommendationId.ToString());
            }
            catch (Exception e)
            {
                // Log Error.
                Logger.LogException(e);

                // Set Error Result.
                result = new UpdateResultDto(false, e.Message);
            }

            // Set Result.
            if (update == null) update = new TroubleCodeRecommendationDto();
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
}
