using AirPro.Service.DTOs.Interface;
using AirPro.Service.Services.Abstract;
using AirPro.Service.Services.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using AirPro.Entities.Repair;
using AirPro.Service.DTOs.Concrete;
using AutoMapper;
using Dapper;

namespace AirPro.Service.Services.Concrete
{
    internal class VehicleMakeService : ServiceBase, IService<IVehicleMakeDto>
    {
        public VehicleMakeService(IServiceSettings settings) : base(settings)
        {
        }

        public IVehicleMakeDto GetById(string id)
        {
            var vmId = Convert.ToInt32(id);
            return Mapper.Map<VehicleMakeDto>(Db.RepairVehicleMakes.Include(x => x.ProgrammTools).FirstOrDefault(d => d.VehicleMakeId == vmId));
        }

        public Task<IVehicleMakeDto> GetByIdAsync(string id)
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

        public Task<IEnumerable<KeyValuePair<string, string>>> GetDisplayListAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IVehicleMakeDto> GetAll(ServiceArgs args = null)
        {
            var makes = Conn.Query<VehicleMakeDto>("Repair.usp_GetVehicleMakes").ToList();
            return makes;
        }

        public Task<IEnumerable<IVehicleMakeDto>> GetAllAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IGridPageDto<IVehicleMakeDto> GetAllByGridPage(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public Task<IGridPageDto<IVehicleMakeDto>> GetAllByGridPageAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IGridPageDto<IVehicleMakeDto> GetAllByGridPage(int pageNumber, int pageSize, string sort, string searchPhrase)
        {
            var rows = Conn.Query<VehicleMakeDto>("Repair.usp_GetVehicleMakes @search", new { search = searchPhrase })
                .Distinct()
                .ToList();

            // Create Result.
            var result = new GridPageDto<IVehicleMakeDto>
            {
                Current = pageNumber,
                Total = rows.Count
            };

            var sorted = !string.IsNullOrEmpty(sort)
                ? rows.OrderBy(sort)
                : rows.OrderBy(d => d.VehicleMakeName);

            // Get Page.
            var page = pageSize < 0 ? sorted : sorted.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            // Set Page.
            result.Rows = page;
            result.RowCount = result.Rows.Count();

            return result;
        }

        public IVehicleMakeDto Save(IVehicleMakeDto update)
        {
            var vm = Db.RepairVehicleMakes.Include(x => x.ProgrammTools).FirstOrDefault(d => d.VehicleMakeId == update.VehicleMakeId);
            var msg = string.Empty;
            if (vm != null)
            {
                vm.VehicleMakeName = update.VehicleMakeName;
                vm.VehicleMakeTypeId = update.VehicleMakeTypeId;
                vm.ProgramName = update.ProgramName;
                vm.ProgramInstructions = update.ProgramInstructions;
                UpdateVehicleMakeTools(vm, update);
                msg = "Vehicle Make Updated Successfully.";
            }
            else
            {
                vm = Mapper.Map<VehicleMakeEntityModel>(update);
                Db.RepairVehicleMakes.Add(vm);
                msg = "Vehicle Make Ctreated Successfully.";
            }

            Db.SaveChanges();

            var vmDto = Mapper.Map<IVehicleMakeDto>(vm);
            vmDto.UpdateResult = new UpdateResultDto(true, msg);

            return vmDto;
        }

        private void UpdateVehicleMakeTools(VehicleMakeEntityModel model, IVehicleMakeDto update)
        {
            var tools = model.ProgrammTools.ToList();
            foreach (var item in tools)
            {
                var tool = update.ProgramTools?.FirstOrDefault(x => x.VehicleMakeToolId == item.VehicleMakeToolId);
                if (tool == null)
                {
                    Db.VehicleMakeTools.Remove(item);
                }
                else
                {
                    item.Name = tool.Name;
                }
            }

            foreach (var item in update.ProgramTools?.Where(x => x.VehicleMakeToolId == null) ?? new List<IVehicleMakeToolDto>())
            {
                Db.VehicleMakeTools.Add(
                    new VehicleMakeToolEntityModel() {Name = item.Name, VehicleMakeId = model.VehicleMakeId});
            }
        }

        public Task<IVehicleMakeDto> SaveAsync(IVehicleMakeDto update)
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
