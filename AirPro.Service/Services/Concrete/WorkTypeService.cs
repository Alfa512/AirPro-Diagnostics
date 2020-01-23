using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using AirPro.Service.DTOs.Concrete;
using AirPro.Service.DTOs.Interface;
using AirPro.Service.Services.Abstract;
using AirPro.Service.Services.Interface;
using AutoMapper;
using Dapper;
using Newtonsoft.Json;

namespace AirPro.Service.Services.Concrete
{
    internal class WorkTypeService : ServiceBase, IService<IWorkTypeDto>
    {

        public WorkTypeService(IServiceSettings settings) : base(settings)
        {
        }

        public IWorkTypeDto GetById(string id)
        {
            // Load Work Type.
            WorkTypeDto result;
            using (var query = Conn.QueryMultiple("Scan.usp_GetWorkType", new { WorkTypeId = id }, commandType: CommandType.StoredProcedure))
            {
                result = query.Read<WorkTypeDto>().FirstOrDefault() ?? new WorkTypeDto() { WorkTypeActiveInd = true };
                result.RequestTypeSelection = new List<IWorkTypeRequestTypeDto>(query.Read<WorkTypeRequestTypeDto>().ToList());
                result.VehicleMakeSelection = new List<IWorkTypeVehicleMakeDto>(query.Read<WorkTypeVehicleMakeDto>().ToList());
            }

            return result;
        }

        public Task<IWorkTypeDto> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public string GetDisplayName(string id)
        {
            return GetById(id)?.WorkTypeName;
        }

        public Task<string> GetDisplayNameAsync(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<KeyValuePair<string, string>> GetDisplayList(ServiceArgs args = null)
        {
            return Conn.Query("Scan.usp_GetWorkTypeDisplayList", commandType: CommandType.StoredProcedure).Select(t =>
                new KeyValuePair<string, string>(t.WorkTypeId.ToString(), t.WorkTypeName)).ToList();
        }

        public Task<IEnumerable<KeyValuePair<string, string>>> GetDisplayListAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IWorkTypeDto> GetAll(ServiceArgs args = null)
        {
            return Conn.Query<WorkTypeDto>("Scan.usp_GetWorkTypeSearch", commandType: CommandType.StoredProcedure).Distinct().ToList();
        }

        public Task<IEnumerable<IWorkTypeDto>> GetAllAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IGridPageDto<IWorkTypeDto> GetAllByGridPage(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public Task<IGridPageDto<IWorkTypeDto>> GetAllByGridPageAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IGridPageDto<IWorkTypeDto> GetAllByGridPage(int pageNumber, int pageSize, string sort, string searchPhrase)
        {
            // Search Database.
            var rows = Conn.Query<WorkTypeDto>("Scan.usp_GetWorkTypeSearch", new { Search = searchPhrase }, commandType: CommandType.StoredProcedure).Distinct().ToList();

            // Create Result.
            var result = new GridPageDto<IWorkTypeDto>
            {
                Current = pageNumber,
                Total = rows.Count
            };

            // Sort Dataset.
            var sorted = string.IsNullOrEmpty(sort) ? rows.OrderBy(s => s.WorkTypeGroupSortOrder).ThenBy(s => s.WorkTypeSortOrder) : rows.OrderBy(sort);

            // Get Page.
            var page = pageSize < 0 ? sorted : sorted.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            // Set Page.
            result.Rows = page;
            result.RowCount = result.Rows.Count();

            return result;
        }

        public IWorkTypeDto Save(IWorkTypeDto update)
        {
            // Load Type.
            var result = Mapper.Map<WorkTypeDto>(update);
            result.UpdateResult = new UpdateResultDto(false, "Unknown Error has Occurred, Please Refresh the Page and Try Again.");

            // Validate Update.
            if (string.IsNullOrWhiteSpace(update.WorkTypeName))
            {
                result.UpdateResult = new UpdateResultDto(false, "Type Name is Required and can NOT be Empty.");
                return result;
            }

            if (update.WorkTypeGroupId == 0)
            {
                result.UpdateResult = new UpdateResultDto(false, "Group Selection is Required.");
                return result;
            }

            if (!update.RequestTypeSelection?.Any(t => t.SelectedInd) ?? true)
            {
                result.UpdateResult = new UpdateResultDto(false, "Request Type Selection is Required.");
                return result;
            }

            if (!update.VehicleMakeSelection?.Any(t => t.SelectedInd) ?? true)
            {
                result.UpdateResult = new UpdateResultDto(false, "Vehicle Make Selection is Required.");
                return result;
            }

            try
            {
                // Create Params.
                var param = new
                {
                    WorkTypeId = update.WorkTypeId,
                    WorkTypeName = update.WorkTypeName,
                    WorkTypeSortOrder = update.WorkTypeSortOrder,
                    WorkTypeDescription = update.WorkTypeDescription,
                    WorkTypeActiveInd = update.WorkTypeActiveInd,
                    WorkTypeGroupId = update.WorkTypeGroupId,
                    WorkTypeRequestTypeIds = JsonConvert.SerializeObject(update.RequestTypeSelection.Where(t => t.SelectedInd).Select(t => t.RequestTypeId).ToArray()),
                    WorkTypeVehicleMakeIds = JsonConvert.SerializeObject(update.VehicleMakeSelection.Where(t => t.SelectedInd).Select(t => t.VehicleMakeId).ToArray()),
                    UserGuid = User.UserGuid
                };

                // Execute Update.
                var exec = Conn.ExecuteScalar("Scan.usp_SaveWorkType", param, commandType: CommandType.StoredProcedure);

                // Load Updated Model.
                result = Mapper.Map<WorkTypeDto>(GetById(exec.ToString()));

                // Set Update Result.
                var updateMessage = $"Type '{ result.WorkTypeName }' ({ result.WorkTypeId }) { (update.WorkTypeId == result.WorkTypeId ? "Updated" : "Created") } Successfully.";
                result.UpdateResult = new UpdateResultDto(true, updateMessage);
            }
            catch (Exception e)
            {
                result.UpdateResult = new UpdateResultDto(false, "Error: " + e.Message);
            }

            return result;
        }

        public Task<IWorkTypeDto> SaveAsync(IWorkTypeDto update)
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
