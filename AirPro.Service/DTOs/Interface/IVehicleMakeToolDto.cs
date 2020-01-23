using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirPro.Service.DTOs.Interface
{
    public interface IVehicleMakeToolDto
    {
        int? VehicleMakeToolId { get; set; }
        string Name { get; set; }
    }
}
