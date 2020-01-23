using AirPro.Service.DTOs.Concrete;
using AirPro.Service.DTOs.Interface;
using AirPro.Service.Services.Abstract;
using AirPro.Service.Services.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AirPro.Logging;
using Dapper;
using Newtonsoft.Json;

namespace AirPro.Service.Services.Concrete
{
    internal class RequestTypeService : ServiceBase, IService<IRequestTypeDto>
    {
        public RequestTypeService(IServiceSettings settings) : base(settings) { }

        public IEnumerable<IRequestTypeDto> GetAll(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<IRequestTypeDto>> GetAllAsync(ServiceArgs args = null)
        {
            return await Conn.QueryAsync<RequestTypeDto>("Scan.usp_GetRequestTypes", param: new { User.UserGuid }, commandType: CommandType.StoredProcedure);
        }

        public IGridPageDto<IRequestTypeDto> GetAllByGridPage(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IGridPageDto<IRequestTypeDto> GetAllByGridPage(int pageNumber, int pageSize, string sort, string searchPhrase)
        {
            throw new NotImplementedException();
        }

        public Task<IGridPageDto<IRequestTypeDto>> GetAllByGridPageAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IRequestTypeDto GetById(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IRequestTypeDto> GetByIdAsync(string id)
        {
            return await Conn.QueryFirstOrDefaultAsync<RequestTypeDto>("Scan.usp_GetRequestTypes", param: new { User.UserGuid, RequestTypeId = id }, commandType: CommandType.StoredProcedure);
        }

        public IEnumerable<KeyValuePair<string, string>> GetDisplayList(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<KeyValuePair<string, string>>> GetDisplayListAsync(ServiceArgs args = null)
        {
            return (await Conn.QueryAsync("Scan.usp_GetRequestTypeDisplayList", commandType: CommandType.StoredProcedure))
                ?.Select(t => new KeyValuePair<string, string>(t.RequestTypeId.ToString(), t.RequestTypeName)).ToList();
        }

        public string GetDisplayName(string id)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetDisplayNameAsync(string id)
        {
            throw new NotImplementedException();
        }

        public IRequestTypeDto Save(IRequestTypeDto update)
        {
            throw new NotImplementedException();
        }

        public async Task<IRequestTypeDto> SaveAsync(IRequestTypeDto update)
        {
            try
            {
                // Create Parameter.
                var param = new
                {
                    update.RequestTypeId,
                    update.RequestTypeName,
                    update.ActiveFlag,
                    update.BillableFlag,
                    update.SortOrder,
                    update.DefaultPrice,
                    update.InvoiceMemo,
                    update.Instructions,
                    RequestCategoryIds = string.Join(",", update.RequestCategoryIds ?? new List<int>()),
                    ValidationRuleIds = string.Join(",", update.ValidationRuleIds ?? new List<int>())
                };

                // Execute Update.
                var requestTypeId = await Conn.QueryFirstAsync<int>(sql: "Scan.usp_SaveRequestType", param: param, commandType: CommandType.StoredProcedure);

                // Load Update.
                update = await GetByIdAsync(requestTypeId.ToString());

                // Set Result.
                update.UpdateResult = new UpdateResultDto(true, "Request Type Updated.");
            }
            catch (Exception e)
            {
                // Log Error.
                Logger.LogException(e);

                // Set Error Result.
                update.UpdateResult = new UpdateResultDto(false, e.Message);
            }

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
