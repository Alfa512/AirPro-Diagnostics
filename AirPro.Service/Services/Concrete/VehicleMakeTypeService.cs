using AirPro.Service.DTOs.Interface;
using AirPro.Service.Services.Abstract;
using AirPro.Service.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirPro.Service.Services.Concrete
{
    internal class VehicleMakeTypeService : ServiceBase, IService<IVehicleMakeTypeDto>
    {
        public VehicleMakeTypeService(IServiceSettings settings) : base(settings)
        {
        }

        public IVehicleMakeTypeDto GetById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IVehicleMakeTypeDto> GetByIdAsync(string id)
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
            return Db.RepairVehicleMakeTypes.OrderBy(s => s.VehicleMakeTypeName).ToList()
                .Select(s => new KeyValuePair<string, string>(s.VehicleMakeTypeId.ToString(), s.VehicleMakeTypeName))
                .ToList();
        }

        public Task<IEnumerable<KeyValuePair<string, string>>> GetDisplayListAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IVehicleMakeTypeDto> GetAll(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IVehicleMakeTypeDto>> GetAllAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IGridPageDto<IVehicleMakeTypeDto> GetAllByGridPage(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public Task<IGridPageDto<IVehicleMakeTypeDto>> GetAllByGridPageAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IGridPageDto<IVehicleMakeTypeDto> GetAllByGridPage(int pageNumber, int pageSize, string sort, string searchPhrase)
        {
            throw new NotImplementedException();
        }

        public IVehicleMakeTypeDto Save(IVehicleMakeTypeDto update)
        {
            throw new NotImplementedException();
        }

        public Task<IVehicleMakeTypeDto> SaveAsync(IVehicleMakeTypeDto update)
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