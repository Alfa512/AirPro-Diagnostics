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
using UniMatrix.Common.Extensions;

namespace AirPro.Service.Services.Concrete
{
    internal class UploadService : ServiceBase, IService<IUploadDto>
    {
        public UploadService(IServiceSettings settings) : base(settings)
        {
        }

        public IUploadDto GetById(string id)
        {
            if (!int.TryParse(id, out var uploadId)) return null;

            var upload = Db.Uploads.FirstOrDefault(u => u.UploadId == uploadId);
            var result = Mapper.Map<UploadEntityModel, UploadDto>(upload, opts => opts.AfterMap((src, dest) => dest.UploadedDateTime = src.CreatedDt.ConvertToUserTime(User.TimeZoneInfoId)));

            return result;
        }

        public Task<IUploadDto> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public string GetDisplayName(string id)
        {
            if (!int.TryParse(id, out var uploadId)) return null;

            var result = Db.Uploads.Where(u => u.UploadId == uploadId)
                .Select(u => $"{u.UploadFileName}.{u.UploadFileExtension}").ToString();

            return result;
        }

        public Task<string> GetDisplayNameAsync(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<KeyValuePair<string, string>> GetDisplayList(ServiceArgs args = null)
        {
            // Return List.
            return GetAll(args)?.Where(u => u.UploadId.HasValue).Select(u =>
                new KeyValuePair<string, string>(u.UploadId.Value.ToString(),
                    $"{u.UploadFileName}.{u.UploadFileExtension}")).ToList();
        }

        public Task<IEnumerable<KeyValuePair<string, string>>> GetDisplayListAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IUploadDto> GetAll(ServiceArgs args = null)
        {
            // Create Result.
            var result = new List<UploadDto>();

            // Load Uploads.
            var query = Db.Uploads.Where(u => !u.UploadDeletedInd);

            // Check Upload Type & Key.
            if (!(args?.ContainsKey("UploadTypeId") ?? false) || !args.ContainsKey("UploadKey") ||
                !int.TryParse(args["UploadTypeId"]?.ToString(), out var type)) return result;

            // Query Data.
            var key = args["UploadKey"].ToString();
            query = query.Where(u => u.UploadTypeId == type && u.UploadKey == key);

            // Load Uploads.
            var uploads = query.ToList();

            // Convert to Dto.
            var mapped = uploads.Select(u => Mapper.Map<UploadEntityModel, UploadDto>(u, opts => opts.AfterMap((src, dest) => dest.UploadedDateTime = src.CreatedDt.ConvertToUserTime(User.TimeZoneInfoId)))).ToList();

            // Load Result.
            result.AddRange(mapped);

            // Return.
            return result;
        }

        public Task<IEnumerable<IUploadDto>> GetAllAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IGridPageDto<IUploadDto> GetAllByGridPage(ServiceArgs args = null)
        {
            // Load Uploads.
            var uploads = GetAll(args).ToList();

            // Set Default Sort.
            args?.SetDefaultSort("UploadId DESC");

            // Load Grid Page.
            var result = uploads.GetGridPageFromCollection(args);

            // Return.
            return result;
        }

        public Task<IGridPageDto<IUploadDto>> GetAllByGridPageAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IGridPageDto<IUploadDto> GetAllByGridPage(int pageNumber, int pageSize, string sort, string searchPhrase)
        {
            // Load Arguments.
            var args = new ServiceArgs();
            args.AddGridOptions(pageNumber, pageSize, sort, searchPhrase);

            // Return.
            return GetAllByGridPage(args);
        }

        public IUploadDto Save(IUploadDto update)
        {
            // Check Upload Id.
            if (update.UploadId.HasValue) throw new NotImplementedException("Updates Not Implemented.");

            // Create New Update.
            var entity = Mapper.Map<UploadEntityModel>(update);
            entity.CreatedByUserGuid = User.UserGuid;
            entity.CreatedDt = DateTimeOffset.UtcNow;

            // Save Changes.
            Db.Uploads.Add(entity);
            Db.SaveChanges();

            // Create Response.
            var result = GetById(entity.UploadId.ToString());
            result.UpdateResult = new UpdateResultDto(true, "File Saved!");

            return result;
        }

        public Task<IUploadDto> SaveAsync(IUploadDto update)
        {
            throw new NotImplementedException();
        }

        public bool Delete(string id)
        {
            // Check Id.
            if (!int.TryParse(id, out var uploadId)) return false;

            // Load Upload.
            var upload = Db.Uploads.FirstOrDefault(u => u.UploadId == uploadId && !u.UploadDeletedInd);
            if (upload == null) return false;

            // Update Values.
            upload.UploadDeletedInd = true;
            upload.UploadDeletedByUserGuid = User.UserGuid;
            upload.UploadDeletedDt = DateTimeOffset.UtcNow;

            // Update Entry.
            Db.Entry(upload).State = EntityState.Modified;
            Db.SaveChanges();

            return true;
        }

        public Task<bool> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}
