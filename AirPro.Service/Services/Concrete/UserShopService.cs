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
    internal class UserShopService : ServiceBase, IService<IUserShopDto>
    {
        public UserShopService(IServiceSettings settings) : base(settings)
        {
        }

        public IUserShopDto GetById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IUserShopDto> GetByIdAsync(string id)
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

        public IEnumerable<IUserShopDto> GetAll(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IUserShopDto>> GetAllAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IGridPageDto<IUserShopDto> GetAllByGridPage(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public Task<IGridPageDto<IUserShopDto>> GetAllByGridPageAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IGridPageDto<IUserShopDto> GetAllByGridPage(int pageNumber, int pageSize, string sort, string searchPhrase)
        {
            throw new NotImplementedException();
        }

        public IUserShopDto Save(IUserShopDto update)
        {
            throw new NotImplementedException();
        }

        public Task<IUserShopDto> SaveAsync(IUserShopDto update)
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

                if (!Guid.TryParse(ids[1], out var shopGuid))
                {
                    return false;
                }

                // Verify Access to Edit.
                if (!UserHasRoles(ApplicationRoles.UserEdit))
                {
                    return false;
                }

                var userShopRelation = Db.UserShops.FirstOrDefault(x => x.ShopGuid == shopGuid && x.UserGuid == userGuid);
                if (userShopRelation != null)
                {
                    Db.UserShops.Remove(userShopRelation);
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
