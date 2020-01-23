using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Service.DTOs.Concrete
{
    internal class EmployeeDto : IEmployeeDto
    {
        public Guid UserGuid { get; set; }
        public string DisplayName { get; set; }
    }
}
