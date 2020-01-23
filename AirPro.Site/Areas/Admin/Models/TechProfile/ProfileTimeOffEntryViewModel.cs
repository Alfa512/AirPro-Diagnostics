using System;

namespace AirPro.Site.Areas.Admin.Models.TechProfile
{
    public class ProfileTimeOffEntryViewModel
    {
        public int TimeOffEntryId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Reason { get; set; }
        public bool Deleted { get; set; }
    }
}