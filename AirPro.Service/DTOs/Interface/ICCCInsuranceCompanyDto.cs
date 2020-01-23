using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirPro.Service.DTOs.Interface
{
    public interface ICCCInsuranceCompanyDto
    {
        string CCCInsuranceCompanyId { get; set; }
        string CCCInsuranceCompanyName { get; set; }
        int? RepairInsuranceCompanyId { get; set; }
    }
}
