using System;
using System.Collections.Generic;

namespace AirPro.Service.DTOs.Interface
{
    public interface IPricingPlanDto
    {
        int PricingPlanId { get; set; }
        string PricingPlanName { get; set; }
        string PricingPlanDescription { get; set; }
        bool PricingPlanActiveInd { get; set; }
        string CreatedByName { get; set; }
        DateTime CreatedDateTime { get; set; }
        string UpdatedByName { get; set; }
        DateTime? UpdatedDateTime { get; set; }
        int CurrencyId { get; set; }
        string CurrencyName { get; set; }
        IEnumerable<IPricingPlanLineItemDto> LineItems { get; set; }
        IUpdateResultDto UpdateResult { get; set; }
    }
}
