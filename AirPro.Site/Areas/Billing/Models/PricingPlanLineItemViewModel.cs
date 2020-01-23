using System.ComponentModel.DataAnnotations;

namespace AirPro.Site.Areas.Billing.Models
{
    public class PricingPlanLineItemViewModel //: IPricingPlanLineItemDto
    {
        public string PlanGroup { get; set; }
        public int TypeId { get; set; }
        public string TypeGroup { get; set; }
        public string TypeName { get; set; }
        [DisplayFormat(DataFormatString = "{0:c}")]
        public decimal DomesticCost { get; set; }
        [DisplayFormat(DataFormatString = "{0:c}")]
        public decimal EuropeanCost { get; set; }
        [DisplayFormat(DataFormatString = "{0:c}")]
        public decimal AsianCost { get; set; }
    }
}