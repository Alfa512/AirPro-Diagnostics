namespace AirPro.Site.Areas.Admin.Models.TechProfile
{
    public class ProfileScheduleViewModel
    {
        public int DayOfWeek { get; set; }
            
        public string Name { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public string BreakStart { get; set; }

        public string BreakEnd { get; set; }
    }
}