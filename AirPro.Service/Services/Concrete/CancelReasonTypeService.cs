using AirPro.Service.DTOs.Concrete;
using AirPro.Service.DTOs.Interface;
using AirPro.Service.Services.Abstract;
using AirPro.Service.Services.Interface;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace AirPro.Service.Services.Concrete
{
    internal class CancelReasonTypeService : ServiceBase, IService<ICancelReasonTypeDto>
    {
        public CancelReasonTypeService(IServiceSettings settings) : base(settings)
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

        public IEnumerable<ICancelReasonTypeDto> GetAll(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ICancelReasonTypeDto>> GetAllAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IGridPageDto<ICancelReasonTypeDto> GetAllByGridPage(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IGridPageDto<ICancelReasonTypeDto> GetAllByGridPage(int pageNumber, int pageSize, string sort, string searchPhrase)
        {
            throw new NotImplementedException();
        }

        public Task<IGridPageDto<ICancelReasonTypeDto>> GetAllByGridPageAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public ICancelReasonTypeDto GetById(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<ICancelReasonTypeDto> GetByIdAsync(string id)
        {
            int cancelReasonTypeId = int.Parse(id);
            var reason = await Db.CancelReasonTypes.FirstOrDefaultAsync(x => x.CancelReasonTypeId == cancelReasonTypeId);
            return Mapper.Map<CancelReasonTypeDto>(reason);
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

        public ICancelReasonTypeDto Save(ICancelReasonTypeDto update)
        {
            throw new NotImplementedException();
        }

        public Task<ICancelReasonTypeDto> SaveAsync(ICancelReasonTypeDto update)
        {
            throw new NotImplementedException();
        }
    }
}
