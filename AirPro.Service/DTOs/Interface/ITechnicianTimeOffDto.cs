using System;

namespace AirPro.Service.DTOs.Interface
{
    public interface ITechnicianTimeOffDto
    {
        int TimeOffEntryId { get; set; }
        DateTime StartDate { get; set; }
        DateTime EndDate { get; set; }
        string Reason { get; set; }
        bool DeleteInd { get; set; }
    }
}