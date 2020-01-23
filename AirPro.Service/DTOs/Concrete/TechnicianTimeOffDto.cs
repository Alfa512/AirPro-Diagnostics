using System;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Service.DTOs.Concrete
{
    public class TechnicianTimeOffDto : ITechnicianTimeOffDto
    {
        public int TimeOffEntryId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Reason { get; set; }
        public bool DeleteInd { get; set; }
    }
}