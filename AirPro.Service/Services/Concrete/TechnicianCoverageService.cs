using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AirPro.Service.DTOs.Concrete;
using AirPro.Service.DTOs.Interface;
using AirPro.Service.Services.Abstract;
using AirPro.Service.Services.Interface;

namespace AirPro.Service.Services.Concrete
{
    internal class TechnicianCoverageService : ServiceBase, IService<ITechnicianCoverageDto>
    {
        private readonly TechnicianProfileService _technicianProfileService;
        private readonly UserPreferenceService _userPreferenceService;

        public TechnicianCoverageService(IServiceSettings settings) : base(settings)
        {
            _technicianProfileService = new TechnicianProfileService(settings);
            _userPreferenceService = new UserPreferenceService(settings);
        }

        public ITechnicianCoverageDto GetById(string id)
        {
            var profiles = _technicianProfileService.GetAll(new ServiceArgs() { { "ProfileCoverage", true } }).ToList();
            var preference = _userPreferenceService.GetAll(new ServiceArgs
            {
                { "UserGuid", id },
                { "ControlId", nameof(TechnicianCoverageUserPreferenceDto) }
            }).FirstOrDefault();

            var model = new TechnicianCoverageDto();

            TechnicianCoverageUserPreferenceDto settings = new TechnicianCoverageUserPreferenceDto();
            if (preference != null)
            {
                settings = preference.GetSettings<TechnicianCoverageUserPreferenceDto>();
                model.Desired = settings.Desired;
                model.Min = settings.Min;
            }

            var time = DateTime.Today.AddHours(6); // Starting from 6 AM
            for (var j = 1; j <= 16; j++) // until 9 PM
            {
                var timeString = time.ToString("hh:mm tt");
                var counts = new TechnicianCoverageRowItemDto(timeString);
                model.TechnicianCoverageCount.Add(counts);

                var cache = settings.ReqByHour?.FirstOrDefault(x => x.Key == timeString);
                if(cache != null)
                {
                    counts.Min = cache.Value.Value.Min;
                    counts.Desired = cache.Value.Value.Desired;
                }

                var date = DateTime.Today;
                for (var i = 1; i <= 7; i++)
                {
                    var dayOfWeek = (int)date.DayOfWeek;
                    if (model.TechnicianCoverageHeader.Count < 7) model.TechnicianCoverageHeader.Add(date.ToString("dd-MMM"));
                    var techCountPerTime = 0;
                    var techs = new List<string>();
                    foreach (var profile in profiles)
                    {
                        var timeOff = profile.TimeOffEntries.FirstOrDefault(x => x.StartDate.Date == date.Date);

                        var schedule = profile.Schedules.FirstOrDefault(x => x.DayOfWeek == dayOfWeek);
                        if (schedule == null || string.IsNullOrWhiteSpace(schedule.StartTime) ||
                            string.IsNullOrWhiteSpace(schedule.EndTime)) continue;

                        var startTime = DateTime.ParseExact(schedule.StartTime, "HH:mm:ss", CultureInfo.InvariantCulture);
                        var endTime = DateTime.ParseExact(schedule.EndTime, "HH:mm:ss", CultureInfo.InvariantCulture);
                        var isTechAvailable = time.TimeOfDay >= startTime.TimeOfDay && time.TimeOfDay < endTime.TimeOfDay;

                        if (!string.IsNullOrWhiteSpace(schedule.BreakStart) && !string.IsNullOrWhiteSpace(schedule.BreakEnd))
                        {
                            var breakStart = DateTime.ParseExact(schedule.BreakStart, "HH:mm:ss", CultureInfo.InvariantCulture);
                            var breakEnd = DateTime.ParseExact(schedule.BreakEnd, "HH:mm:ss", CultureInfo.InvariantCulture);

                            isTechAvailable = (time.TimeOfDay >= startTime.TimeOfDay && time.TimeOfDay < breakStart.TimeOfDay)
                                              || (time.TimeOfDay >= breakEnd.TimeOfDay && time.TimeOfDay < endTime.TimeOfDay);
                        }


                        if (timeOff != null && timeOff.StartDate <= date.Add(time.TimeOfDay) && date.Add(time.TimeOfDay) < timeOff.EndDate)
                        {
                            isTechAvailable = false;
                        }

                        if (!isTechAvailable) continue;

                        techCountPerTime++;
                        techs.Add(profile.DisplayName);
                    }
                    counts.Count.Add(new Tuple<int, string>(techCountPerTime, string.Join("<br />", techs)));
                    date = date.AddDays(1);
                }
                time = time.AddHours(1);
            }

            return model;
        }

        public Task<ITechnicianCoverageDto> GetByIdAsync(string id)
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

        public IEnumerable<ITechnicianCoverageDto> GetAll(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ITechnicianCoverageDto>> GetAllAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IGridPageDto<ITechnicianCoverageDto> GetAllByGridPage(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public Task<IGridPageDto<ITechnicianCoverageDto>> GetAllByGridPageAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IGridPageDto<ITechnicianCoverageDto> GetAllByGridPage(int pageNumber, int pageSize, string sort, string searchPhrase)
        {
            throw new NotImplementedException();
        }

        public ITechnicianCoverageDto Save(ITechnicianCoverageDto update)
        {
            throw new NotImplementedException();
        }

        public Task<ITechnicianCoverageDto> SaveAsync(ITechnicianCoverageDto update)
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
