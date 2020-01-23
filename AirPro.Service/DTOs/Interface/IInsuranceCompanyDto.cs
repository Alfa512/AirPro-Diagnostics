using System.Collections.Generic;

namespace AirPro.Service.DTOs.Interface
{
    public interface IInsuranceCompanyDto
    {
        int InsuranceCompanyId { get; set; }
        string InsuranceCompanyName { get; set; }
        List<string> CccInsuranceCompanyIds { get; set; }
        string ProgramName { get; set; }
        bool DisabledInd { get; set; }
        IUpdateResultDto UpdateResult { get; set; }
    }
}
