using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Site.Areas.Admin.Models.VehicleMakes
{
    public class VehicleMakeToolViewModel : IVehicleMakeToolDto
    {
        public int? VehicleMakeToolId { get; set; }
        public string Name { get; set; }
    }
}