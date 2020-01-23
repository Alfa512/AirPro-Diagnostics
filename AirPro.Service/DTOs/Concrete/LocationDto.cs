using AirPro.Service.DTOs.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirPro.Service.DTOs.Concrete
{
    public class LocationDto : ILocationDto
    {
        public int LocationId { get; set; }

        public string Name { get; set; }

        public int SortOrder { get; set; }
    }
}
