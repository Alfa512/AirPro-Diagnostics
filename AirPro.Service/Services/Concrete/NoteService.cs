using AirPro.Entities.Common;
using AirPro.Service.DTOs.Concrete;
using AirPro.Service.DTOs.Interface;
using AirPro.Service.Services.Abstract;
using AirPro.Service.Services.Interface;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using UniMatrix.Common.Extensions;

namespace AirPro.Service.Services.Concrete
{
    internal class NoteService : ServiceBase, IService<INoteDto>
    {
        public NoteService(IServiceSettings settings) : base(settings)
        {
        }

        public INoteDto GetById(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<INoteDto> GetByIdAsync(string id)
        {
            if (!int.TryParse(id, out var noteId)) return null;

            var note = await Db.Notes.Include(x => x.CreatedBy).Include(x => x.UpdatedBy).FirstOrDefaultAsync(u => u.NoteId == noteId);
            var result = Mapper.Map<NoteEntityModel, NoteDto>(note, opts => opts.AfterMap((src, dest) =>
            {
                dest.UpdatedDateTime = src.UpdatedDt == null ? src.CreatedDt.ConvertToUserTime(User.TimeZoneInfoId) : src.UpdatedDt.Value.ConvertToUserTime(User.TimeZoneInfoId);
                dest.UpdatedByUser = src.UpdatedByUserGuid == null ? src.CreatedBy.GetDisplayName : src.UpdatedBy.GetDisplayName;
            }));

            return result;
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

        public IEnumerable<INoteDto> GetAll(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<INoteDto>> GetAllAsync(ServiceArgs args = null)
        {
            // Create Result.
            var result = new List<NoteDto>();

            // Load Uploads.
            var query = Db.Notes.Where(u => !u.NoteDeletedInd);

            // Check Upload Type & Key.
            if (!(args?.ContainsKey("NoteTypeId") ?? false) || !args.ContainsKey("NoteKey") ||
                !int.TryParse(args["NoteTypeId"]?.ToString(), out var type)) return result;

            // Query Data.
            var key = args["NoteKey"].ToString();
            query = query.Where(u => u.NoteTypeId == type && u.NoteKey == key).OrderByDescending(x => x.NoteId);

            // Load Uploads.
            var notes = await query.ToListAsync();

            // Convert to Dto.
            var mapped = notes.Select(u => Mapper.Map<NoteEntityModel, NoteDto>(u, opts => opts.AfterMap((src, dest) =>
            {
                dest.UpdatedDateTime = src.UpdatedDt == null ? src.CreatedDt.ConvertToUserTime(User.TimeZoneInfoId) : src.UpdatedDt.Value.ConvertToUserTime(User.TimeZoneInfoId);
                dest.UpdatedByUser = src.UpdatedByUserGuid == null ? src.CreatedBy.GetDisplayName : src.UpdatedBy.GetDisplayName;
            }))).ToList();

            // Load Result.
            result.AddRange(mapped);

            // Return.
            return result;
        }

        public IGridPageDto<INoteDto> GetAllByGridPage(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public Task<IGridPageDto<INoteDto>> GetAllByGridPageAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IGridPageDto<INoteDto> GetAllByGridPage(int pageNumber, int pageSize, string sort, string searchPhrase)
        {
            throw new NotImplementedException();
        }

        public INoteDto Save(INoteDto update)
        {
            throw new NotImplementedException();
        }

        public async Task<INoteDto> SaveAsync(INoteDto update)
        {
            NoteEntityModel entity = null;
            if (update.NoteId.HasValue)
            {
                entity = await Db.Notes.FirstOrDefaultAsync(x => x.NoteId == update.NoteId.Value && x.NoteKey == update.NoteKey && x.NoteTypeId == (int)update.NoteTypeId);
                entity.UpdatedByUserGuid = User.UserGuid;
                entity.UpdatedDt = DateTimeOffset.UtcNow;
            }
            else
            {

                // Create New Update.
                entity = Mapper.Map<NoteEntityModel>(update);
                entity.CreatedByUserGuid = User.UserGuid;
                entity.CreatedDt = DateTimeOffset.UtcNow;

                // Save Changes.
                Db.Notes.Add(entity);
            }

            entity.NoteDescription = update.NoteDescription;

            await Db.SaveChangesAsync();

            // Create Response.
            var result = await GetByIdAsync(entity.NoteId.ToString());
            result.UpdateResult = new UpdateResultDto(true, "File Saved!");

            return result;
        }

        public bool Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteAsync(string id)
        {
            // Check Id.
            if (!int.TryParse(id, out var noteId)) return false;

            // Load Upload.
            var note = Db.Notes.FirstOrDefault(u => u.NoteId == noteId && !u.NoteDeletedInd);
            if (note == null) return false;

            // Update Values.
            note.NoteDeletedInd = true;
            note.NoteDeletedByUserGuid = User.UserGuid;
            note.NoteDeletedDt = DateTimeOffset.UtcNow;

            // Update Entry.
            Db.Entry(note).State = EntityState.Modified;
            await Db.SaveChangesAsync();

            return true;
        }
    }
}
