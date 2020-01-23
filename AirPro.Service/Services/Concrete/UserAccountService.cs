using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AirPro.Common.Enumerations;
using AirPro.Logging;
using AirPro.Service.DTOs.Interface;
using AirPro.Service.Services.Abstract;
using AirPro.Service.Services.Interface;

namespace AirPro.Service.Services.Concrete
{
    internal class UserAccountService : ServiceBase, IService<IUserAccountDto>
    {
        public UserAccountService(IServiceSettings settings) : base(settings)
        {
        }

        public IUserAccountDto GetById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IUserAccountDto> GetByIdAsync(string id)
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

        public IEnumerable<IUserAccountDto> GetAll(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IUserAccountDto>> GetAllAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IGridPageDto<IUserAccountDto> GetAllByGridPage(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public Task<IGridPageDto<IUserAccountDto>> GetAllByGridPageAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IGridPageDto<IUserAccountDto> GetAllByGridPage(int pageNumber, int pageSize, string sort, string searchPhrase)
        {
            throw new NotImplementedException();
        }

        public IUserAccountDto Save(IUserAccountDto update)
        {
            throw new NotImplementedException();
        }

        public Task<IUserAccountDto> SaveAsync(IUserAccountDto update)
        {
            throw new NotImplementedException();
        }

        public bool Delete(string id)
        {
            try
            {
                string[] ids = id.Split('|');
                if (!Guid.TryParse(ids[0], out var userGuid))
                {
                    return false;
                }

                if (!Guid.TryParse(ids[1], out var accountGuid))
                {
                    return false;
                }

                // Verify Access to Edit.
                if (!UserHasRoles(ApplicationRoles.UserEdit))
                {
                    return false;
                }

                var userAccountRelation = Db.UserAccounts.FirstOrDefault(x => x.AccountGuid == accountGuid && x.UserGuid == userGuid);
                if (userAccountRelation != null)
                {
                    Db.UserAccounts.Remove(userAccountRelation);
                    Db.SaveChanges();
                    return true;
                }
            }
            catch (Exception e)
            {
                // ignored
                Logger.LogException(e);
            }

            return false;
        }

        public Task<bool> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}
