using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AirPro.Entities.Access;
using AirPro.Entities.Repair;

namespace AirPro.Entities.Billing
{
    [Table("ShopInsuranceCompaniesEstimate", Schema = "Billing")]
    public class ShopInsuranceCompanyEstimateEntityModel
    {
        [Column(Order = 0), Key, Required, ForeignKey(nameof(Shop))]
        public Guid ShopId { get; set; }
        [Column(Order = 1), Key, Required, ForeignKey(nameof(InsuranceCompany))]
        public int InsuranceCompanyId { get; set; }
        [ForeignKey(nameof(EstimatePlan))]
        public int EstimatePlanId { get; set; }

        public virtual ShopEntityModel Shop { get; set; }
        public virtual InsuranceCompanyEntityModel InsuranceCompany { get; set; }
        public virtual EstimatePlanEntityModel EstimatePlan { get; set; }
    }
}
