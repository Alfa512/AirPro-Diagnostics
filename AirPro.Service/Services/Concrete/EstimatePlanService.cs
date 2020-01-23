using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using AirPro.Common.Enumerations;
using AirPro.Entities.Billing;
using AirPro.Entities.Repair;
using AirPro.Service.DTOs.Concrete;
using AirPro.Service.DTOs.Interface;
using AirPro.Service.Services.Abstract;
using AirPro.Service.Services.Interface;
using AutoMapper;
using Dapper;

namespace AirPro.Service.Services.Concrete
{
    internal class EstimatePlanService : ServiceBase, IService<IEstimatePlanDto>
    {
        public EstimatePlanService(IServiceSettings settings) : base(settings)
        {
        }

        public IEstimatePlanDto GetById(string id)
        {
            var estimatePlanId = Convert.ToInt32(id);
            var entry = Db.EstimatePlans
                .Include(i => i.EstimatePlanVehicles)
                .Include(i => i.EstimatePlanVehicles.Select(d => d.VehicleMake))
                .FirstOrDefault(d => d.EstimatePlanId == estimatePlanId);

            if (entry == null)
            {
                entry = new EstimatePlanEntityModel
                {
                    EstimatePlanVehicles = new List<EstimatePlanVehicleEntityModel>()
                };
            };

            var allVehicles = Db.RepairVehicleMakes.ToList();

            var newToAdd = allVehicles
                .GroupJoin(entry.EstimatePlanVehicles, db => db.VehicleMakeId, arr => arr.VehicleMakeId,
                    (arr, db) => new { Local = arr, Db = db.SingleOrDefault() })
                .Where(d => d.Db == null)
                .Select(d => new EstimatePlanVehicleEntityModel
                {
                    VehicleMakeId = d.Local.VehicleMakeId,
                    VehicleMake = new VehicleMakeEntityModel
                    {
                        VehicleMakeId = d.Local.VehicleMakeId,
                        VehicleMakeName = d.Local.VehicleMakeName
                    }
                }).ToList();

            entry.EstimatePlanVehicles = entry.EstimatePlanVehicles.Concat(newToAdd).OrderBy(p => p.VehicleMake.VehicleMakeName).ToList();

            var result = Mapper.Map<IEstimatePlanDto>(entry);

            return result;
        }

        public Task<IEstimatePlanDto> GetByIdAsync(string id)
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
            return Db.EstimatePlans.OrderBy(e => e.Name).ToList()
                .Select(e => new KeyValuePair<string, string>(e.EstimatePlanId.ToString(), e.Name)).ToList();
        }

        public Task<IEnumerable<KeyValuePair<string, string>>> GetDisplayListAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IEstimatePlanDto> GetAll(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IEstimatePlanDto>> GetAllAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IGridPageDto<IEstimatePlanDto> GetAllByGridPage(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public Task<IGridPageDto<IEstimatePlanDto>> GetAllByGridPageAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IGridPageDto<IEstimatePlanDto> GetAllByGridPage(int pageNumber, int pageSize, string sort, string searchPhrase)
        {
            var rows = Db.EstimatePlans.AsQueryable()
                .Include(d => d.EstimatePlanVehicles)
                .Include(i => i.EstimatePlanVehicles.Select(d => d.VehicleMake));

            rows = rows.Where(d => d.Name.Contains(searchPhrase)
                                   || d.Description.Contains(searchPhrase));

            // Create Result.
            var result = new GridPageDto<IEstimatePlanDto>
            {
                Current = pageNumber,
                Total = rows.Count()
            };

            var sorted = !string.IsNullOrEmpty(sort)
                ? rows.OrderBy(sort)
                : rows.OrderBy(d => d.Name);

            // Get Page.
            var page = pageSize < 0 ? sorted : sorted.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            // Set Page.
            result.Rows = Mapper.Map<IEnumerable<IEstimatePlanDto>>(page.ToList());
            result.RowCount = result.Rows.Count();

            return result;
        }

        public IEstimatePlanDto Save(IEstimatePlanDto update)
        {
            // Load Type.
            var result = update;

            if (!UserHasRoles(ApplicationRoles.EstimatePlanCreate, ApplicationRoles.EstimatePlanEdit))
            {
                // No Group Access.
                result.UpdateResult = new UpdateResultDto(false, "You do not have access to Estimate Plans.");
                return result;
            }

            try
            {
                var dt = new DataTable();
                dt.Columns.Add("VehicleMakeId", typeof(int));
                dt.Columns.Add("CompletionCost", typeof(decimal));
                foreach (var v in update.VehiclePlans)
                {
                    dt.Rows.Add(v.VehicleMakeId, v.CompletionCost);
                }

                // Create Params.
                var param = new
                {
                    EstimatePlanId = update.EstimatePlanId,
                    Name = update.Name,
                    Description = update.Description,
                    ActiveInd = update.ActiveInd,
                    CurrentUser = User.UserGuid,
                    EstimateVehiclePlans = dt.AsTableValuedParameter("[Billing].[udt_EstimateVehiclePlans]")
                };

                // Execute Update.
                var spResult = Conn.Query<string>(
                    "[Billing].[usp_SaveEstimatePlan] @EstimatePlanId, @Name, @Description, @ActiveInd, @CurrentUser, @EstimateVehiclePlans",
                    param).FirstOrDefault();

                // Reload Vehicle Plan.
                result = GetById(spResult);

                // Set Update Result.
                var updateMessage = "Estimate Vehicle Plan Updated Successfully.";
                result.UpdateResult = new UpdateResultDto(true, updateMessage);
            }
            catch (Exception e)
            {
                update.UpdateResult = new UpdateResultDto(false, "Error: " + e.Message);
            }

            return result;
        }

        public Task<IEstimatePlanDto> SaveAsync(IEstimatePlanDto update)
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