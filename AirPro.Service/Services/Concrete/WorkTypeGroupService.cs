using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using AirPro.Service.DTOs.Concrete;
using AirPro.Service.DTOs.Interface;
using AirPro.Service.Services.Abstract;
using AirPro.Service.Services.Interface;
using AutoMapper;
using Dapper;

namespace AirPro.Service.Services.Concrete
{
    internal class WorkTypeGroupService : ServiceBase, IService<IWorkTypeGroupDto>
    {

        public WorkTypeGroupService(IServiceSettings settings) : base(settings)
        {
        }

        public IWorkTypeGroupDto GetById(string id)
        {
            var result = Conn.QueryFirstOrDefault<WorkTypeGroupDto>(
                @"Scan.usp_GetWorkTypeGroups @Search, @WorkTypeGroupId",
                new { Search = string.Empty, WorkTypeGroupId = id });

            return result;
        }

        public Task<IWorkTypeGroupDto> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public string GetDisplayName(string id)
        {
            return GetById(id)?.WorkTypeGroupName;
        }

        public Task<string> GetDisplayNameAsync(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<KeyValuePair<string, string>> GetDisplayList(ServiceArgs args = null)
        {
            return GetAll()?.Where(g => g.WorkTypeGroupActiveInd)
                .Select(g => new KeyValuePair<string, string>(g.WorkTypeGroupId.ToString(), g.WorkTypeGroupName)).ToList();
        }

        public Task<IEnumerable<KeyValuePair<string, string>>> GetDisplayListAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IWorkTypeGroupDto> GetAll(ServiceArgs args = null)
        {
            var result = Conn.Query<WorkTypeGroupDto>("Scan.usp_GetWorkTypeGroups").ToList();

            return result;
        }

        public Task<IEnumerable<IWorkTypeGroupDto>> GetAllAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IGridPageDto<IWorkTypeGroupDto> GetAllByGridPage(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public Task<IGridPageDto<IWorkTypeGroupDto>> GetAllByGridPageAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IGridPageDto<IWorkTypeGroupDto> GetAllByGridPage(int pageNumber, int pageSize, string sort, string searchPhrase)
        {
            // Load Records.
            var rows = Conn.Query<WorkTypeGroupDto>("Scan.usp_GetWorkTypeGroups @Search", new { Search = searchPhrase }).ToList();

            // Create Result.
            var result = new GridPageDto<IWorkTypeGroupDto>
            {
                Current = pageNumber,
                Total = rows.Count
            };

            // Sort Dataset.
            var sorted = string.IsNullOrEmpty(sort) ? rows.OrderBy(s => s.WorkTypeGroupSortOrder) : rows.OrderBy(sort);

            // Get Page.
            var page = pageSize < 0 ? sorted : sorted.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            // Set Page.
            result.Rows = page;
            result.RowCount = result.Rows.Count();

            return result;
        }

        public IWorkTypeGroupDto Save(IWorkTypeGroupDto update)
        {
            // Load Group.
            var result = Mapper.Map<WorkTypeGroupDto>(update);
            result.UpdateResult = new UpdateResultDto(false, "Unknown Error has Occurred, Please Refresh the Page and Try Again.");

            // Validate Update.
            if (string.IsNullOrWhiteSpace(update.WorkTypeGroupName))
            {
                result.UpdateResult = new UpdateResultDto(false, "Group Name is Required and can NOT be Empty.");
                return result;
            }

            try
            {
                // Create Params.
                var param = new
                {
                    WorkTypeGroupId = update.WorkTypeGroupId,
                    WorkTypeGroupName = update.WorkTypeGroupName,
                    WorkTypeGroupSortOrder = update.WorkTypeGroupSortOrder,
                    WorkTypeGroupActiveInd = update.WorkTypeGroupActiveInd,
                    UserGuid = User.UserGuid
                };

                // Execute Update.
                var exec = Conn.ExecuteScalar("Scan.usp_SaveWorkTypeGroup @WorkTypeGroupId, @WorkTypeGroupName, @WorkTypeGroupSortOrder, @WorkTypeGroupActiveInd, @UserGuid", param);

                // Load Updated Model.
                result = Mapper.Map<WorkTypeGroupDto>(GetById(exec.ToString()));

                // Set Update Result.
                var updateMessage = $"Group '{ result.WorkTypeGroupName }' ({ result.WorkTypeGroupId }) { (update.WorkTypeGroupId == result.WorkTypeGroupId ? "Updated" : "Created") } Successfully.";
                result.UpdateResult = new UpdateResultDto(true, updateMessage);
            }
            catch (Exception e)
            {
                result.UpdateResult = new UpdateResultDto(false, "Error: " + e.Message);
            }

            return result;
        }

        public Task<IWorkTypeGroupDto> SaveAsync(IWorkTypeGroupDto update)
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
