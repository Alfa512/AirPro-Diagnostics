using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Site.Areas.Billing.Models
{
    public class PricingPlanViewModel //: IPricingPlanDto
    {
        public int PricingPlanId { get; set; }
        [Required, Display(Name = "Name")]
        public string PricingPlanName { get; set; }
        [Display(Name = "Description"), DataType(DataType.MultilineText)]
        public string PricingPlanDescription { get; set; }
        [Display(Name = "Active")]
        public bool PricingPlanActiveInd { get; set; }
        public string CreatedByName { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string UpdatedByName { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        [Required, Display(Name = "Currency")]
        public int? CurrencyId { get; set; }
        public string CurrencyName { get; set; }
        public List<PricingPlanLineItemViewModel> LineItems { get; set; }
        public IUpdateResultDto UpdateResult { get; set; }
    }
}