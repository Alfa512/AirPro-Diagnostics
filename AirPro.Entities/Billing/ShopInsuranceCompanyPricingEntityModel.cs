using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AirPro.Entities.Access;
using AirPro.Entities.Repair;

namespace AirPro.Entities.Billing
{
    [Table("ShopInsuranceCompaniesPricing", Schema = "Billing")]
    public class ShopInsuranceCompanyPricingEntityModel
    {
        [Column(Order = 0), Key, Required, ForeignKey(nameof(Shop))]
        public Guid ShopId { get; set; }
        [Column(Order = 1), Key, Required, ForeignKey(nameof(InsuranceCompany))]
        public int InsuranceCompanyId { get; set; }
        [ForeignKey(nameof(PricingPlan))]
        public int PricingPlanId { get; set; }

        public virtual ShopEntityModel Shop { get; set; }
        public virtual InsuranceCompanyEntityModel InsuranceCompany { get; set; }
        public virtual PricingPlanEntityModel PricingPlan { get; set; }
    }
}
