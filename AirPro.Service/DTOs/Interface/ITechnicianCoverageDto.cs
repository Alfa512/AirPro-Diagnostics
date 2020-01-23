using AirPro.Service.DTOs.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirPro.Service.DTOs.Interface
{
    public interface ITechnicianCoverageDto
    {
        List<string> TechnicianCoverageHeader { get; set; }
        List<TechnicianCoverageRowItemDto> TechnicianCoverageCount { get; set; }
        int Min { get; set; }
        int Desired { get; set; }
    }
}
