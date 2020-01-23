using System;
using System.Collections.Generic;
using System.Linq;
using AirPro.Entities.Technician;
using AirPro.Service.DTOs.Concrete;
using AirPro.Service.DTOs.Interface;
using AutoMapper;
using UniMatrix.Common.Extensions;

namespace AirPro.Service.DTOs.Profiles
{
    public class TechnicianProfileDtoProfile : Profile
    {
        public TechnicianProfileDtoProfile()
        {
            CreateMap<TechnicianProfileEntityModel, TechnicianProfileDto>()
                .ForMember(d => d.IsActive, opt => opt.MapFrom(i => i.ActiveInd))
                .ForMember(d => d.UserName, opt => opt.MapFrom(p => p.User.UserName))
                .ForMember(d => d.UserLocked, opt => opt.MapFrom(p => p.User.GetAccountLocked))
                .ForMember(d => d.Location, opt => opt.MapFrom(p => p.Location.Name))
                .ForMember(d => d.VehicleMakes,
                    opt => opt.MapFrom(i => i.VehicleMakes.Select(vm => vm.VehicleMake).OrderBy(m => m.VehicleMakeName)
                        .Select(m => new KeyValuePair<int, string>(m.VehicleMakeId, m.VehicleMakeName))))
                .ForMember(d => d.Schedules, opt =>
                    opt.ResolveUsing((s, d, m, c) => s.Schedules
                        .Select(q => new TechnicianScheduleDto
                        {
                            DayOfWeek = q.DayOfWeek,
                            StartTime = q.StartTime.HasValue ? q.StartTime.Value.LocalTimeZone(s.User.TimeZoneInfoId, c.Items["TimeZoneInfoId"].ToString()) : string.Empty,
                            EndTime = q.EndTime.HasValue ? q.EndTime.Value.LocalTimeZone(s.User.TimeZoneInfoId, c.Items["TimeZoneInfoId"].ToString()) : string.Empty,
                            BreakStart = q.BreakStart.HasValue ? q.BreakStart.Value.LocalTimeZone(s.User.TimeZoneInfoId, c.Items["TimeZoneInfoId"].ToString()) : string.Empty,
                            BreakEnd = q.BreakEnd.HasValue ? q.BreakEnd.Value.LocalTimeZone(s.User.TimeZoneInfoId, c.Items["TimeZoneInfoId"].ToString()) : string.Empty
                        })))
                .ForMember(d => d.TimeOffEntries, opt =>
                    opt.ResolveUsing((s, d, m, c) => s.TimeOffEntries
                        .Where(w => w.CreatedDt.DateTime > DateTime.UtcNow.AddDays(-30) && w.DeleteInd == false)
                        .Select(q => new TechnicianTimeOffDto
                            {
                                TimeOffEntryId = q.TimeOffEntryId,
                                Reason = q.Reason,
                                StartDate = q.StartDate.ConvertToUserTime(c.Items["TimeZoneInfoId"].ToString()),
                                EndDate = q.EndDate.ConvertToUserTime(c.Items["TimeZoneInfoId"].ToString()),
                                DeleteInd = q.DeleteInd
                            }
                        )));

            CreateMap<ITechnicianProfileDto, TechnicianProfileEntityModel>()
                .ForMember(d => d.ActiveInd, opt => opt.MapFrom(g => g.IsActive))
                .ForMember(d => d.VehicleMakes,
                    opt => opt.MapFrom(g => g.VehicleMakes.Select(m =>
                        new ProfileVehicleMakeEntityModel {VehicleMakeId = m.Key, UserGuid = g.UserGuid})));
        }
    }

    public static class Extension
    {
        public static string LocalTimeZone(this TimeSpan ts, string currentTimeZone, string targetTimeZone)
        {
            // Get Time Zones.
            var t = TimeZoneInfo.FindSystemTimeZoneById(targetTimeZone).GetUtcOffset(DateTime.UtcNow);
            var c = TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone).GetUtcOffset(DateTime.UtcNow);

            // Calculate Times.
            return DateTime.UtcNow.Date.Add(ts.Add(t.Subtract(c))).TimeOfDay.ToString();
        }
    }
}