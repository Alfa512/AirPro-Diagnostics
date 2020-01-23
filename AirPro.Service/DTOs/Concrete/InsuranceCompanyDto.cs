using AirPro.Service.DTOs.Interface;
using System.Collections.Generic;

namespace AirPro.Service.DTOs.Concrete
{
    public class InsuranceCompanyDto : IInsuranceCompanyDto
    {
        public int InsuranceCompanyId { get; set; }
        public string InsuranceCompanyName { get; set; }
        public List<string> CccInsuranceCompanyIds { get; set; }
        public string ProgramName { get; set; }
        public bool DisabledInd { get; set; }
        public IUpdateResultDto UpdateResult { get; set; }
    }
}
