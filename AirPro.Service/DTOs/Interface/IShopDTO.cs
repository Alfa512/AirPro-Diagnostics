using AirPro.Common.Interfaces;
using System;
using System.Collections.Generic;

namespace AirPro.Service.DTOs.Interface
{
    public interface IShopDto : IMailingAddress
    {
        Guid ShopGuid { get; set; }
        int ShopNumber { get; set; }
        Guid? AccountGuid { get; set; }
        Guid? AccountEmployeeGuid { get; set; }
        string AccountName { get; set; }
        string Phone { get; set; }
        string Fax { get; set; }
        string Name { get; set; }
        string DisplayName { get; set; }
        string Notes { get; set; }
        int DiscountPercentage { get; set; }
        IUpdateResultDto UpdateResult { get; set; }
        string CCCShopId { get; set; }
        bool AllowAllRepairAutoClose { get; set; }
        bool AllowSelfScan { get; }
        bool AllowAutoRepairClose { get; set; }
        bool AllowScanAnalysisAutoClose { get; set; }
        bool AllowScanAnalysis { get; }
        bool AllowSelfScanAssessment { get; set; }
        bool AllowDemoScan { get; }
        int DefaultInsuranceCompanyId { get; set; }
        int? PricingPlanId { get; set; }
        int? EstimatePlanId { get; set; }
        int? BillingCycleId { get; set; }
        int? AverageVehiclesPerMonth { get; set; }
        bool ShopFixedPriceInd { get; set; }
        bool HideFromReports { get; set; }
        decimal FirstScanCost { get; set; }
        decimal AdditionalScanCost { get; set; }
        int? AutomaticRepairCloseDays { get; set; }
        bool ActiveInd { get; set; }
        int CurrencyId { get; set; }
        bool SendToMitchellInd { get; set; }
        bool DisableShopBillingNotification { get; set; }
        bool DisableShopStatementNotification { get; set; }
        ICollection<IUserDto> Users { get; set; }
        ICollection<IUserDto> AccountUsers { get; set; }
        ICollection<IShopInsuranceCompanyPlanDto> ShopInsuranceCompanyPricingPlans { get; set; }
        ICollection<IShopInsuranceCompanyPlanDto> ShopInsuranceCompanyEstimatePlans { get; set; }
        ICollection<IShopVehicleMakesPricingDto> ShopVehicleMakesPricingPlans { get; set; }
        IEnumerable<int> ShopVehicleMakes { get; set; }
        IEnumerable<int> ShopInsuranceCompanies { get; set; }
        IEnumerable<IAirProToolDto> AirProTools { get; set; }
        IEnumerable<IAirProToolDto> AccountAirProTools { get; set; }
        IEnumerable<IShopContactDto> ShopContacts { get; set; }
        IEnumerable<int> ShopRequestTypes { get; set; }
        Guid? EmployeeGuid { get; set; }
        string AccountRep { get; set; }
        string ShopRep { get; set; }
        bool AutomaticInvoicesInd { get; set; }
    }
}