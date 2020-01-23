using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirPro.Service.DTOs.Concrete
{
    public class TechnicianCoverageUserPreferenceDto
    {
        public int Min { get; set; }
        public int Desired { get; set; }

        public Dictionary<string, TechnicianCoverageUserPreferenceDto> ReqByHour { get; set; }
    }
}
