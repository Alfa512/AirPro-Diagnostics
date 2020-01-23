using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AirPro.Entities.Common;
using AirPro.Service.DTOs.Concrete;
using AirPro.Service.DTOs.Interface;
using AirPro.Service.Services.Abstract;
using AirPro.Service.Services.Interface;
using AutoMapper;

namespace AirPro.Service.Services.Concrete
{
    internal class StateService : ServiceBase, IService<IStateDto>
    {

        private readonly IQueryable<StateEntityModel> AvailableStates;

        public StateService(IServiceSettings settings) : base(settings)
        {
            // Load Available States.
            AvailableStates = Db.States;
        }

        public IStateDto GetById(string id)
        {
            int stateId = Int32.Parse(id);
            var state = AvailableStates.Include(s => s.Country).FirstOrDefault(s => s.StateId == stateId);
            return state != null ? Mapper.Map<StateDto>(state) : null;
        }

        public Task<IStateDto> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public string GetDisplayName(string id)
        {
            // Test ID.
            int stateId;
            if (Int32.TryParse(id, out stateId)) return "Invalid ID";

            // Lookup State.
            return Db.States?.FirstOrDefault(s => s.StateId == stateId)?.Name ?? "Unknown";
        }

        public Task<string> GetDisplayNameAsync(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<KeyValuePair<string, string>> GetDisplayList(ServiceArgs args = null)
        {
            return AvailableStates?.OrderBy(s => s.CountryId).ThenBy(s => s.Name).ToList().Select(s => new KeyValuePair<string, string>(s.Abbreviation, s.Name)).ToList()
                   ?? new List<KeyValuePair<string, string>>();
        }

        public Task<IEnumerable<KeyValuePair<string, string>>> GetDisplayListAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IStateDto> GetAll(ServiceArgs args = null)
        {
            var result = new List<IStateDto>();

            if (AvailableStates != null)
                result.AddRange(AvailableStates.Include(s => s.Country).ToList().Select(Mapper.Map<StateDto>));

            return result;
        }

        public Task<IEnumerable<IStateDto>> GetAllAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IGridPageDto<IStateDto> GetAllByGridPage(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public Task<IGridPageDto<IStateDto>> GetAllByGridPageAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IGridPageDto<IStateDto> GetAllByGridPage(int pageNumber, int pageSize, string sort, string searchPhrase)
        {
            throw new NotImplementedException();
        }

        public IStateDto Save(IStateDto update)
        {
            throw new NotImplementedException();
        }

        public Task<IStateDto> SaveAsync(IStateDto update)
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
