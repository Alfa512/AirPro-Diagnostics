using AirPro.Service.DTOs.Concrete;
using AirPro.Service.DTOs.Interface;
using AirPro.Service.Services.Abstract;
using AirPro.Service.Services.Interface;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirPro.Service.Services.Concrete
{
    internal class RegistrationOptionsService : ServiceBase, IService<IRegistrationOptionsDto>
    {
        public RegistrationOptionsService(IServiceSettings settings) : base(settings)
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

        public IEnumerable<IRegistrationOptionsDto> GetAll(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IRegistrationOptionsDto>> GetAllAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IGridPageDto<IRegistrationOptionsDto> GetAllByGridPage(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IGridPageDto<IRegistrationOptionsDto> GetAllByGridPage(int pageNumber, int pageSize, string sort, string searchPhrase)
        {
            throw new NotImplementedException();
        }

        public Task<IGridPageDto<IRegistrationOptionsDto>> GetAllByGridPageAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IRegistrationOptionsDto GetById(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IRegistrationOptionsDto> GetByIdAsync(string id)
        {
            var result = new RegistrationOptionsDto();

            using (var reader = await Conn.QueryMultipleAsync("Access.usp_GetRegistrationOptions"))
            {
                result.StateSelection = reader.Read<KeyValuePair<string, string>>().ToList();
                result.PricingPlanSelection = reader.Read<KeyValuePair<string, string>>().ToList();
                result.EstimatePlanSelection = reader.Read<KeyValuePair<string, string>>().ToList();
                result.InsuranceCompanies = reader.Read<InsuranceCompanyDto>().ToList<IInsuranceCompanyDto>();
                result.BillingCycleSelection = reader.Read<KeyValuePair<string, string>>().ToList();
                result.AllVehicleMakes = reader.Read<VehicleMakeDto>().ToList<IVehicleMakeDto>().ToList();
                result.GroupSelection = reader.Read<KeyValuePair<Guid, string>>().Select(x => new KeyValuePair<string, string>(x.Key.ToString(), x.Value.ToString())).ToList();
                result.RequestTypeSelection = reader.Read<KeyValuePair<string, string>>().ToList();
                result.CurrencySelection = reader.Read<KeyValuePair<string, string>>().ToList();
            }

            // Add default item
            result.StateSelection.Insert(0, new KeyValuePair<string, string>("", @"<-- Select State -->"));
            result.BillingCycleSelection.Insert(0, new KeyValuePair<string, string>("", @"<-- Select Billing Cycle -->"));

            return result;
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

        public IRegistrationOptionsDto Save(IRegistrationOptionsDto update)
        {
            throw new NotImplementedException();
        }

        public Task<IRegistrationOptionsDto> SaveAsync(IRegistrationOptionsDto update)
        {
            throw new NotImplementedException();
        }
    }
}
