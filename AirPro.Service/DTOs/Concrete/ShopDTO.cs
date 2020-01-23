using AirPro.Service.DTOs.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AirPro.Service.DTOs.Concrete
{
    [Serializable]
    internal class ShopDto : IShopDto
    {
        public ShopDto()
        {
            ShopContacts = new List<IShopContactDto>();
            ShopInsuranceCompanyPricingPlans = new List<IShopInsuranceCompanyPlanDto>();
            ShopInsuranceCompanyEstimatePlans = new List<IShopInsuranceCompanyPlanDto>();
            ShopVehicleMakesPricingPlans = new List<IShopVehicleMakesPricingDto>();
            ShopVehicleMakes = new List<int>();
            ShopInsuranceCompanies = new List<int>();
            AirProTools = new List<IAirProToolDto>();
            AccountAirProTools = new List<IAirProToolDto>();
            Users = new List<IUserDto>();
            AccountUsers = new List<IUserDto>();
        }

        public Guid ShopGuid { get; set; }
        public int ShopNumber { get; set; }
        public Guid? AccountGuid { get; set; }
        public Guid? AccountEmployeeGuid { get; set; }
        public string AccountName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Fax { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Notes { get; set; }
        public int DiscountPercentage { get; set; }
        public string Phone { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }

        public IUpdateResultDto UpdateResult { get; set; }
        public string CCCShopId { get; set; }
        public bool AllowAllRepairAutoClose { get; set; }
        public bool AllowSelfScan => ShopRequestTypes?.Any(x => x == 6) ?? false;
        public bool AllowAutoRepairClose { get; set; }
        public bool AllowScanAnalysisAutoClose { get; set; }
        public bool AllowScanAnalysis => ShopRequestTypes?.Any(x => x == 7) ?? false;
        public bool AllowSelfScanAssessment { get; set; }
        public bool AllowDemoScan => ShopRequestTypes?.Any(x => x == 8) ?? false;
        public int DefaultInsuranceCompanyId { get; set; }
        public int? PricingPlanId { get; set; }
        public int? EstimatePlanId { get; set; }
        public int? BillingCycleId { get; set; }
        public int? AverageVehiclesPerMonth { get; set; }
        public bool ShopFixedPriceInd { get; set; }
        public bool HideFromReports { get; set; }
        public decimal FirstScanCost { get; set; }
        public decimal AdditionalScanCost { get; set; }
        public int? AutomaticRepairCloseDays { get; set; }
        public bool ActiveInd { get; set; }
        public int CurrencyId { get; set; }
        public bool SendToMitchellInd { get; set; }
        public bool DisableShopBillingNotification { get; set; }
        public bool DisableShopStatementNotification { get; set; }
        public Guid? EmployeeGuid { get; set; }
        public string AccountRep { get; set; }
        public string ShopRep { get; set; }
        public bool AutomaticInvoicesInd { get; set; }
        public ICollection<IUserDto> Users { get; set; }
        public ICollection<IUserDto> AccountUsers { get; set; }
        public ICollection<IShopInsuranceCompanyPlanDto> ShopInsuranceCompanyPricingPlans { get; set; }
        public ICollection<IShopInsuranceCompanyPlanDto> ShopInsuranceCompanyEstimatePlans { get; set; }
        public ICollection<IShopVehicleMakesPricingDto> ShopVehicleMakesPricingPlans { get; set; }
        public IEnumerable<int> ShopVehicleMakes { get; set; }
        public IEnumerable<int> ShopInsuranceCompanies { get; set; }
        public IEnumerable<IAirProToolDto> AirProTools { get; set; }
        public IEnumerable<IAirProToolDto> AccountAirProTools { get; set; }
        public IEnumerable<IShopContactDto> ShopContacts { get; set; }
        public IEnumerable<int> ShopRequestTypes { get; set; }
    }
}