using AirPro.Entities.Billing;
using AirPro.Entities.Common;
using AirPro.Entities.Repair;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AirPro.Entities.Interfaces;

namespace AirPro.Entities.Access
{
    [Table("Shops", Schema = "Access")]
    public class ShopEntityModel : AuditBaseEntityModel, IShopEntityModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ShopGuid { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int ShopNumber { get; private set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string DisplayName { get; private set;}

        [Required]
        public Guid AccountGuid { get; set; }
        [ForeignKey(nameof(AccountGuid))]
        public virtual AccountEntityModel Account { get; set; }

        [Required, DisplayName("Shop Name")]
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public int StateId { get; set; }
        [ForeignKey(nameof(StateId))]
        public virtual StateEntityModel State { get; set; }
        public string Zip { get; set; }
        public bool SendToMitchellInd { get; set; }
        public string Notes { get; set; }

        public int DiscountPercentage { get; set; }

        [Index, MaxLength(128)]
        public string CCCShopId { get; set; }

        public bool AllowSelfScan { get; set; }

        public bool AllowAllRepairAutoClose { get; set; }
        public bool AllowAutoRepairClose { get; set; }
        public bool AllowScanAnalysisAutoClose { get; set; }
        
        public bool AllowScanAnalysis { get; set; }

        public bool AllowSelfScanAssessment { get; set; }

        public bool AllowDemoScan { get; set; }

        public bool ShopFixedPriceInd { get; set; }

        public decimal FirstScanCost { get; set; }

        public decimal AdditionalScanCost { get; set; }

        public int? AutomaticRepairCloseDays { get; set; }

        public bool ActiveInd { get; set; } = true;
        public bool HideFromReports { get; set; }


        public bool DisableShopBillingNotification { get; set; }
        public bool DisableShopStatementNotification { get; set; }

        public int? DefaultInsuranceCompanyId { get; set; }
        [ForeignKey(nameof(DefaultInsuranceCompanyId))]
        public virtual InsuranceCompanyEntityModel DefaultInsuranceCompany { get; set; }

        public int? AverageVehiclesPerMonth { get; set; }
        public bool AutomaticInvoicesInd { get; set; }

        public int CurrencyId { get; set; }
        [ForeignKey(nameof(CurrencyId))]
        public virtual CurrencyEntityModel Currency { get; set; }

        [ForeignKey(nameof(ShopGuid))]
        public virtual ICollection<UserShopEntityModel> ShopUsers { get; set; }

        public int? PricingPlanId { get; set; }
        [ForeignKey(nameof(PricingPlanId))]
        public virtual PricingPlanEntityModel PricingPlan { get; set; }

        public int? EstimatePlanId { get; set; }
        [ForeignKey(nameof(EstimatePlanId))]
        public virtual EstimatePlanEntityModel EstimatePlan { get; set; }

        public int? BillingCycleId { get; set; }
        [ForeignKey(nameof(BillingCycleId))]
        public virtual BillingCycleEntityModel BillingCycle { get; set; }

        [ForeignKey(nameof(Access.ShopInsuranceCompanyEntityModel.ShopId))]
        public virtual ICollection<Access.ShopInsuranceCompanyEntityModel> InsuranceCompanies { get; set; }

        [ForeignKey(nameof(Billing.ShopInsuranceCompanyPricingEntityModel.ShopId))]
        public virtual ICollection<Billing.ShopInsuranceCompanyPricingEntityModel> InsuranceCompaniesPricingPlans { get; set; }

        [ForeignKey(nameof(Billing.ShopInsuranceCompanyEstimateEntityModel.ShopId))]
        public virtual ICollection<Billing.ShopInsuranceCompanyEstimateEntityModel> InsuranceCompaniesEstimatePlans { get; set; }

        [ForeignKey(nameof(ShopVehicleMakeEntityModel.ShopId))]
        public virtual ICollection<ShopVehicleMakeEntityModel> VehicleMakes { get; set; }

        [ForeignKey(nameof(Billing.ShopVehicleMakesPricingEntityModel.ShopId))]
        public virtual ICollection<Billing.ShopVehicleMakesPricingEntityModel> VehicleMakesPricingPlans { get; set; }

        [ForeignKey(nameof(Inventory.AirProToolShopEntityModel.ShopGuid))]
        public virtual ICollection<Inventory.AirProToolShopEntityModel> AirProTools { get; set; }

        [ForeignKey(nameof(Access.ShopContactEntityModel.ShopContactGuid))]
        public virtual ICollection<Access.ShopContactEntityModel> ShopContacts { get; set; }
        public virtual ICollection<Access.ShopRequestTypeEntityModel> ShopRequestTypes { get; set; }
        [ForeignKey(nameof(Employee))]
        public Guid? EmployeeGuid { get; set; }
        public virtual UserEntityModel Employee { get; set; }
    }
}