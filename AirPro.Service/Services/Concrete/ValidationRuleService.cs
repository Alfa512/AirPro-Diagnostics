using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AirPro.Service.DTOs.Interface;
using AirPro.Service.Services.Abstract;
using AirPro.Service.Services.Interface;
using Dapper;

namespace AirPro.Service.Services.Concrete
{
    internal class ValidationRuleService : ServiceBase, IService<IValidationRuleDto>
    {
        public ValidationRuleService(IServiceSettings settings) : base(settings)
        {
        }

        public IValidationRuleDto GetById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IValidationRuleDto> GetByIdAsync(string id)
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

        public async Task<IEnumerable<KeyValuePair<string, string>>> GetDisplayListAsync(ServiceArgs args = null)
        {
            return (await Conn.QueryAsync("Scan.usp_GetValidationRuleDisplayList", commandType: CommandType.StoredProcedure))
                ?.Select(r => new KeyValuePair<string, string>(r.ValidationRuleId.ToString(), r.ValidationRuleText)).ToList();
        }

        public IEnumerable<IValidationRuleDto> GetAll(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IValidationRuleDto>> GetAllAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IGridPageDto<IValidationRuleDto> GetAllByGridPage(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public Task<IGridPageDto<IValidationRuleDto>> GetAllByGridPageAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IGridPageDto<IValidationRuleDto> GetAllByGridPage(int pageNumber, int pageSize, string sort, string searchPhrase)
        {
            throw new NotImplementedException();
        }

        public IValidationRuleDto Save(IValidationRuleDto update)
        {
            throw new NotImplementedException();
        }

        public Task<IValidationRuleDto> SaveAsync(IValidationRuleDto update)
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
