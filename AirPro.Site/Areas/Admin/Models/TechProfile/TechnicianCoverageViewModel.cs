using System.Collections.Generic;

namespace AirPro.Site.Areas.Admin.Models.TechProfile
{
    public class TechnicianCoverageViewModel
    {
        public TechnicianCoverageViewModel()
        {
            TechnicianCoverageHeader = new List<string>();
            TechnicianCoverageCount = new List<TechnicianCoverageRowItemViewModel>();
        }

        public List<string> TechnicianCoverageHeader { get; private set; }
        public List<TechnicianCoverageRowItemViewModel> TechnicianCoverageCount { get; private set; }
        public int Min { get; set; } = 1;
        public int Desired { get; set; } = 5;
    }
}