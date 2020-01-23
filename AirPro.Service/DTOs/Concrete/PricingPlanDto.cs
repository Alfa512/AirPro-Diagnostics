using System;
using System.Collections.Generic;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Service.DTOs.Concrete
{
    internal class PricingPlanDto : IPricingPlanDto
    {
        public int PricingPlanId { get; set; }
        public string PricingPlanName { get; set; }
        public string PricingPlanDescription { get; set; }
        public bool PricingPlanActiveInd { get; set; }
        public string CreatedByName { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string UpdatedByName { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public int CurrencyId { get; set; }
        public string CurrencyName { get; set; }
        public IEnumerable<IPricingPlanLineItemDto> LineItems { get; set; }
        public IUpdateResultDto UpdateResult { get; set; }
    }
}