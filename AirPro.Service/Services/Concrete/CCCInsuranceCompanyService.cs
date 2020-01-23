using AirPro.Service.DTOs.Concrete;
using AirPro.Service.DTOs.Interface;
using AirPro.Service.Services.Abstract;
using AirPro.Service.Services.Interface;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace AirPro.Service.Services.Concrete
{
    internal class CCCInsuranceCompanyService : ServiceBase, IService<ICCCInsuranceCompanyDto>
    {
        public CCCInsuranceCompanyService(IServiceSettings settings) : base(settings)
        {
        }

        public bool Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ICCCInsuranceCompanyDto> GetAll(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ICCCInsuranceCompanyDto>> GetAllAsync(ServiceArgs args = null)
        {
            var companies = await Db.CCCInsuranceCompanies.ToListAsync();
            return Mapper.Map<List<CCCInsuranceCompanyDto>>(companies).ToList<ICCCInsuranceCompanyDto>();
        }

        public IGridPageDto<ICCCInsuranceCompanyDto> GetAllByGridPage(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IGridPageDto<ICCCInsuranceCompanyDto> GetAllByGridPage(int pageNumber, int pageSize, string sort, string searchPhrase)
        {
            throw new NotImplementedException();
        }

        public Task<IGridPageDto<ICCCInsuranceCompanyDto>> GetAllByGridPageAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public ICCCInsuranceCompanyDto GetById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<ICCCInsuranceCompanyDto> GetByIdAsync(string id)
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

        public string GetDisplayName(string id)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetDisplayNameAsync(string id)
        {
            throw new NotImplementedException();
        }

        public ICCCInsuranceCompanyDto Save(ICCCInsuranceCompanyDto update)
        {
            throw new NotImplementedException();
        }

        public Task<ICCCInsuranceCompanyDto> SaveAsync(ICCCInsuranceCompanyDto update)
        {
            throw new NotImplementedException();
        }
    }
}
