using System;
using System.Collections.Generic;

namespace AirPro.Service.DTOs.Interface
{
    public interface ITechnicianProfileDto
    {
        Guid UserGuid { get; set; }
        string UserName { get; set; }
        bool UserLocked { get; set; }
        string DisplayName { get; set; }
        string EmployeeId { get; set; }
        string OtherNotes { get; set; }
        bool IsActive { get; set; }
        int? LocationId { get; set; }
        string Location { get; set; }
        IEnumerable<KeyValuePair<int, string>> VehicleMakes { get; set; }
        IEnumerable<ITechnicianScheduleDto> Schedules { get; set; }
        IEnumerable<ITechnicianTimeOffDto> TimeOffEntries { get; set; }
        IEnumerable<ITechReportDto> Reports { get; set; }
        IUpdateResultDto UpdateResult { get; set; }
    }
}