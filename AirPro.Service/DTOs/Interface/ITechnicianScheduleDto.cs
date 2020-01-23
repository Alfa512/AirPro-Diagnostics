namespace AirPro.Service.DTOs.Interface
{
    public interface ITechnicianScheduleDto
    {
        int DayOfWeek { get; set; }
        string StartTime { get; set; }
        string EndTime { get; set; }
        string BreakStart { get; set; }
        string BreakEnd { get; set; }
    }
}
