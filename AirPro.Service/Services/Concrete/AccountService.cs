using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using AirPro.Common.Enumerations;
using AirPro.Entities.Access;
using AirPro.Service.DTOs.Concrete;
using AirPro.Service.DTOs.Interface;
using AirPro.Service.Services.Abstract;
using AirPro.Service.Services.Interface;
using AutoMapper;
using Dapper;

namespace AirPro.Service.Services.Concrete
{
    internal class AccountService : ServiceBase, IService<IAccountDto>
    {

        internal readonly IQueryable<AccountEntityModel> AllowedAccounts;

        public AccountService(IServiceSettings settings) : base(settings)
        {
            // Load Allowed Accounts.
            AllowedAccounts = UserHasRoles(ApplicationRoles.AccountShowAll)
                ? Db.Accounts
                : Db.UserAccounts.Where(a => a.UserGuid == User.UserGuid).Select(a => a.Account);
        }

        public IAccountDto GetById(string id)
        {
            if (!UserHasRoles(ApplicationRoles.AccountView, ApplicationRoles.AccountEdit, ApplicationRoles.AccountCreate))
                return null;

            Guid accountGuid = Guid.Parse(id);
            var account = AllowedAccounts
                .Include(a => a.State)
                .Include(a => a.AccountUsers)
                .Include(a => a.AccountUsers.Select(au => au.User))
                .FirstOrDefault(a => a.AccountGuid == accountGuid);

            return account != null ? Mapper.Map<AccountDto>(account) : null;
        }

        public Task<IAccountDto> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public string GetDisplayName(string id)
        {
            // Test Guid.
            Guid accountGuid;
            if (!Guid.TryParse(id, out accountGuid)) return "Invalid ID";

            // Lookup Account.
            return Db.Accounts?.FirstOrDefault(a => a.AccountGuid == accountGuid)?.Name ?? "Unknown";
        }

