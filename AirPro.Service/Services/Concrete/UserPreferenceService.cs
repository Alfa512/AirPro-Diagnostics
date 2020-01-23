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
    internal class UserPreferenceService : ServiceBase, IService<IUserPreferenceDto>
    {
        public UserPreferenceService(IServiceSettings settings) : base(settings)
        {
        }

        public IUserPreferenceDto GetById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IUserPreferenceDto> GetByIdAsync(string id)
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

        public IEnumerable<IUserPreferenceDto> GetAll(ServiceArgs args = null)
        {
            if (args != null && args.ContainsKey("ControlId") && args.ContainsKey("UserGuid"))
            {
                var controlId = args["ControlId"].ToString();
                var userGuid = Guid.Parse(args["UserGuid"].ToString());

                var sql = @"SELECT * FROM Access.UserPreferences WHERE UserGuid = @UserGuid AND ControlId = @ControlId";

                var param = new
                {
                    UserGuid = userGuid,
                    ControlId = controlId
                };
                var result = Conn.Query<UserPreferenceDto>(sql, param, null, true, null, CommandType.Text).ToList<IUserPreferenceDto>();

                return result;
            }

            throw new NotImplementedException();
        }

        public Task<IEnumerable<IUserPreferenceDto>> GetAllAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IGridPageDto<IUserPreferenceDto> GetAllByGridPage(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public Task<IGridPageDto<IUserPreferenceDto>> GetAllByGridPageAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IGridPageDto<IUserPreferenceDto> GetAllByGridPage(int pageNumber, int pageSize, string sort, string searchPhrase)
        {
            throw new NotImplementedException();
        }

        public IUserPreferenceDto Save(IUserPreferenceDto update)
        {
            var result = Conn.Execute("Access.usp_SaveUserPreferences", update, null,
                null, CommandType.StoredProcedure);

            return update;
        }

        public Task<IUserPreferenceDto> SaveAsync(IUserPreferenceDto update)
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
