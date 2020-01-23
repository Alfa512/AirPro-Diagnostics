using System;
using System.Collections.Generic;
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

namespace AirPro.Service.Services.Concrete
{
    internal class GroupService : ServiceBase, IService<IGroupDto>
    {

        internal readonly IQueryable<GroupEntityModel> AllowedGroups;

        public GroupService(IServiceSettings settings) : base(settings)
        {
            // Load Allowed Groups.
            AllowedGroups = UserHasRoles(ApplicationRoles.GroupShowAll)
                ? Db.Groups : Db.UserGroups.Where(a => a.UserGuid == User.UserGuid).Select(a => a.Group);

        }

        public IGroupDto GetById(string id)
        {
            if (!UserHasRoles(ApplicationRoles.GroupView, ApplicationRoles.GroupEdit, ApplicationRoles.GroupCreate)) return null;

            Guid groupGuid = Guid.Parse(id);
            var group = AllowedGroups
                .Include(g => g.Roles)
                .Include(u => u.Roles.Select(r => r.Role))
                .Include(g => g.GroupUsers)
                .Include(g => g.GroupUsers.Select(gu => gu.User))
                .FirstOrDefault(a => a.GroupGuid == groupGuid);

            return group != null ? Mapper.Map<GroupDto>(group) : null;
        }

        public Task<IGroupDto> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public string GetDisplayName(string id)
        {
            // Test Guid.
            Guid groupGuid;
            if (!Guid.TryParse(id, out groupGuid)) return "Invalid ID";

            // Lookup Group.
            return Db.Groups?.FirstOrDefault(g => g.GroupGuid == groupGuid)?.Name ?? "Unknown";
        }

