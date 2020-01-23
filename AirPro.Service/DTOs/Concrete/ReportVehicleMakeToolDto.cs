using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Service.DTOs.Concrete
{
    public class ReportVehicleMakeToolDto : IReportVehicleMakeToolDto
    {
        public int VehicleMakeToolId { get; set; }
        public string ToolVersion { get; set; }
        public bool CheckedInd { get; set; }
        public string Name { get; set; }
    }
}
