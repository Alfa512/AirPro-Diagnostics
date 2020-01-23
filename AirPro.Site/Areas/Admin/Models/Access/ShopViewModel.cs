using AirPro.Service.DTOs.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AirPro.Site.Areas.Admin.Models.Inventory;
using System.Web.Mvc;

namespace AirPro.Site.Areas.Admin.Models.Access
{
    public class ShopViewModel
    {
        [Display(Name = "Identifier")]
        public Guid ShopGuid { get; set; }
        [Display(Name = "Shop Number")]
        public int ShopNumber { get; set; }
        [Display(Name = "Name")]
        public string DisplayName { get; set; }
        [Required, Display(Name = "Account")]
        public Guid? AccountGuid { get; set; }
        [Display(Name = "Account Rep")]
        public Guid? AccountEmployeeGuid { get; set; }
        public string AccountRep { get; set; }
        [Display(Name = "Address 1")]
        public string Address1 { get; set; }
        [Display(Name = "Address 2")]
        public string Address2 { get; set; }
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        public string Zip { get; set; }
        [Display(Name = "Account")]
        public string AccountName { get; set; }
        public string Fax { get; set; }
        [Required, Display(Name = "Name")]
        [Remote("IsShopNameExist", "Access", "Admin", AdditionalFields = "ShopGuid", HttpMethod = "POST", ErrorMessage = "Shop name already exists in database.")]
        public string Name { get; set; }
        [Display(Name = "Notes"), DataType(DataType.MultilineText)]
        public string Notes { get; set; }

        [Display(Name = "Discount")]
        public int DiscountPercentage { get; set; }

        public bool AllowEntry { get; set; }

        public IUpdateResultDto UpdateResult { get; set; }

        [Display(Name = "CCC Shop ID")]
        public string CCCShopId { get; set; }

        [Display(Name = "Self Scan Shop")]
        public bool AllowSelfScan { get; set; }

        [Display(Name = "Close on Completion")]
        public bool AllowAutoRepairClose { get; set; }

        [Display(Name = "Close on All Scans")]
        public bool AllowAllRepairAutoClose { get; set; }

        [Display(Name = "Auto Repair Close Age")]
        public int? AutomaticRepairCloseDays { get; set; }

        [Display(Name = "Close on Scan Analysis")]
        public bool AllowScanAnalysisAutoClose { get; set; }

        [Display(Name = "Scan Analysis")]
        public bool AllowScanAnalysis { get; set; }

        [Display(Name = "Estimate Enabled")]
        public bool AllowSelfScanAssessment { get; set; }

        [Display(Name = "Demo Scan")]
        public bool AllowDemoScan { get; set; }

        [Display(Name = "Default Ins Co")]
        public int DefaultInsuranceCompanyId { get; set; }

        [Required, Display(Name = "Pricing Plan")]
        public int? PricingPlanId { get; set; }

        [Display(Name = "Estimate Plan")]
        public int? EstimatePlanId { get; set; }

        [Display(Name = "Avg Veh Per Mo")]
        public int? AverageVehiclesPerMonth { get; set; }

        public string Phone { get; set; }

        [Display(Name = "Shop Fixed Price Program")]
        public bool ShopFixedPriceInd { get; set; }
        [Display(Name = "Hide From Reports")]
        public bool HideFromReports { get; set; }
        [Display(Name = "First Scan Cost")]
        public decimal FirstScanCost { get; set; }
        [Display(Name = "Additional Scan Cost")]
        public decimal AdditionalScanCost { get; set; }

        [Display(Name = "Enabled")]
        public bool ActiveInd { get; set; } = true;

        [Display(Name = "Default Currency")]
        public int CurrencyId { get; set; } = 1;

        [Display(Name = "Billing Cycle")]
        public int? BillingCycleId { get; set;}

        [Display(Name = "Send To Mitchell")]
        public bool SendToMitchellInd { get; set; }
        [Display(Name = "Disable Invoice Notifications")]
        public bool DisableShopBillingNotification { get; set; }

        [Display(Name = "Disable Statement Notifications")]
        public bool DisableShopStatementNotification { get; set; }
        public ICollection<IUserDto> Users { get; set; }

        public ICollection<IUserDto> AccountUsers { get; set; }
        public List<ShopInsuranceCompanyPlanViewModel> ShopInsuranceCompanyPricingPlans { get; set; }
        public List<ShopInsuranceCompanyPlanViewModel> ShopInsuranceCompanyEstimatePlans { get; set; }
        public List<ShopVehicleMakesPricingViewModel> ShopVehicleMakesPricingPlans { get; set; }
        public IEnumerable<int> ShopVehicleMakes { get; set; }
        public IEnumerable<int> ShopInsuranceCompanies { get; set; }
        public List<AirProToolViewModel> AirProTools { get; set; }
        public List<AirProToolViewModel> AccountAirProTools { get; set; }
        public List<ShopContactViewModel> ShopContacts { get; set; }

        [Display(Name = "Request Types")]
        public List<int> ShopRequestTypes { get; set; }

        [Display(Name = "Shop Rep")]
        public Guid? EmployeeGuid { get; set; }
        [Display(Name = "Enable Automatic Invoices")]
        public bool AutomaticInvoicesInd { get; set; }
    }
}