using System;

namespace AirPro.Entities.Interfaces
{
    public interface IShopEntityModel
    {
        Guid AccountGuid { get; set; }
        bool ActiveInd { get; set; }
        decimal AdditionalScanCost { get; set; }
        string Address1 { get; set; }
        string Address2 { get; set; }
        bool AllowAllRepairAutoClose { get; set; }
        bool AllowAutoRepairClose { get; set; }
        bool AllowScanAnalysisAutoClose { get; set; }
        bool AllowDemoScan { get; set; }
        bool AllowScanAnalysis { get; set; }
        bool AllowSelfScan { get; set; }
        bool AllowSelfScanAssessment { get; set; }
        int? AutomaticRepairCloseDays { get; set; }
        int? AverageVehiclesPerMonth { get; set; }
        string CCCShopId { get; set; }
        string City { get; set; }
        int CurrencyId { get; set; }
        int? DefaultInsuranceCompanyId { get; set; }
        int DiscountPercentage { get; set; }
        string DisplayName { get; }
        int? EstimatePlanId { get; set; }
        int? BillingCycleId { get; set; }
        string Fax { get; set; }
        decimal FirstScanCost { get; set; }
        bool HideFromReports { get; set; }
        string Name { get; set; }
        string Notes { get; set; }
        string Phone { get; set; }
        int? PricingPlanId { get; set; }
        bool ShopFixedPriceInd { get; set; }
        Guid ShopGuid { get; set; }
        int ShopNumber { get; }
        int StateId { get; set; }
        string Zip { get; set; }
        bool SendToMitchellInd { get; set; }
        bool DisableShopBillingNotification { get; set; }
        bool DisableShopStatementNotification { get; set; }
        Guid? EmployeeGuid { get; set; }
        bool AutomaticInvoicesInd { get; set; }
    }
}