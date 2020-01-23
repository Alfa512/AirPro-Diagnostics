using System;
using System.Collections.Generic;
using System.Linq;
using AirPro.Service.DTOs.Concrete;
using AirPro.Service.DTOs.Interface;
using AutoMapper;

namespace AirPro.Site.Areas.Admin.Models.TechProfile
{
    public class TechnicianProfileViewModelProfile : Profile
    {
        public TechnicianProfileViewModelProfile()
        {
            CreateMap<ITechnicianProfileDto, TechnicianProfileViewModel>()
                .ForMember(d => d.VehicleMakeIds,
                    opt => opt.MapFrom(s => s.VehicleMakes.Select(m => m.Key).AsEnumerable()))
                .ForMember(d => d.Schedules, opt => opt.MapFrom(s => s.Schedules.Select(d =>
                    new ProfileScheduleViewModel
                    {
                        DayOfWeek = d.DayOfWeek,
                        Name = GetDay(d.DayOfWeek),
                        StartTime = d.StartTime,
                        EndTime = d.EndTime,
                        BreakStart = d.BreakStart,
                        BreakEnd = d.BreakEnd
                    })))
                .ForMember(d => d.TimeOffEntries, opt => opt.MapFrom(s => s.TimeOffEntries.Select(d =>
                    new ProfileTimeOffEntryViewModel
                    {
                        TimeOffEntryId = d.TimeOffEntryId,
                        StartDate = d.StartDate,
                        EndDate = d.EndDate,
                        Reason = d.Reason,
                        Deleted = d.DeleteInd
                    })));

            CreateMap<TechnicianProfileViewModel, ITechnicianProfileDto>()
                .ForMember(d => d.VehicleMakes,
                    opt => opt.MapFrom(
                        s => s.VehicleMakeIds.Select(m => new KeyValuePair<int, string>(m, string.Empty))))
                .ForMember(d => d.Schedules, opt => opt.MapFrom(s => s.Schedules.Select(d => 
                    new TechnicianScheduleDto
                    {
                        DayOfWeek = d.DayOfWeek,
                        StartTime = d.StartTime,
                        EndTime = d.EndTime,
                        BreakStart = d.BreakStart,
                        BreakEnd = d.BreakEnd
                    })))
                .ForMember(d => d.TimeOffEntries, opt => opt.MapFrom(s => s.TimeOffEntries.Select(d =>
                    new TechnicianTimeOffDto
                    {
                        TimeOffEntryId = d.TimeOffEntryId,
                        StartDate = d.StartDate,
                        EndDate = d.EndDate,
                        Reason = d.Reason,
                        DeleteInd = d.Deleted
                    })));
        }

        private string GetDay(int dayOfWeek)
        {
            var value = (DayOfWeek)dayOfWeek;
            return value.ToString();
        }
    }
}