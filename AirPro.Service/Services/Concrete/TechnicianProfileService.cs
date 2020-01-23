using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using AirPro.Common.Enumerations;
using AirPro.Entities.Technician;
using AirPro.Service.DTOs.Concrete;
using AirPro.Service.DTOs.Interface;
using AirPro.Service.Services.Abstract;
using AirPro.Service.Services.Interface;
using AutoMapper;
using Dapper;

namespace AirPro.Service.Services.Concrete
{
    internal class TechnicianProfileService : ServiceBase, IService<ITechnicianProfileDto>
    {
        private IQueryable<TechnicianProfileEntityModel> AvailableProfiles => Db.TechnicianProfiles.Where(x => x.ActiveInd);

        public TechnicianProfileService(IServiceSettings settings) : base(settings)
        {
        }

        public ITechnicianProfileDto GetById(string id)
        {
            if (!UserHasRoles(ApplicationRoles.TechProfileAdmin)) return null;

            var userGuid = Guid.Parse(id);
            var profile = AvailableProfiles
                .Include(d => d.User)
                .Include(d => d.VehicleMakes)
                .Include(d => d.VehicleMakes.Select(m => m.VehicleMake))
                .Include(d => d.Schedules)
                .Include(d => d.TimeOffEntries)
                .Include(d => d.Location)
                .FirstOrDefault(d => d.UserGuid == userGuid);

            var technicianProfileDto = profile != null ? Mapper.Map<TechnicianProfileEntityModel, TechnicianProfileDto>(profile, opts => opts.Items["TimeZoneInfoId"] = User.TimeZoneInfoId) : null;

            if (technicianProfileDto != null)
            {
                technicianProfileDto.Reports = GetTechnicianProfileReports(profile.UserGuid);
            }

            return technicianProfileDto;
        }

        private IEnumerable<ITechReportDto> GetTechnicianProfileReports(Guid techProfileGuid)
        {
            var result = new List<ITechReportDto>();

            var rows = from report in Db.ScanReports
                       join scan in Db.ScanRequests on report.ReportId equals scan.ReportId
                       join scanRequest in Db.ScanRequests on scan.RequestId equals scanRequest.RequestId
                       join repair in Db.RepairOrders on scanRequest.OrderId equals repair.OrderId
                       join vehicle in Db.RepairVehicles on repair.VehicleVIN equals vehicle.VehicleVIN
                       join shop in Db.Shops on repair.ShopGuid equals shop.ShopGuid
                       where report.ResponsibleTechnicianUserGuid == techProfileGuid
                       select new TechReportDto
                       {
                           VehicleMakeName = vehicle.Make,
                           VehicleVIN = vehicle.VehicleVIN,
                           VehicleModelName = vehicle.Model,
                           VehicleTransmission = vehicle.Transmission,
                           VehicleYear = vehicle.Year,
                           ReportId = report.ReportId,
                           RequestId = scan.RequestId,
                           ShopGuid = repair.ShopGuid,
                           ShopName = shop.Name,
                           RepairId = repair.OrderId,
                           ResponsibleTechnicianUserGuid = report.ResponsibleTechnicianUserGuid
                       };
            result = rows.ToList<ITechReportDto>();

            return result;
        }

        public Task<ITechnicianProfileDto> GetByIdAsync(string id)
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

        public IEnumerable<ITechnicianProfileDto> GetAll(ServiceArgs args = null)
        {
            if (args != null && args.ContainsKey("ProfileCoverage"))
            {
                return AvailableProfiles
                    .Include(d => d.Schedules)
                    .Include(d => d.TimeOffEntries)
                    .ToList().Select(p => Mapper.Map<TechnicianProfileEntityModel, TechnicianProfileDto>(p, opts => opts.Items["TimeZoneInfoId"] = User.TimeZoneInfoId));
            }

            return AvailableProfiles
                .Include(d => d.User)
                .Include(d => d.VehicleMakes)
                .Include(d => d.VehicleMakes.Select(m => m.VehicleMake))
                .Include(d => d.Schedules)
                .Include(d => d.TimeOffEntries)
                .ToList().Select(p => Mapper.Map<TechnicianProfileEntityModel, TechnicianProfileDto>(p, opts => opts.Items["TimeZoneInfoId"] = User.TimeZoneInfoId));
        }

