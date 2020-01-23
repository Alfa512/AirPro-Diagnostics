using AirPro.Service.DTOs.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirPro.Service.DTOs.Concrete
{
    internal class CCCInsuranceCompanyDto : ICCCInsuranceCompanyDto
    {
        public string CCCInsuranceCompanyId { get; set; }
        public string CCCInsuranceCompanyName { get; set; }
        public int? RepairInsuranceCompanyId { get; set; }
    }
}
