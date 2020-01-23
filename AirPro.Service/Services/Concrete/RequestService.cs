using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AirPro.Service.DTOs.Concrete;
using AirPro.Service.DTOs.Interface;
using AirPro.Service.Services.Abstract;
using AirPro.Service.Services.Interface;
using Dapper;

namespace AirPro.Service.Services.Concrete
{
    internal class RequestService : ServiceBase, IService<IRequestDto>
    {
        public RequestService(IServiceSettings settings) : base(settings)
        {
        }

        public IRequestDto GetById(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IRequestDto> GetByIdAsync(string id)
        {
            // Load Scan Request.
            RequestDto result;
            using (var x = await Conn.QueryMultipleAsync(sql: "Scan.usp_GetRequestById", param: 
                new { User.UserGuid, RequestId = id }, commandType: CommandType.StoredProcedure))
            {
                // Load Request.
                result = x.Read<RequestDto>()?.FirstOrDefault();

                // Check Request.
                if (result != null)
                {
                    // Load Lists.
                    result.WarningIndicators = x.Read<string>().ToList();
                    result.PointsOfImpact = x.Read<int>().ToList();

                    // Remove Unicode.
                    result.VehicleLookupInfo = Regex.Replace((result?.VehicleLookupInfo ?? string.Empty), @"\\+[uU]([0-9A-Fa-f]{4})", " ");
                }
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

        public IEnumerable<IRequestDto> GetAll(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<IRequestDto>> GetAllAsync(ServiceArgs args = null)
        {
            // Set Arguments.
            if (args == null) args = new ServiceArgs();
            args.Add("CurrentPage", "1");
            args.Add("RowCount", "1000");
            args.Add("CompletedInd", "false");
            args.Remove("SearchPhrase");

            // Load Requests.
            var requestGrid = await GetAllByGridPageAsync(args);

            // Return Requests.
            return requestGrid.Rows;
        }

        public IGridPageDto<IRequestDto> GetAllByGridPage(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public async Task<IGridPageDto<IRequestDto>> GetAllByGridPageAsync(ServiceArgs args = null)
        {
            IGridPageDto<IRequestDto> result;

            using (var x = await Conn.QueryMultipleAsync("Scan.usp_GetRequestsByUser", param: ParamsFromArgs(User.UserGuid, args), commandType: CommandType.StoredProcedure))
            {
                // Load Grid Page.
                result = x.ReadFirst<GridPageDto<IRequestDto>>();

                // Load Requests.
                result.Rows = x.Read<RequestDto>().ToList();
            }

            return result;
        }

        public IGridPageDto<IRequestDto> GetAllByGridPage(int pageNumber, int pageSize, string sort, string searchPhrase)
        {
            throw new NotImplementedException();
        }

        public IRequestDto Save(IRequestDto update)
        {
            throw new NotImplementedException();
        }

        public async Task<IRequestDto> SaveAsync(IRequestDto update)
        {
            if (update.RequestId == 0) throw new NotImplementedException();

            var scanRequest = await Db.ScanRequests.FirstOrDefaultAsync(x => x.RequestId == update.RequestId);
            if (scanRequest == null) return await GetByIdAsync(update.RequestId.ToString());

            scanRequest.OrderId = update.RepairId;
            await Db.SaveChangesAsync();

            return await GetByIdAsync(update.RequestId.ToString());
        }

        public bool Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        private static object ParamsFromArgs(Guid userGuid, ServiceArgs args = null)
        {
            var sort = (args != null && args.ContainsKey("SortOrder") ? args["SortOrder"].ToString() : string.Empty).Replace(",", "").Split(' ');

            return new
            {
                UserGuid = userGuid,
                CompletedInd = args != null && args.ContainsKey("CompletedInd") && bool.TryParse(args["CompletedInd"].ToString(), out var completed) && completed,
                Search = args != null && args.ContainsKey("SearchPhrase") ? args["SearchPhrase"].ToString() : string.Empty,
                CurrentPage = args != null && args.ContainsKey("CurrentPage") && int.TryParse(args["CurrentPage"].ToString(), out var page) ? page : 1,
                RowCount = args != null && args.ContainsKey("RowCount") && int.TryParse(args["RowCount"].ToString(), out var rows) ? rows : 25,
                SortCol = sort.Length > 0 ? sort[0] : "RequestCreatedDt",
                SortDir = sort.Length > 1 ? sort[1] : "ASC",
            };
        }
    }
}
