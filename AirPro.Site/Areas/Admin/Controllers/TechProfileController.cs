using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AirPro.Common.Enumerations;
using AirPro.Service.DTOs.Concrete;
using AirPro.Service.DTOs.Interface;
using AirPro.Site.Areas.Admin.Models.TechProfile;
using AirPro.Site.Attributes;
using AirPro.Site.Models.Repairs;
using AirPro.Site.Results;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using UniMatrix.Common.Extensions;

namespace AirPro.Site.Areas.Admin.Controllers
{
    [AuthorizeRoles(ApplicationRoles.TechProfileAdmin)]
    public class TechProfileController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetTechnicianProfilesByPage(int current, int rowCount, string searchPhrase)
        {
            var result = Factory.GetAllByGridPage<ITechnicianProfileDto>(current, rowCount,
                Request.Form.GetDynamicSortString(), searchPhrase);

            return new JsonCamelCaseResult(result);
        }

        [HttpGet]
        public async Task<ActionResult> ManageProfile(string id)
        {
            var profile = !string.IsNullOrEmpty(id)
                ? Mapper.Map<TechnicianProfileViewModel>(Factory.GetById<ITechnicianProfileDto>(id))
                : new TechnicianProfileViewModel();

            if (string.IsNullOrEmpty(id))
            {
                BindPendingUsers();
                profile.ShouldCreate = true;
            }

            profile.Schedules = BindPendingSchedule(profile.Schedules);
            profile.TimeOffEntries = profile.TimeOffEntries?.OrderByDescending(d => d.StartDate);
            profile.Locations = await Factory.GetDisplayListAsync<ILocationDto>();

            BindVehicleMakes();

            return PartialView("_ProfilesManage", profile);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            return Json(await Factory.DeleteAsync<ITechnicianProfileDto>(id));
        }

        [HttpGet]
        public ActionResult TechnicianProfilesCoverage()
        {
            var model = Mapper.Map<TechnicianCoverageViewModel>(Factory.GetById<ITechnicianCoverageDto>(User.Identity.GetUserId()));

            return PartialView("_TechCoverage", model);
        }

        [HttpPost]
        public void UpdateCoveragePreferences(TechnicianCoverageUserPreferenceDto preference)
        {
            IUserPreferenceDto userPrefernce = new UserPreferenceDto
            {
                UserGuid = Guid.Parse(User.Identity.GetUserId()),
                ControlId = nameof(TechnicianCoverageUserPreferenceDto),
                SettingsJson = JsonConvert.SerializeObject(preference)
            };

            Factory.Save(userPrefernce);
        }

        private IEnumerable<ProfileScheduleViewModel> BindPendingSchedule(
            IEnumerable<ProfileScheduleViewModel> profileSchedules)
        {
            var daysOfWeek = Enum.GetValues(typeof(DayOfWeek))
                .Cast<DayOfWeek>()
                .Select(t => new
                {
                    DayOfWeek = (int)t,
                    Name = t.ToString()
                });

            var qry = daysOfWeek.GroupJoin(
                    profileSchedules ?? Enumerable.Empty<ProfileScheduleViewModel>(),
                    arr => arr.DayOfWeek,
                    db => db.DayOfWeek,
                    (x, y) => new { Local = x, Db = y })
                .SelectMany(
                    x => x.Db.DefaultIfEmpty(),
                    (x, y) => new ProfileScheduleViewModel
                    {
                        DayOfWeek = x.Local.DayOfWeek,
                        Name = x.Local.Name,
                        StartTime = y?.StartTime,
                        EndTime = y?.EndTime,
                        BreakStart = y?.BreakStart,
                        BreakEnd = y?.BreakEnd
                    }).ToList();

            return qry;
        }

        private void BindPendingUsers()
        {
            var profiles = Factory.GetAll<ITechnicianProfileDto>().Select(p => p.UserGuid).ToList();
            var users = Factory.GetDisplayList<IUserDto>().Where(u => !profiles.Contains(Guid.Parse(u.Key)));

            var selection = users.Select(u => new SelectListItem { Value = u.Key, Text = u.Value }).ToList();

            ViewBag.PendingUsers = selection;
        }

        [HttpPost]
        public async Task<ActionResult> ManageProfile(TechnicianProfileViewModel profile)
        {
            // Check Model.
            if (ModelState.IsValid)
            {
                var update = Mapper.Map<ITechnicianProfileDto>(profile);
                profile = Mapper.Map<TechnicianProfileViewModel>(Factory.Save(update));
            }
            else
            {
                if (profile.ShouldCreate)
                {
                    BindPendingUsers();
                }
            }
            profile.Locations = await Factory.GetDisplayListAsync<ILocationDto>();

            BindVehicleMakes();

            return PartialView("_ProfilesManage", profile);
        }

        public void BindVehicleMakes()
        {
            var entries = Mapper.Map<IEnumerable<VehicleViewModel>>(Factory.GetAll<IVehicleDto>());
            var vehicleMakes = entries.GroupBy(d => d.VehicleMakeId)
                .Select(d => d.First())
                .OrderBy(d => d.VehicleMakeName);

            ViewBag.VehicleMakes = vehicleMakes.Select(d => new SelectListItem
            {
                Value = d.VehicleMakeId.ToString(),
                Text = d.VehicleMakeName
            });
        }
    }
}