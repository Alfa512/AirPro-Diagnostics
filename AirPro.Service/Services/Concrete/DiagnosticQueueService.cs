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
    internal class DiagnosticQueueService : ServiceBase, IService<IDiagnosticQueueDto>
    {
        public DiagnosticQueueService(IServiceSettings settings) : base(settings)
        {
        }

        public IDiagnosticQueueDto GetById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IDiagnosticQueueDto> GetByIdAsync(string id)
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

        public IEnumerable<IDiagnosticQueueDto> GetAll(ServiceArgs args = null)
        {
            // Create Result.
            var result = new List<IDiagnosticQueueDto>();

            // Check Args.
            if (args == null) return result;

            try
            {
                // Shop Guid.
                Guid shopGuid = new Guid();
                if (args.ContainsKey("ShopGuid"))
                    Guid.TryParse(args["ShopGuid"].ToString(), out shopGuid);

                // Load Params.
                var param = new
                {
                    ShopGuid = shopGuid,
                    User.UserGuid
                };

                // Load Scan Queue.
                result.AddRange(Conn.Query<DiagnosticQueueDto>("Diagnostic.usp_GetDiagnosticQueueByShop", param,
                    commandType: CommandType.StoredProcedure).ToList());
            }
            catch (Exception e)
            {
                Logger.LogException(e);
            }

            return result;
        }

        public Task<IEnumerable<IDiagnosticQueueDto>> GetAllAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IGridPageDto<IDiagnosticQueueDto> GetAllByGridPage(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public Task<IGridPageDto<IDiagnosticQueueDto>> GetAllByGridPageAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IGridPageDto<IDiagnosticQueueDto> GetAllByGridPage(int pageNumber, int pageSize, string sort, string searchPhrase)
        {
            throw new NotImplementedException();
        }

        public IDiagnosticQueueDto Save(IDiagnosticQueueDto update)
        {
            throw new NotImplementedException();
        }

        public Task<IDiagnosticQueueDto> SaveAsync(IDiagnosticQueueDto update)
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
