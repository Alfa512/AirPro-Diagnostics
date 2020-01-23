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
    internal class UserService : ServiceBase, IService<IUserDto>
    {

        internal readonly IQueryable<UserEntityModel> AllowedUsers;

        public UserService(IServiceSettings settings) : base(settings)
        {
            // Populate Allowed Users.
            if (UserHasRoles(ApplicationRoles.AccountShowAll, ApplicationRoles.ShopShowAll, ApplicationRoles.UserShowAll))
            {
                AllowedUsers = Db.Users.Where(u => u.Id != Guid.Empty);
            }
            else
            {
                // Load Shop & Account Guids.
                var allowedShops = new ShopService(settings).AllowedShops.Select(s => s.ShopGuid);
                var allowedAccounts = new AccountService(settings).AllowedAccounts.Select(a => a.AccountGuid);

                // Load Allowed Users.
                AllowedUsers = Db.UserAccounts.Where(ua => allowedAccounts.Contains(ua.AccountGuid)).Select(ua => ua.User)
                    .Union(Db.UserShops.Where(us => allowedShops.Contains(us.ShopGuid)).Select(us => us.User)).Where(u => u.Id != Guid.Empty);
            }
        }

        public IUserDto GetById(string id)
        {
            if (!UserHasRoles(ApplicationRoles.UserView, ApplicationRoles.UserEdit)) return null;

            Guid userGuid = Guid.Parse(id);
            var user = AllowedUsers.Include(u => u.UserAccounts).Include(u => u.UserShops).Include(u => u.UserGroups).Include(x => x.EmployeeAccounts).Include(x => x.EmployeeShops).FirstOrDefault(s => s.Id == userGuid);
            return user != null ? Mapper.Map<UserDto>(user) : null;
        }

        public Task<IUserDto> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public string GetDisplayName(string id)
        {
            // Test Guid.
            Guid userGuid;
            if (!Guid.TryParse(id, out userGuid)) return "Invalid ID";

            // Lookup User.
            var displayName = Db.Users?.Find(userGuid)?.GetDisplayName;

            // Return Display Name.
            return displayName ?? "Unknown";
        }

        public Task<string> GetDisplayNameAsync(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<KeyValuePair<string, string>> GetDisplayList(ServiceArgs args = null)
        {
            if (args != null && args.ContainsKey("RepairId") && int.TryParse(args["RepairId"].ToString(), out var repairId) && args.ContainsKey("ShopGuid") && Guid.TryParse(args["ShopGuid"].ToString(), out var shopGuid))
            {
                var users = Conn.Query<UserEntityModel>("Access.usp_GetUsersByRepairId", new { RepairId = repairId == 0 ? null : new int?(repairId), ShopGuid = shopGuid == Guid.Empty ? null : new Guid?(shopGuid) }, null, true, null, CommandType.StoredProcedure);
                return users.Select(x => new KeyValuePair<string, string>(x.Id.ToString(), x.GetDisplayName));

            }

            return AllowedUsers?.OrderBy(u => u.LastName).ThenBy(u => u.FirstName).AsEnumerable()
                .Select(u => new KeyValuePair<string, string>(u.Id.ToString(), u.GetDisplayName));
        }

        public Task<IEnumerable<KeyValuePair<string, string>>> GetDisplayListAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IUserDto> GetAll(ServiceArgs args = null)
        {
            var result = new List<IUserDto>();

            if (!UserHasRoles(ApplicationRoles.UserView, ApplicationRoles.UserEdit)) return result;

            if (AllowedUsers != null)
                result.AddRange(AllowedUsers
                    .Include(u => u.UserAccounts)
                    .Include(u => u.UserShops)
                    .Include(u => u.UserGroups)
                    .ToList().Select(Mapper.Map<UserDto>));

            return result;
        }

        public Task<IEnumerable<IUserDto>> GetAllAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IGridPageDto<IUserDto> GetAllByGridPage(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public Task<IGridPageDto<IUserDto>> GetAllByGridPageAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IGridPageDto<IUserDto> GetAllByGridPage(int pageNumber, int pageSize, string sort, string searchPhrase)
        {
            // Create Result.
            var result = new GridPageDto<IUserDto>
            {
                Current = pageNumber,
                Rows = new List<IUserDto>()
            };

            if (!UserHasRoles(ApplicationRoles.UserView, ApplicationRoles.UserEdit)) return result;

            // Search Users.
            var users = string.IsNullOrEmpty(searchPhrase) ? AllowedUsers
                        : AllowedUsers.Where(u => u.Email.Contains(searchPhrase) || u.LastName.Contains(searchPhrase) ||
                                            (u.FirstName + " " + u.LastName).Contains(searchPhrase) ||
                                            (u.LastName + ", " + u.FirstName).Contains(searchPhrase) ||
                                            u.FirstName.Contains(searchPhrase) || u.JobTitle.Contains(searchPhrase) ||
                                            u.ContactNumber.Contains(searchPhrase) || u.PhoneNumber.Contains(searchPhrase) ||
                                            u.UserGroups.Any(g => g.Group.Name.Contains(searchPhrase)) || u.UserRoles.Any(r => r.Role.Name.Contains(searchPhrase)) ||
                                            u.UserAccounts.Any(a => a.Account.Name.Contains(searchPhrase)) || u.UserShops.Any(s => s.Shop.Name.Contains(searchPhrase)));

            // Count Results.
            result.Total = users.Count();

            // Sort Dataset.
            var sorted = string.IsNullOrEmpty(sort) ? users?.OrderBy(u => u.Email) : users?.OrderBy(sort);

            // Get Page.
            var page = pageSize < 0 ? sorted : sorted.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            // Set Page.
            result.RowCount = page.Count();
            result.Rows = page.Include(u => u.UserAccounts).Include(u => u.UserShops).Include(u => u.UserGroups).Include(x => x.EmployeeAccounts).Include(x => x.EmployeeShops).Select(Mapper.Map<UserDto>);

            return result;
        }

        public IUserDto Save(IUserDto user)
        {
            UserEntityModel update;
            UpdateResultDto result = new UpdateResultDto(false, "Unknown Error Occured.");

            // Verify Groups.
            var allowedGroups = new GroupService(Settings).AllowedGroups.Select(g => g.GroupGuid);
            if (user.GroupMemberships?.Any(g => !allowedGroups.Contains(g)) ?? false)
            {
                // No Group Access.
                user.UpdateResult = new UpdateResultDto(false, "You do not have access to assign selected Group(s).");
                return user;
            }

            // Verify Shops.
            var allowedShops = new ShopService(Settings).AllowedShops.Select(s => s.ShopGuid);
            if (user.ShopMemberships?.Any(s => !allowedShops.Contains(s)) ?? false)
            {
                // No Shop Access.
                user.UpdateResult = new UpdateResultDto(false, "You do not have access to assign selected Shop(s).");
                return user;
            }

            // Verify Accounts.
            var allowedAccounts = new AccountService(Settings).AllowedAccounts.Select(a => a.AccountGuid);
            if (user.AccountMemberships?.Any(a => !allowedAccounts.Contains(a)) ?? false)
            {
                // No Account Access.
                user.UpdateResult = new UpdateResultDto(false, "You do not have access to assign selected Account(s).");
                return user;
            }

            // Check for New User.
            if (user.UserGuid == Guid.Empty)
            {
                // Verify Access for Add.
                if (!UserHasRoles(ApplicationRoles.UserCreate))
                {
                    // No Ability to Add.
                    user.UpdateResult = new UpdateResultDto(false, "You do not have access to create a record.");
                    return user;
                }

                // Check Duplicate Address.
                if (Db.Users.Any(u => u.Email == user.Email))
                {
                    // Duplicate Account.
                    user.UpdateResult = new UpdateResultDto(false, "An Account with this Email already Exists.");
                    return user;
                }

                // Get Entity.
                update = Mapper.Map<UserEntityModel>(user);

                // Verify Employee Assignment.
                if (!UserHasRoles(ApplicationRoles.AirProEmployeeAssign))
                    update.EmployeeInd = false;

                // Set Created.
                update.CreatedByUserGuid = User.UserGuid;
                update.CreatedDt = DateTimeOffset.UtcNow;

                // Generate Security Stamp.
                update.LockoutEnabled = true;
                update.SecurityStamp = Guid.NewGuid().ToString();

                // Set Creations.
                foreach (var account in update.UserAccounts ?? new List<UserAccountEntityModel>())
                    account.CreatedByUserGuid = User.UserGuid;

                foreach (var shop in update.UserShops ?? new List<UserShopEntityModel>())
                    shop.CreatedByUserGuid = User.UserGuid;

                foreach (var group in update.UserGroups ?? new List<UserGroupEntityModel>())
                    group.CreatedByUserGuid = User.UserGuid;

                // Add Entity.
                Db.Users.Add(update);

                // Set Result.
                result = new UpdateResultDto(true, "User Created Successfully.");
            }
            else
            {
                // Verify Access to Edit.
                if ((!AllowedUsers?.Any(u => u.Id == user.UserGuid) ?? true) || !UserHasRoles(ApplicationRoles.UserEdit))
                {
                    // No Ability to Edit.
                    user.UpdateResult = new UpdateResultDto(false, "You do not have access to modify this record.");
                    return user;
                }

                // Load User for Update.
                update = AllowedUsers.Include(x => x.EmployeeShops).Include(x => x.EmployeeAccounts).FirstOrDefault(u => u.Id == user.UserGuid);

                // Check User.
                if (update != null)
                {
                    // Verify Employee Assignment.
                    if (UserHasRoles(ApplicationRoles.AirProEmployeeAssign))
                    {
                        if (update.EmployeeInd != user.EmployeeInd && !update.EmployeeShops.Any(x => x.ActiveInd) && !update.EmployeeAccounts.Any(x => x.ActiveInd))
                            update.EmployeeInd = user.EmployeeInd;
                    }

                    // Update User Info.
                    if (update.FirstName != user.FirstName)
                        update.FirstName = user.FirstName;
                    if (update.LastName != user.LastName)
                        update.LastName = user.LastName;
                    if (update.JobTitle != user.JobTitle)
                        update.JobTitle = user.JobTitle;
                    if (update.ContactNumber != user.ContactNumber)
                        update.ContactNumber = user.ContactNumber;
                    if (update.TimeZoneInfoId != user.TimeZoneInfoId)
                        update.TimeZoneInfoId = user.TimeZoneInfoId;

                    // Check Password.
                    if (user.PasswordHash != null && update.PasswordHash != user.PasswordHash)
                    {
                        update.PasswordHash = user.PasswordHash;
                        update.SecurityStamp = Guid.NewGuid().ToString();
                    }

                    // Check Email.
                    if (user.Email != update.Email)
                    {
                        // Update Email.
                        update.Email = user.Email;
                        update.UserName = user.Email;
                        update.EmailConfirmed = false;
                    }

                    // Check Phone.
                    if (user.PhoneNumber != update.PhoneNumber)
                    {
                        // Update Phone.
                        update.PhoneNumber = user.PhoneNumber;
                        update.PhoneNumberConfirmed = false;
                    }

                    // Lock/Unlock Account.
                    update.LockoutEndDateUtc = user.AccountLocked ? DateTime.MaxValue : new DateTime?();

                    // Reset Two-Factor.
                    if (!update.EmailConfirmed && !update.PhoneNumberConfirmed)
                        update.TwoFactorEnabled = false;
                    else
                        update.TwoFactorEnabled = user.TwoFactorEnabled;

                    // Update Notifications.
                    if (update.ShopReportNotification != user.ShopReportNotification)
                        update.ShopReportNotification = user.ShopReportNotification;
                    if (update.ShopBillingNotification != user.ShopBillingNotification)
                        update.ShopBillingNotification = user.ShopBillingNotification;
                    if (update.ShopStatementNotification != user.ShopStatementNotification)
                        update.ShopStatementNotification = user.ShopStatementNotification;

                    // Set Updated.
                    update.UpdatedByUserGuid = User.UserGuid;
                    update.UpdatedDt = DateTimeOffset.UtcNow;

                    // Parse Accounts for Updates/Deletes.
                    foreach (var account in update.UserAccounts.ToList())
                    {
                        // Update Existing Records.
                        if (user.AccountMemberships?.Contains(account.AccountGuid) ?? false)
                        {
                            account.UpdatedByUserGuid = User.UserGuid;
                            account.UpdatedDt = DateTimeOffset.UtcNow;
                        }
                        else if (allowedAccounts.Contains(account.AccountGuid))// Delete Existing Record.
                        {
                            Db.UserAccounts.Remove(account);
                        }
                    }

                    // Parse Accounts for Add.
                    if (user.AccountMemberships != null)
                    {
                        var accounts = update.UserAccounts.Select(a => a.AccountGuid).ToList();
                        foreach (var account in user.AccountMemberships.Where(a => !accounts.Contains(a)))
                        {
                            // Add New Account.
                            var add = new UserAccountEntityModel()
                            {
                                User = update,
                                AccountGuid = account,
                                CreatedByUserGuid = User.UserGuid
                            };
                            update.UserAccounts.Add(add);
                        }
                    }

                    // Parse Shops for Updates/Deletes.
                    foreach (var shop in update.UserShops.ToList())
                    {
                        // Update Existing Records.
                        if (user.ShopMemberships?.Contains(shop.ShopGuid) ?? false)
                        {
                            shop.UpdatedByUserGuid = User.UserGuid;
                            shop.UpdatedDt = DateTimeOffset.UtcNow;
                        }
                        else if (allowedShops.Contains(shop.ShopGuid))// Delete Existing Record.
                        {
                            Db.UserShops.Remove(shop);
                        }
                    }

                    // Parse Shops for Add.
                    if (user.ShopMemberships != null)
                    {
                        var shops = update.UserShops.Select(s => s.ShopGuid).ToList();
                        foreach (var shop in user.ShopMemberships.Where(s => !shops.Contains(s)))
                        {
                            // Add New Shop.
                            var add = new UserShopEntityModel()
                            {
                                User = update,
                                ShopGuid = shop,
                                CreatedByUserGuid = User.UserGuid
                            };
                            update.UserShops.Add(add);
                        }
                    }

                    // Parse Groups for Updates/Deletes.
                    foreach (var group in update.UserGroups.ToList())
                    {
                        // Update Existing Records.
                        if (user.GroupMemberships?.Contains(group.GroupGuid) ?? false)
                        {
                            group.UpdatedByUserGuid = User.UserGuid;
                            group.UpdatedDt = DateTimeOffset.UtcNow;
                        }
                        else if(allowedGroups.Contains(group.GroupGuid))// Delete Existing Record.
                        {
                            Db.UserGroups.Remove(group);
                        }
                    }

                    // Parse Groups for Add.
                    if (user.GroupMemberships != null)
                    {
                        var groups = update.UserGroups.Select(g => g.GroupGuid).ToList();
                        foreach (var group in user.GroupMemberships.Where(g => !groups.Contains(g)))
                        {
                            // Add New Group.
                            var add = new UserGroupEntityModel()
                            {
                                User = update,
                                GroupGuid = group,
                                CreatedByUserGuid = User.UserGuid
                            };
                            update.UserGroups.Add(add);
                        }
                    }

                    // Set Result.
                    result = new UpdateResultDto(true, "User Updated Successfully.");
                }
            }

            // Save Changes.
            Db.SaveChanges();

            // Update User Roles.
            Db.Database.ExecuteSqlCommand("Access.usp_UserGroupRoleSync");

            // Load Account Record.
            Db.Entry(update).Reload();

            // Load User.
            user = Mapper.Map<UserDto>(update);

            // Set Result.
            user.UpdateResult = result;

            return user;
        }

        public Task<IUserDto> SaveAsync(IUserDto update)
        {
            throw new NotImplementedException();
        }

        public bool Delete(string id)
        {
            if (!Guid.TryParse(id, out var userGuid))
            {
                return false;
            }

            // Verify Access to Edit.
            if ((!AllowedUsers?.Any(u => u.Id == userGuid) ?? true) || !UserHasRoles(ApplicationRoles.UserEdit))
            {
                return false;
            }

            // Load User for Update.
            var update = AllowedUsers.Include(x => x.EmployeeShops).Include(x => x.EmployeeAccounts).FirstOrDefault(u => u.Id == userGuid);
            if (update == null)
            {
                return false;
            }

            if (update.EmployeeInd && (update.EmployeeAccounts.Any() || update.EmployeeShops.Any()))
            {
                throw new Exception("This AirPro Employee assigned to active shop or account. To lock please unassign first.");
            }

            update.LockoutEnabled = true;
            update.LockoutEndDateUtc = DateTime.MaxValue;

            return Db.SaveChanges() > 0;
        }

        public Task<bool> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}
