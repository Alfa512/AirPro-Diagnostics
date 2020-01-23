using System;
using System.Collections.Generic;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Service.DTOs.Concrete
{
    public class TechnicianProfileDto : ITechnicianProfileDto
    {
        public Guid UserGuid { get; set; }
        public string UserName { get; set; }
        public bool UserLocked { get; set; }
        public string DisplayName { get; set; }
        public string EmployeeId { get; set; }
        public string OtherNotes { get; set; }
        public bool IsActive { get; set; }
        public int? LocationId { get; set; }
        public string Location { get; set; }
        public IEnumerable<KeyValuePair<int,string>> VehicleMakes { get; set; }
        public IEnumerable<ITechnicianScheduleDto> Schedules { get; set; }
        public IEnumerable<ITechnicianTimeOffDto> TimeOffEntries { get; set; }
        public IEnumerable<ITechReportDto> Reports { get; set; }
        public IUpdateResultDto UpdateResult { get; set; }
    }
}
