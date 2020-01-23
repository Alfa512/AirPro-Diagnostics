using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AirPro.Common.Enumerations;
using AirPro.Service.DTOs.Concrete;
using AirPro.Service.DTOs.Interface;
using AirPro.Service.Services.Abstract;
using AirPro.Service.Services.Interface;
using AutoMapper;
using System.Linq.Dynamic;
using AirPro.Entities.Common;
using AirPro.Logging;

namespace AirPro.Service.Services.Concrete
{
    internal class ReleaseNoteService : ServiceBase, IService<IReleaseNoteDto>
    {
        public ReleaseNoteService(IServiceSettings settings) : base(settings)
        {
        }

        public IReleaseNoteDto GetById(string id)
        {
            var releaseNoteId = int.Parse(id);
            var releaseNote = Db.ReleaseNotes
                .Include(r => r.CreatedBy)
                .Include(r => r.UpdatedBy)
                .Include(r => r.ReleaseNoteRoles)
                .FirstOrDefault(s => s.ReleaseNoteId == releaseNoteId);

            if (releaseNote != null)
            {
                var result = Mapper.Map<ReleaseNoteDto>(releaseNote);
                result.ImpactedRoleGuids = releaseNote.ReleaseNoteRoles.Select(x => x.RoleGuid).ToArray();
                return result;
            }

            return null;
        }

        public Task<IReleaseNoteDto> GetByIdAsync(string id)
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

        public async Task<IEnumerable<KeyValuePair<string, string>>> GetDisplayListAsync(ServiceArgs args = null)
        {
            return (await Db.ReleaseNotes.ToListAsync()).ToDictionary(x => x.ReleaseNoteId.ToString(), x => x.Version);
        }

        public IEnumerable<IReleaseNoteDto> GetAll(ServiceArgs args = null)
        {
            string version = null, userGuid = null;
            if (args != null && args.ContainsKey("Version"))
            {
                version = args["Version"].ToString();
            }
            if (args != null && args.ContainsKey("UserGuid"))
            {
                userGuid = args["UserGuid"].ToString();
            }

            var query = GetReleaseNoteQuery(userGuid);

            if (!string.IsNullOrWhiteSpace(version))
            {
                query = query.Where(x => x.Version == version);
            }

            return query.ToList();
        }

