using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Service.DTOs.Concrete
{
    public class TechnicianCoverageDto : ITechnicianCoverageDto
    {
        public TechnicianCoverageDto()
        {
            TechnicianCoverageHeader = new List<string>();
            TechnicianCoverageCount = new List<TechnicianCoverageRowItemDto>();
        }
        public List<string> TechnicianCoverageHeader { get; set; }
        public List<TechnicianCoverageRowItemDto> TechnicianCoverageCount { get; set; }
        public int Min { get; set; } = 1;
        public int Desired { get; set; } = 5;
    }
}
