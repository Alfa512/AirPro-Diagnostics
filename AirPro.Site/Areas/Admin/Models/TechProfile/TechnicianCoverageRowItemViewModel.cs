using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirPro.Site.Areas.Admin.Models.TechProfile
{
    public class TechnicianCoverageRowItemViewModel
    {
        public int Min { get; set; } = 1;
        public int Desired { get; set; } = 5;
        public string Date { get; set; }
        public List<Tuple<int, string>> Count { get; set; }
    }
}