using AirPro.Service.DTOs.Concrete;
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
    internal class ClientValidationService : ServiceBase, IService<IClientValidationDto>
    {
        public ClientValidationService(IServiceSettings settings) : base(settings)
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

        public IEnumerable<IClientValidationDto> GetAll(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<IClientValidationDto>> GetAllAsync(ServiceArgs args = null)
        {
            var validationDto = new ClientValidationDto();
            if (args != null)
            {
                if (args.ContainsKey("ShopName"))
                {
                    var shopName = args["ShopName"].ToString();
                    validationDto.IsValid = await Db.Shops.CountAsync(x => x.Name == shopName) == 0;
                }
                if (args.ContainsKey("AccountName"))
                {
                    var shopName = args["AccountName"].ToString();
                    validationDto.IsValid = await Db.Accounts.CountAsync(x => x.Name == shopName) == 0;
                }
                if (args.ContainsKey("UserEmail"))
                {
                    var email = args["UserEmail"].ToString();
                    var registrationCheckQuery = Db.Registrations.Where(x => x.Email == email);
                    if (args.ContainsKey("RegistrationId"))
                    {
                        var registrationId = Guid.Parse(args["RegistrationId"].ToString());
                        registrationCheckQuery = registrationCheckQuery.Where(x => x.RegistrationId != registrationId);

                    }
                    validationDto.IsValid = await Db.Users.Where(x => x.Email == email).CountAsync() == 0 && await registrationCheckQuery.CountAsync() == 0;
                }
            }

            return new List<IClientValidationDto> { validationDto };
        }

        public IGridPageDto<IClientValidationDto> GetAllByGridPage(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IGridPageDto<IClientValidationDto> GetAllByGridPage(int pageNumber, int pageSize, string sort, string searchPhrase)
        {
            throw new NotImplementedException();
        }

        public Task<IGridPageDto<IClientValidationDto>> GetAllByGridPageAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IClientValidationDto GetById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IClientValidationDto> GetByIdAsync(string id)
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

        public string GetDisplayName(string id)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetDisplayNameAsync(string id)
        {
            throw new NotImplementedException();
        }

        public IClientValidationDto Save(IClientValidationDto update)
        {
            throw new NotImplementedException();
        }

        public Task<IClientValidationDto> SaveAsync(IClientValidationDto update)
        {
            throw new NotImplementedException();
        }
    }
}
