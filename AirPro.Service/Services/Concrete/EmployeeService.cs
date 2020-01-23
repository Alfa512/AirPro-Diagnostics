using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AirPro.Service.DTOs.Concrete;
using AirPro.Service.DTOs.Interface;
using AirPro.Service.Services.Abstract;
using AirPro.Service.Services.Interface;
using Dapper;

namespace AirPro.Service.Services.Concrete
{
    internal class EmployeeService : ServiceBase, IService<IEmployeeDto>
    {
        public EmployeeService(IServiceSettings settings) : base(settings)
        {
        }

        public IEmployeeDto GetById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEmployeeDto> GetByIdAsync(string id)
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
            return (await Conn.QueryAsync<EmployeeDto>("Access.usp_GetEmployeeDisplayList", commandType: CommandType.StoredProcedure)).ToDictionary(x => x.UserGuid.ToString(), x => x.DisplayName);
        }

        public IEnumerable<IEmployeeDto> GetAll(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IEmployeeDto>> GetAllAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IGridPageDto<IEmployeeDto> GetAllByGridPage(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public Task<IGridPageDto<IEmployeeDto>> GetAllByGridPageAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IGridPageDto<IEmployeeDto> GetAllByGridPage(int pageNumber, int pageSize, string sort, string searchPhrase)
        {
            throw new NotImplementedException();
        }

        public IEmployeeDto Save(IEmployeeDto update)
        {
            throw new NotImplementedException();
        }

        public Task<IEmployeeDto> SaveAsync(IEmployeeDto update)
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
