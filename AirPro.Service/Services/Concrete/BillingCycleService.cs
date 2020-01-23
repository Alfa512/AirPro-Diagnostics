using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AirPro.Service.DTOs.Interface;
using AirPro.Service.Services.Abstract;
using AirPro.Service.Services.Interface;

namespace AirPro.Service.Services.Concrete
{
    internal class BillingCycleService : ServiceBase, IService<IBillingCycleDto>
    {
        public BillingCycleService(IServiceSettings settings) : base(settings)
        {
        }

        public IBillingCycleDto GetById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IBillingCycleDto> GetByIdAsync(string id)
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
            return Db.BillingCycles.OrderBy(x => x.SortOrder).ToList().Select(x => new KeyValuePair<string, string>(x.CycleId.ToString(), x.CycleName));
        }

        public Task<IEnumerable<KeyValuePair<string, string>>> GetDisplayListAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IBillingCycleDto> GetAll(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IBillingCycleDto>> GetAllAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IGridPageDto<IBillingCycleDto> GetAllByGridPage(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public Task<IGridPageDto<IBillingCycleDto>> GetAllByGridPageAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IGridPageDto<IBillingCycleDto> GetAllByGridPage(int pageNumber, int pageSize, string sort, string searchPhrase)
        {
            throw new NotImplementedException();
        }

        public IBillingCycleDto Save(IBillingCycleDto update)
        {
            throw new NotImplementedException();
        }

        public Task<IBillingCycleDto> SaveAsync(IBillingCycleDto update)
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
