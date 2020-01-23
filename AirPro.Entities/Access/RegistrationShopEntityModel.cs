using AirPro.Entities.Billing;
using AirPro.Entities.Common;
using AirPro.Entities.Repair;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirPro.Entities.Access
{
    [Table("RegistrationShops", Schema = "Access")]
    public class RegistrationShopEntityModel
    {
        [Key]
        public int RegistrationShopId { get; set; }
        [MaxLength(256)]
        public string Name { get; set; }
        [MaxLength(15)]
        public string Phone { get; set; }
        [MaxLength(15)]
        public string Fax { get; set; }
        [MaxLength(1024)]
        public string Address1 { get; set; }
        [MaxLength(1024)]
        public string Address2 { get; set; }
        [MaxLength(1024)]
        public string City { get; set; }
        public string StateId { get; set; }
        [MaxLength(25)]
        public string Zip { get; set; }
        public bool SendToMitchellInd { get; set; }
        public int? DiscountPercentage { get; set; }
        [Index, MaxLength(128)]
        public string CCCShopId { get; set; }
        public bool AllowAllRepairAutoClose { get; set; }
        public bool AllowAutoRepairClose { get; set; }
        public bool AllowScanAnalysisAutoClose { get; set; }
        public bool ShopFixedPriceInd { get; set; }
        public decimal? FirstScanCost { get; set; }
        public decimal? AdditionalScanCost { get; set; }
        public int? AutomaticRepairCloseDays { get; set; }

        public bool HideFromReports { get; set; }


        public bool DisableShopBillingNotification { get; set; }
        public bool DisableShopStatementNotification { get; set; }

        public int? DefaultInsuranceCompanyId { get; set; }
        [ForeignKey(nameof(DefaultInsuranceCompanyId))]
        public virtual InsuranceCompanyEntityModel DefaultInsuranceCompany { get; set; }

        public int? AverageVehiclesPerMonth { get; set; }

        public int? CurrencyId { get; set; }
        [ForeignKey(nameof(CurrencyId))]
        public virtual CurrencyEntityModel Currency { get; set; }

        public int? PricingPlanId { get; set; }
        [ForeignKey(nameof(PricingPlanId))]
        public virtual PricingPlanEntityModel PricingPlan { get; set; }

        public int? EstimatePlanId { get; set; }
        [ForeignKey(nameof(EstimatePlanId))]
        public virtual EstimatePlanEntityModel EstimatePlan { get; set; }

        public int? BillingCycleId { get; set; }
        [ForeignKey(nameof(BillingCycleId))]
        public virtual BillingCycleEntityModel BillingCycle { get; set; }

        public string AllowedRequestTypeIds { get; set; }

        public string InsuranceCompaniesIds { get; set; }

        public string InsuranceCompaniesPricingPlansJson { get; set; }

        public string InsuranceCompaniesEstimatePlansJson { get; set; }

        public string VehicleMakesIds { get; set; }

        public string VehicleMakesPricingPlansJson { get; set; }
        public bool AllowSelfScanAssessment { get; set; }
    }
}
