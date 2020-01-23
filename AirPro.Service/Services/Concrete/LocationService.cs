using AirPro.Service.DTOs.Interface;
using AirPro.Service.Services.Abstract;
using AirPro.Service.Services.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace AirPro.Service.Services.Concrete
{
    internal class LocationService : ServiceBase, IService<ILocationDto>
    {
        public LocationService(IServiceSettings settings) : base(settings)
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

        public IEnumerable<ILocationDto> GetAll(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ILocationDto>> GetAllAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IGridPageDto<ILocationDto> GetAllByGridPage(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IGridPageDto<ILocationDto> GetAllByGridPage(int pageNumber, int pageSize, string sort, string searchPhrase)
        {
            throw new NotImplementedException();
        }

        public Task<IGridPageDto<ILocationDto>> GetAllByGridPageAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public ILocationDto GetById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<ILocationDto> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<KeyValuePair<string, string>> GetDisplayList(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<KeyValuePair<string, string>>> GetDisplayListAsync(ServiceArgs args = null)
        {
            return (await Db.Locations.OrderBy(x => x.SortOrder).ToListAsync()).ToDictionary(x => x.LocationId.ToString(), x => x.Name).ToArray();
        }

        public string GetDisplayName(string id)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetDisplayNameAsync(string id)
        {
            throw new NotImplementedException();
        }

        public ILocationDto Save(ILocationDto update)
        {
            throw new NotImplementedException();
        }

        public Task<ILocationDto> SaveAsync(ILocationDto update)
        {
            throw new NotImplementedException();
        }
    }
}
