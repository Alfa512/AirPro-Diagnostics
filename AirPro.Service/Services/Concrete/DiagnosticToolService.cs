using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AirPro.Service.DTOs.Concrete;
using AirPro.Service.DTOs.Interface;
using AirPro.Service.Services.Abstract;
using AirPro.Service.Services.Interface;
using AutoMapper;

namespace AirPro.Service.Services.Concrete
{
    internal class DiagnosticToolService : ServiceBase, IService<IDiagnosticToolDto>
    {
        public DiagnosticToolService(IServiceSettings settings) : base(settings)
        {
        }

        public IDiagnosticToolDto GetById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IDiagnosticToolDto> GetByIdAsync(string id)
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

        public IEnumerable<IDiagnosticToolDto> GetAll(ServiceArgs args = null)
        {

            // Load Diagnostic Tools.
            var tools = Db.DiagnosticTools.ToList();

            // Map tools
            var toolsDto = Mapper.Map<List<DiagnosticToolDto>>(tools);

            // Convert concrete to interface
            var result = toolsDto.ToList<IDiagnosticToolDto>();

            // Return.
            return result;
        }

        public Task<IEnumerable<IDiagnosticToolDto>> GetAllAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IGridPageDto<IDiagnosticToolDto> GetAllByGridPage(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public Task<IGridPageDto<IDiagnosticToolDto>> GetAllByGridPageAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IGridPageDto<IDiagnosticToolDto> GetAllByGridPage(int pageNumber, int pageSize, string sort, string searchPhrase)
        {
            throw new NotImplementedException();
        }

        public IDiagnosticToolDto Save(IDiagnosticToolDto update)
        {
            throw new NotImplementedException();
        }

        public Task<IDiagnosticToolDto> SaveAsync(IDiagnosticToolDto update)
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