        public Task<string> GetDisplayNameAsync(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<KeyValuePair<string, string>> GetDisplayList(ServiceArgs args = null)
        {
            return AllowedGroups?.ToList().Select(g => new KeyValuePair<string, string>(g.GroupGuid.ToString(), g.Name)).ToList()
                   ?? new List<KeyValuePair<string, string>>();
        }

        public Task<IEnumerable<KeyValuePair<string, string>>> GetDisplayListAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IGroupDto> GetAll(ServiceArgs args = null)
        {
            var result = new List<IGroupDto>();

            if (!UserHasRoles(ApplicationRoles.GroupView, ApplicationRoles.GroupEdit)) return result;

            if (AllowedGroups != null)
                result.AddRange(AllowedGroups
                    .Include(g => g.Roles)
                    .Include(u => u.Roles.Select(r => r.Role))
                    .Include(g => g.GroupUsers)
                    .Include(g => g.GroupUsers.Select(gu => gu.User))
                    .ToList()
                    .Select(Mapper.Map<GroupDto>));

            return result;
        }

        public Task<IEnumerable<IGroupDto>> GetAllAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IGridPageDto<IGroupDto> GetAllByGridPage(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public Task<IGridPageDto<IGroupDto>> GetAllByGridPageAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IGridPageDto<IGroupDto> GetAllByGridPage(int pageNumber, int pageSize, string sort, string searchPhrase)
        {
            // Create Result
            var result = new GridPageDto<IGroupDto>
            {
                Current = pageNumber,
                Rows = new List<IGroupDto>()
            };

            if (!UserHasRoles(ApplicationRoles.GroupView, ApplicationRoles.GroupEdit)) return result;

            // Search Users.
            var groups = string.IsNullOrEmpty(searchPhrase)
                ? AllowedGroups
                : AllowedGroups.Where(g => g.Name.Contains(searchPhrase) || g.Description.Contains(searchPhrase) ||
                                           g.Roles.Any(r => r.Role.Name.Contains(searchPhrase)));

            // Count Results.
            result.Total = groups.Count();

            // Sort Dataset.
            var sorted = string.IsNullOrEmpty(sort) ? groups?.OrderBy(g => g.Name) : groups?.OrderBy(sort);

            // Get Page.
            var page = pageSize < 0 ? sorted : sorted.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            // Set Page.
            result.RowCount = page.Count();
            result.Rows = page
                .Include(u => u.Roles)
                .Include(u => u.Roles.Select(r => r.Role))
                .Include(g => g.GroupUsers)
                .Include(g => g.GroupUsers.Select(gu => gu.User))
                .ToList().Select(Mapper.Map<GroupDto>);

            return result;
        }

        public IGroupDto Save(IGroupDto group)
        {
            GroupEntityModel update;
            UpdateResultDto result = new UpdateResultDto(false, "Unknown Error Occured.");

            // Check for New Group.
            if (group.GroupGuid == Guid.Empty)
            {
                // Verify Access for Add.
                if (!UserHasRoles(ApplicationRoles.GroupCreate))
                {
                    // No Ability to Add.
                    group.UpdateResult = new UpdateResultDto(false, "You do not have access to create a record.");
                    return group;
                }

                // Get Entity.
                update = Mapper.Map<GroupEntityModel>(group);

                // Set Creation.
                update.CreatedByUserGuid = User.UserGuid;
                foreach (var role in update.Roles)
                    role.CreatedByUserGuid = User.UserGuid;

                // Create Default Membership.
                var userGroup = new UserGroupEntityModel()
                {
                    UserGuid = User.UserGuid,
                    Group = update,
                    CreatedByUserGuid = User.UserGuid
                };
                Db.Entry(userGroup).State = EntityState.Added;

                // Add Entity.
                Db.Entry(update).State = EntityState.Added;

                // Set Result.
                result = new UpdateResultDto(true, "Group Created Successfully.");
            }
            else
            {
                // Verify Access to Edit.
                if ((!AllowedGroups?.Any(g => g.GroupGuid == group.GroupGuid) ?? true)
                    || !UserHasRoles(ApplicationRoles.GroupEdit))
                {
                    // No Ability to Edit.
                    group.UpdateResult = new UpdateResultDto(false, "You do not have access to modify this record.");
                    return group;
                }

                // Load Group for Update.s
                update = AllowedGroups.FirstOrDefault(g => g.GroupGuid == group.GroupGuid);

                // Check Group.
                if (update != null)
                {
                    // Update Group.
                    update.Name = group.Name;
                    update.Description = group.Description;
                    update.UpdatedByUserGuid = User.UserGuid;
                    update.UpdatedDt = DateTimeOffset.UtcNow;

                    // Parse Roles for Update/Delete.
                    foreach (var role in update.Roles.ToList())
                    {
                        // Update Existing Record.
                        if (group.Roles?.Any(r => r.Key == role.RoleGuid) ?? false)
                        {
                            role.UpdatedByUserGuid = User.UserGuid;
                            role.UpdatedDt = DateTimeOffset.UtcNow;
                        }
                        else // Delete Existing Record.
                        {
                            Db.GroupRoles.Remove(role);
                        }
                    }

                    // Parse Roles for Add.
                    if (group.Roles != null)
                    {
                        var existing = update.Roles.Select(r => r.RoleGuid).ToList();
                        foreach (var role in group.Roles.Where(r => !existing.Contains(r.Key)))
                        {
                            // Add New Role.
                            var groupRole = new GroupRoleEntityModel()
                            {
                                Group = update,
                                RoleGuid = role.Key,
                                CreatedByUserGuid = User.UserGuid
                            };
                            update.Roles.Add(groupRole);
                        }
                    }

                    // Update Entry.
                    Db.Entry(update).State = EntityState.Modified;

                    // Set Result.
                    result = new UpdateResultDto(true, "Group Updated Successfully.");
                }
            }

            // Save.
            Db.SaveChanges();

            // Update User Roles.
            Db.Database.ExecuteSqlCommand("Access.usp_UserGroupRoleSync");

            // Load Group.
            group = GetById(update?.GroupGuid.ToString() ?? group.GroupGuid.ToString());

            // Set Update Result.
            group.UpdateResult = result;

            return group;
        }

        public Task<IGroupDto> SaveAsync(IGroupDto update)
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