        public Task<string> GetDisplayNameAsync(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<KeyValuePair<string, string>> GetDisplayList(ServiceArgs args = null)
        {
            return AllowedAccounts?.Where(a => a.ActiveInd).ToList().OrderBy(a => a.Name)
                       .Select(a => new KeyValuePair<string, string>(a.AccountGuid.ToString(), a.Name)).ToList()
                ?? new List<KeyValuePair<string, string>>();
        }

        public Task<IEnumerable<KeyValuePair<string, string>>> GetDisplayListAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IAccountDto> GetAll(ServiceArgs args = null)
        {
            var result = new List<IAccountDto>();

            if (!UserHasRoles(ApplicationRoles.AccountView, ApplicationRoles.AccountEdit)) return result;
            if (AllowedAccounts != null)
            {
                var query = AllowedAccounts
                    .Include(a => a.State)
                    .Include(a => a.AccountUsers)
                    .Include(a => a.AccountUsers.Select(au => au.User));

                if (args != null && args.ContainsKey("Light") && bool.TryParse(args["Light"].ToString(), out var light) && light)
                {
                    query = AllowedAccounts;
                }

                if (args != null && args.ContainsKey("AccountName") && args["AccountName"] != null)
                {
                    var shopName = args["AccountName"].ToString();
                    query = query.Where(x => x.Name == shopName);
                }

                if (args != null && args.ContainsKey("NotAccountGuid") && Guid.TryParse(args["NotAccountGuid"].ToString(), out var notAccountGuid))
                {
                    query = query.Where(x => x.AccountGuid != notAccountGuid);
                }

                result.AddRange(query.ToList().Select(Mapper.Map<AccountDto>));
            }

            return result;
        }

        public Task<IEnumerable<IAccountDto>> GetAllAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IGridPageDto<IAccountDto> GetAllByGridPage(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public Task<IGridPageDto<IAccountDto>> GetAllByGridPageAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IGridPageDto<IAccountDto> GetAllByGridPage(int pageNumber, int pageSize, string sort, string searchPhrase)
        {
            // Create Result
            var result = new GridPageDto<IAccountDto>
            {
                Current = pageNumber,
                Rows = new List<IAccountDto>()
            };

            if (!UserHasRoles(ApplicationRoles.AccountView, ApplicationRoles.AccountEdit)) return result;

            // Search Accounts.
            var shops = string.IsNullOrEmpty(searchPhrase) ? AllowedAccounts
                : AllowedAccounts.Where(s => s.Name.Contains(searchPhrase) || s.Address1.Contains(searchPhrase) || 
                                        s.Address2.Contains(searchPhrase) || s.City.Contains(searchPhrase) ||
                                        s.State.Name.Contains(searchPhrase) || s.Zip.Contains(searchPhrase) ||
                                        (s.EmployeeGuid != null && s.Employee.DisplayName.Contains(searchPhrase)) || s.Phone.Contains(searchPhrase) || s.Fax.Contains(searchPhrase));

            // Count Results.
            result.Total = shops.Count();

            // Sort Dataset.
            var sorted = string.IsNullOrEmpty(sort) ? shops?.OrderBy(u => u.Name) : shops?.OrderBy(sort);

            // Get Page.
            var page = pageSize < 0 ? sorted : sorted.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            // Set Page.
            result.RowCount = page.Count();
            result.Rows = page.Include(i => i.AccountUsers)
                .Include(i => i.AccountUsers.Select(d => d.User))
                .Include(a => a.State)
                .Include(a => a.Employee)
                .Include(a => a.AccountUsers)
                .Include(a => a.AccountUsers.Select(au => au.User))
                .ToList().Select(Mapper.Map<AccountDto>);

            return result;
        }

        public IAccountDto Save(IAccountDto account)
        {
            AccountEntityModel update;
            UpdateResultDto result = new UpdateResultDto(false, "Unknown Error Occured.");

            // Limit Discount Percentage.
            if (account.DiscountPercentage > 100) account.DiscountPercentage = 100;

            // Check for New Account.
            if (account.AccountGuid == Guid.Empty)
            {
                // Verify Access for Add.
                if (!UserHasRoles(ApplicationRoles.AccountCreate))
                {
                    // No Ability to Add.
                    account.UpdateResult = new UpdateResultDto(false, "You do not have access to create a record.");
                    return account;
                }

                // Get Entity.
                update = Mapper.Map<AccountEntityModel>(account);

                // Allow Set Discount.
                if (!UserHasRoles(ApplicationRoles.PaymentCreate))
                {
                    update.DiscountPercentage = 0;
                }

                // Set Creation.
                update.CreatedByUserGuid = User.UserGuid;
                update.CreatedDt = DateTimeOffset.UtcNow;

                if (!UserHasRoles(ApplicationRoles.AccountShowAll))
                {
                    update.AccountUsers = new List<UserAccountEntityModel>
                    {
                        new UserAccountEntityModel
                        {
                            UserGuid = User.UserGuid,
                            CreatedByUserGuid = User.UserGuid,
                            CreatedDt = DateTimeOffset.UtcNow
                        }
                    };
                }

                Db.Accounts.Add(update);

                // Create Result.
                result = new UpdateResultDto(true, "Account Created Successfully.");
            }
            else
            {
                // Verify Access to Edit.
                if ((!AllowedAccounts?.Any(a => a.AccountGuid == account.AccountGuid) ?? true)
                    || !UserHasRoles(ApplicationRoles.AccountEdit))
                {
                    // No Ability to Edit.
                    account.UpdateResult = new UpdateResultDto(false, "You do not have access to modify this record.");
                    return account;
                }

                // Load Account for Update.
                update = AllowedAccounts.FirstOrDefault(a => a.AccountGuid == account.AccountGuid);

                // Check Account.
                if (update != null)
                {
                    // Update Account Fields.
                    update.Name = account.Name;
                    update.Address1 = account.Address1;
                    update.Address2 = account.Address2;
                    update.City = account.City;
                    update.State = Db.States.FirstOrDefault(s => s.Abbreviation == account.State);
                    update.Zip = account.Zip;
                    update.Phone = account.Phone;
                    update.Fax = account.Fax;
                    update.EmployeeGuid = account.EmployeeGuid;

                    // Check Active.
                    if (account.ActiveInd) update.ActiveInd = account.ActiveInd;

                    // Check Payment Create Permission.
                    if (UserHasRoles(ApplicationRoles.PaymentCreate))
                    {
                        update.DiscountPercentage = account.DiscountPercentage;
                    }

                    // Set Update.
                    update.UpdatedByUserGuid = User.UserGuid;
                    update.UpdatedDt = DateTimeOffset.UtcNow;
                    Db.Entry(update).State = EntityState.Modified;

                    // Create Result.
                    result = new UpdateResultDto(true, "Account Updated Successfully.");
                }
            }

            // Save.
            Db.SaveChanges();

            // Load Account.
            account = GetById(update?.AccountGuid.ToString() ?? account.AccountGuid.ToString());

            // Set Update Results.
            account.UpdateResult = result;

            return account;
        }

        public Task<IAccountDto> SaveAsync(IAccountDto update)
        {
            throw new NotImplementedException();
        }

        public bool Delete(string id)
        {
            var result = false;

            try
            {
                if (Guid.TryParse(id, out var accountGuid))
                {
                    var param = new
                    {
                        AccountGuid = accountGuid,
                        UserGuid = User.UserGuid
                    };

                    Conn.Execute("Access.usp_DeleteAccount", param, null, null, CommandType.StoredProcedure);
                }

                result = true;
            }
            catch (Exception e)
            {
                Logging.Logger.LogException(e);
            }

            return result;
        }

        public Task<bool> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}
