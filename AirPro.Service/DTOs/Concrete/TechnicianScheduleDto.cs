using AirPro.Service.DTOs.Interface;

namespace AirPro.Service.DTOs.Concrete
{
    public class TechnicianScheduleDto : ITechnicianScheduleDto
    {
        public int DayOfWeek { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string BreakStart { get; set; }
        public string BreakEnd { get; set; }
    }
}