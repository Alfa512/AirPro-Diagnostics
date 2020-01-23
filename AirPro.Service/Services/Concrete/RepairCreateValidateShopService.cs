using AirPro.Service.DTOs.Concrete;
using AirPro.Service.DTOs.Interface;
using AirPro.Service.Services.Abstract;
using AirPro.Service.Services.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace AirPro.Service.Services.Concrete
{
    internal class RepairCreateValidateShopService : ServiceBase, IService<IRepairCreateValidateShopDto>
    {
        public RepairCreateValidateShopService(IServiceSettings settings) : base(settings)
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

        public IEnumerable<IRepairCreateValidateShopDto> GetAll(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IRepairCreateValidateShopDto>> GetAllAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IGridPageDto<IRepairCreateValidateShopDto> GetAllByGridPage(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IGridPageDto<IRepairCreateValidateShopDto> GetAllByGridPage(int pageNumber, int pageSize, string sort, string searchPhrase)
        {
            throw new NotImplementedException();
        }

        public Task<IGridPageDto<IRepairCreateValidateShopDto>> GetAllByGridPageAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IRepairCreateValidateShopDto GetById(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IRepairCreateValidateShopDto> GetByIdAsync(string id)
        {
            var shopGuid = Guid.Parse(id);
            return new RepairCreateValidateShopDto {
                CanCreateRequest = await Db.ShopRequestTypes.CountAsync(x => x.ShopGuid == shopGuid && x.RequestTypeId != 6 && x.RequestTypeId != 7) > 0
            };
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

        public IRepairCreateValidateShopDto Save(IRepairCreateValidateShopDto update)
        {
            throw new NotImplementedException();
        }

        public Task<IRepairCreateValidateShopDto> SaveAsync(IRepairCreateValidateShopDto update)
        {
            throw new NotImplementedException();
        }
    }
}