        public Task<IEnumerable<IReleaseNoteDto>> GetAllAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IGridPageDto<IReleaseNoteDto> GetAllByGridPage(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public Task<IGridPageDto<IReleaseNoteDto>> GetAllByGridPageAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        private IQueryable<ReleaseNoteEntityModel> ActiveReleaseNotes
        {
            get { return Db.ReleaseNotes.Where(r => !r.DeletedInd); }
        }

        public IGridPageDto<IReleaseNoteDto> GetAllByGridPage(int pageNumber, int pageSize, string sort, string searchPhrase)
        {
            // Create Result
            var result = new GridPageDto<IReleaseNoteDto>
            {
                Current = pageNumber,
                Rows = new List<IReleaseNoteDto>()
            };

            if (!UserHasRoles(ApplicationRoles.ReleaseNoteView, ApplicationRoles.ReleaseNoteEdit)) return result;

            var query = GetReleaseNoteQuery();

            // Search Release Notess.
            var releaseNotes = string.IsNullOrEmpty(searchPhrase) ? query
                : query.Where(s => s.ReleaseNote.Contains(searchPhrase) || s.Summary.Contains(searchPhrase) ||
                                          s.Version.Contains(searchPhrase) || s.DevelopmentId.Contains(searchPhrase) || s.UpdatedBy.Contains(searchPhrase));

            // Count Results.
            result.Total = releaseNotes.Count();

            // Sort Dataset.
            var sorted = string.IsNullOrEmpty(sort) ? releaseNotes?.OrderBy(u => u.UpdatedDt) : releaseNotes?.OrderBy(sort);

            // Get Page.
            var page = pageSize < 0 ? sorted : sorted.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            // Set Page.
            result.RowCount = page.Count();
            result.Rows = page.ToList();

            return result;
        }

        private IQueryable<ReleaseNoteDto> GetReleaseNoteQuery(string userGuid = null)
        {
            var query = ActiveReleaseNotes;
            if (!string.IsNullOrWhiteSpace(userGuid))
            {
                Guid userId = Guid.Parse(userGuid);
                query = from q in query
                    join releaseNoteRole in Db.ReleaseNoteRoles on q.ReleaseNoteId equals releaseNoteRole.ReleaseNoteId
                    join role in Db.Roles on releaseNoteRole.RoleGuid equals  role.Id
                    join userRole in Db.UserRoles on role.Id equals userRole.RoleId
                    join user in Db.Users on userRole.UserId equals user.Id
                    where user.Id == userId
                    select q;
                query = query.GroupBy(x => x.ReleaseNoteId).Select(x => x.FirstOrDefault());
            }

            return query.Select(x => new ReleaseNoteDto
            {
                Summary = x.Summary,
                DevelopmentId = x.DevelopmentId,
                Version = x.Version,
                ReleaseNote = x.ReleaseNote,
                UpdatedBy = x.UpdatedByUserGuid == null
                    ? x.CreatedBy.LastName + ", " + x.CreatedBy.FirstName
                    : x.UpdatedBy.LastName + ", " + x.UpdatedBy.FirstName,
                ReleaseNoteId = x.ReleaseNoteId,
                UpdatedDt = x.UpdatedDt ?? x.CreatedDt
            });
        }

        public IReleaseNoteDto Save(IReleaseNoteDto releaseNote)
        {
            ReleaseNoteEntityModel update;
            UpdateResultDto result = new UpdateResultDto(false, "Unknown Error Occured.");


            // Check for New Release Note.
            if (releaseNote.ReleaseNoteId == 0)
            {
                // Get Entity.
                update = Mapper.Map<ReleaseNoteEntityModel>(releaseNote);

                // Set Created.
                update.CreatedByUserGuid = User.UserGuid;
                update.CreatedDt = DateTimeOffset.UtcNow;
                foreach (var releaseNoteRoleEntityModel in update.ReleaseNoteRoles ?? new List<ReleaseNoteRoleEntityModel>())
                {
                    releaseNoteRoleEntityModel.CreatedByUserGuid = User.UserGuid;
                }

                // Add Entity.
                Db.ReleaseNotes.Add(update);

                // Set Result.
                result = new UpdateResultDto(true, "Release Note Created Successfully.");
            }
            else
            {
                // Load Release Note for Update.
                update = Db.ReleaseNotes.FirstOrDefault(s => s.ReleaseNoteId == releaseNote.ReleaseNoteId);

                // Check Release Note.
                if (update != null)
                {
                    // Update Release Note Fields.
                    update.ReleaseNote = releaseNote.ReleaseNote;
                    update.Summary = releaseNote.Summary;
                    update.DevelopmentId = releaseNote.DevelopmentId;
                    update.Version = releaseNote.Version;

                    update.ReleaseNoteRoles.Clear();
                    update.ReleaseNoteRoles = releaseNote.ImpactedRoleGuids?.Select(x =>
                            new ReleaseNoteRoleEntityModel() {ReleaseNoteId = releaseNote.ReleaseNoteId, RoleGuid = x, CreatedByUserGuid = User.UserGuid})
                        .ToList();

                    // Set Updated.
                    update.UpdatedByUserGuid = User.UserGuid;
                    update.UpdatedDt = DateTimeOffset.UtcNow;

                    // Update Entry.
                    Db.Entry(update).State = EntityState.Modified;

                    // Set Result.
                    result = new UpdateResultDto(true, "Release Note Updated Successfully.");
                }
            }

            // Save.
            Db.SaveChanges();

            // Load Release Note.
            releaseNote = GetById(update?.ReleaseNoteId.ToString() ?? releaseNote.ReleaseNoteId.ToString());

            // Set Update Result.
            releaseNote.UpdateResult = result;

            return releaseNote;
        }

        public Task<IReleaseNoteDto> SaveAsync(IReleaseNoteDto update)
        {
            throw new NotImplementedException();
        }

        public bool Delete(string id)
        {
            try
            {
                // Check Identity.
                if (!int.TryParse(id, out var resultId)) return false;

                // Find Result.
                var result = Db.ReleaseNotes.Find(resultId);

                // Check Result.
                if (result == null) return false;

                // Flip Deleted Ind.
                result.DeletedInd = !result.DeletedInd;
                result.UpdatedByUserGuid = User.UserGuid;
                result.UpdatedDt = DateTimeOffset.UtcNow;

                // Modify State.
                Db.Entry(result).State = EntityState.Modified;

                // Save Changes.
                Db.SaveChanges();

                // Result.
                return true;
            }
            catch (Exception e)
            {
                // Log Error.
                Logger.LogException(e);
            }

            // Default.
            return false;
        }

        public Task<bool> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}
