using AirPro.Service.DTOs.Interface;

namespace AirPro.Service.DTOs.Concrete
{
    internal class PricingPlanLineItemDto : IPricingPlanLineItemDto
    {
        public string PlanGroup { get; set; }
        public int TypeId { get; set; }
        public string TypeGroup { get; set; }
        public string TypeName { get; set; }
        public decimal DomesticCost { get; set; }
        public decimal EuropeanCost { get; set; }
        public decimal AsianCost { get; set; }
    }
}