using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Site.Areas.Admin.Models.TechProfile
{
    public class TechnicianProfileViewModel
    {
        [Required, Display(Name = "User")] public Guid UserGuid { get; set; }

        [Display(Name = "User Name")] public string UserName { get; set; }

        public bool UserLocked { get; set; }

        [Required, Display(Name = "Display Name")]
        public string DisplayName { get; set; }

        [Required, Display(Name = "Employee ID")]
        public string EmployeeId { get; set; }

        [Display(Name = "Notes"), DataType(DataType.MultilineText)]
        public string OtherNotes { get; set; }

        [Display(Name = "Active Profile")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Vehicle Makes")]
        public IEnumerable<int> VehicleMakeIds { get; set; }

        [Display(Name = "Location")]
        public int? LocationId { get; set; }

        public IEnumerable<KeyValuePair<string, string>> Locations { get; set; }

        public IEnumerable<KeyValuePair<int, string>> VehicleMakes { get; set; }

        public IUpdateResultDto UpdateResult { get; set; }
        public bool ShouldCreate { get; set; }

        public IEnumerable<ProfileScheduleViewModel> Schedules { get; set; }

        public IEnumerable<ProfileTimeOffEntryViewModel> TimeOffEntries { get; set; }

        public IEnumerable<ITechReportDto> Reports { get; set; }
    }
}