        public Task<IEnumerable<ITechnicianProfileDto>> GetAllAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IGridPageDto<ITechnicianProfileDto> GetAllByGridPage(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public Task<IGridPageDto<ITechnicianProfileDto>> GetAllByGridPageAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IGridPageDto<ITechnicianProfileDto> GetAllByGridPage(int pageNumber, int pageSize, string sort, string searchPhrase)
        {
            var rows = AvailableProfiles
                .Include(d => d.User)
                .Include(d => d.VehicleMakes)
                .Include(d => d.VehicleMakes.Select(m => m.VehicleMake))
                .Include(d => d.Schedules)
                .Include(d => d.TimeOffEntries);

            rows = rows.Where(d => d.DisplayName.Contains(searchPhrase)
                                   || d.EmployeeId.Contains(searchPhrase)
                                   || d.OtherNotes.Contains(searchPhrase)
                                   || d.User.UserName.Contains(searchPhrase)
                                   || d.Location.Name.Contains(searchPhrase)
                                   || d.VehicleMakes.Select(m => m.VehicleMake).Any(m => m.VehicleMakeName.Contains(searchPhrase)));

            // Create Result.
            var result = new GridPageDto<ITechnicianProfileDto>
            {
                Current = pageNumber,
                Total = rows.Count()
            };

            var sorted = !string.IsNullOrEmpty(sort)
                ? rows.OrderBy(sort)
                : rows.OrderBy("DisplayName");

            // Get Page.
            var page = pageSize < 0 ? sorted : sorted.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            // Set Page.
            result.Rows = page.ToList().Select(p => Mapper.Map<TechnicianProfileEntityModel, TechnicianProfileDto>(p, opts => opts.Items["TimeZoneInfoId"] = User.TimeZoneInfoId));
            result.RowCount = result.Rows.Count();

            return result;
        }

        public ITechnicianProfileDto Save(ITechnicianProfileDto update)
        {
            // Load Type.
            var result = update;

            if (!UserHasRoles(ApplicationRoles.TechProfileAdmin))
            {
                // No Group Access.
                result.UpdateResult = new UpdateResultDto(false, "You do not have access to Technician Profiles.");
                return result;
            }

            try
            {
                // Create Params.
                var param = new
                {
                    UserGuid = update.UserGuid,
                    DisplayName = update.DisplayName,
                    EmployeeId = update.EmployeeId,
                    ActiveInd = update.IsActive,
                    OtherNotes = update.OtherNotes,
                    CurrentUser = User.UserGuid,
                    LocationId = update.LocationId,
                    VehicleMakeIds = string.Join(",", update.VehicleMakes.Select(m => m.Key).ToList())
                };

                // Execute Update.
                var exec = Conn.Query<string>(
                    "Technician.usp_SaveProfile @UserGUid, @DisplayName, @EmployeeID, @ActiveInd, @OtherNotes, @CurrentUser, @VehicleMakeIds, @LocationId",
                    param).ToList();

                SaveSchedules(update);
                SaveTimeOff(update);

                // Reload Profile.
                result = GetById(param.UserGuid.ToString()) ?? update;

                // Set Update Result.
                var updateMessage = "Technician Profile Updated Successfully.";
                result.UpdateResult = new UpdateResultDto(true, updateMessage);
            }
            catch (Exception e)
            {
                update.UpdateResult = new UpdateResultDto(false, "Error: " + e.Message);
            }

            return result;
        }

        public Task<ITechnicianProfileDto> SaveAsync(ITechnicianProfileDto update)
        {
            throw new NotImplementedException();
        }

        private void SaveSchedules(ITechnicianProfileDto update)
        {
            var dt = new DataTable();
            dt.Columns.Add("DayOfWeek", typeof(int));
            dt.Columns.Add("StartTime", typeof(string));
            dt.Columns.Add("EndTime", typeof(string));
            dt.Columns.Add("BreakStart", typeof(string));
            dt.Columns.Add("BreakEnd", typeof(string));
            foreach (var v in update.Schedules)
            {
                dt.Rows.Add(v.DayOfWeek, v.StartTime, v.EndTime, v.BreakStart, v.BreakEnd);
            }

            // Create Params.
            var param2 = new
            {
                UserGuid = update.UserGuid,
                UpdatedBy = User.UserGuid,
                Schedules = dt.AsTableValuedParameter("[Technician].[udt_Schedules]")
            };

            // Execute Update.
            var spResult = Conn
                .Query<string>("[Technician].[usp_SaveProfileSchedules] @UserGuid, @UpdatedBy, @Schedules", param2)
                .FirstOrDefault();
        }

        private void SaveTimeOff(ITechnicianProfileDto update)
        {
            var dt2 = new DataTable();
            dt2.Columns.Add("TimeOffEntryId", typeof(int));
            dt2.Columns.Add("StartDate", typeof(string));
            dt2.Columns.Add("EndDate", typeof(string));
            dt2.Columns.Add("Reason", typeof(string));
            dt2.Columns.Add("DeleteInd", typeof(bool));
            foreach (var v in update.TimeOffEntries)
            {
                dt2.Rows.Add(v.TimeOffEntryId, v.StartDate, v.EndDate, v.Reason, v.DeleteInd);
            }

            // Create Params.
            var param = new
            {
                UserGuid = update.UserGuid,
                UpdatedBy = User.UserGuid,
                TimeoffEntries = dt2.AsTableValuedParameter("[Technician].[udt_TimeOff]")
            };

            // Execute Update.
            var spResult2 = Conn
                .Query<string>("[Technician].[usp_SaveProfileTimeOff] @UserGuid, @UpdatedBy, @TimeoffEntries", param)
                .FirstOrDefault();
        }

        public bool Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var userGuid = Guid.Parse(id);
            var profile = await Db.TechnicianProfiles.FirstOrDefaultAsync(x => x.UserGuid == userGuid);
            if (profile != null)
            {
                profile.ActiveInd = false;
                profile.UpdatedByUserGuid = User.UserGuid;
                profile.UpdatedDt = DateTimeOffset.UtcNow;
            }
            await Db.SaveChangesAsync();
            return true;
        }
    }
}