using AirPro.Service.DTOs.Interface;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace AirPro.Site.Models.Client
{
    public class ShopInformationViewModel : IRegistrationShopDto
    {
        public int RegistrationShopId { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        [Display(Name = "State")]
        public string StateId { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }

        [Display(Name = "Additional Cost")]
        public decimal? AdditionalScanCost { get; set; }
        [Display(Name = "Close on Completion")]
        public bool AllowAutoRepairClose { get; set; }
        [Display(Name = "Close on Scan Analisys")]
        public bool AllowScanAnalysisAutoClose { get; set; }
        [Display(Name = "Close on All Scans")]
        public bool AllowAllRepairAutoClose { get; set; }
        [Display(Name = "Repair Aging Days")]
        public int? AutomaticRepairCloseDays { get; set; }
        [Display(Name = "Average Mo Volume")]
        public int? AverageVehiclesPerMonth { get; set; }
        [Display(Name = "Billing Cycle")]
        public int? BillingCycleId { get; set; }
        [Display(Name = "CCC Shop ID")]
        public string CCCShopId { get; set; }
        [Display(Name = "Default Currency")]
        public int? CurrencyId { get; set; }
        [Display(Name = "Scan Analysis Ins Co.")]
        public int? DefaultInsuranceCompanyId { get; set; }
        [Display(Name = "Invoice Notifications")]
        public bool AllowShopBillingNotification { get; set; }
        [Display(Name = "Statement Notifications")]
        public bool AllowShopStatementNotification { get; set; }
        [Display(Name = "Discount Percentage")]
        public int? DiscountPercentage { get; set; }
        [Display(Name = "First Scan Cost")]
        public decimal? FirstScanCost { get; set; }
        public int? EstimatePlanId { get; set; }
        [Display(Name = "Hide From Reports")]
        public bool HideFromReports { get; set; }
        [Display(Name = "Pricing Plan")]
        public int? PricingPlanId { get; set; }
        [Display(Name = "Send to Mitchell")]
        public bool SendToMitchellInd { get; set; }
        [Display(Name = "Fixed Pricing")]
        public bool ShopFixedPriceInd { get; set; }

        [Display(Name = "OEM Certifications")]
        public List<int> VehicleMakes { get; set; }
        [Display(Name = "Direct Repair Partners")]
        public List<int> InsuranceCompanies { get; set; }
        [Display(Name = "Allowed Scan Types")]
        public List<int> AllowedRequestTypes { get; set; }
        public List<IShopInsuranceCompanyPlanDto> ShopInsuranceCompanyPlans
        {
            get { return InsuranceCompanyPricingPlans?.ToList<IShopInsuranceCompanyPlanDto>(); }
            set { InsuranceCompanyPricingPlans = value.Select(x => new ShopInsuranceCompanyPlanViewModel { InsuranceCompanyId = x.InsuranceCompanyId, PlanId = x.PlanId }).ToList(); }
        }
        public List<IShopInsuranceCompanyPlanDto> ShopInsuranceCompanyEstimatePlans
        {
            get { return InsuranceCompanyEstimatePlans?.ToList<IShopInsuranceCompanyPlanDto>(); }
            set { InsuranceCompanyEstimatePlans = value.Select(x => new ShopInsuranceCompanyPlanViewModel { InsuranceCompanyId = x.InsuranceCompanyId, PlanId = x.PlanId }).ToList(); }
        }
        public List<IShopVehicleMakesPricingDto> ShopVehicleMakesPricingPlans
        {
            get { return VehicleMakesPricingPlans?.ToList<IShopVehicleMakesPricingDto>(); }
            set { VehicleMakesPricingPlans = value.Select(x => new ShopVehicleMakesPricingViewModel { VehicleMakeId = x.VehicleMakeId, PricingPlanId = x.PricingPlanId }).ToList(); }
        }
        public List<ShopInsuranceCompanyPlanViewModel> InsuranceCompanyPricingPlans { get; set; }
        public List<ShopInsuranceCompanyPlanViewModel> InsuranceCompanyEstimatePlans { get; set; }
        public List<ShopVehicleMakesPricingViewModel> VehicleMakesPricingPlans { get; set; }
        public bool AllowSelfScanAssessment { get; set; }
    }
}