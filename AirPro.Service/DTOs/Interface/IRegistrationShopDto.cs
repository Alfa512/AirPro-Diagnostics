using System.Collections.Generic;

namespace AirPro.Service.DTOs.Interface
{
    public interface IRegistrationShopDto
    {
        decimal? AdditionalScanCost { get; set; }
        string Address1 { get; set; }
        string Address2 { get; set; }
        bool AllowAllRepairAutoClose { get; set; }
        bool AllowAutoRepairClose { get; set; }
        bool AllowScanAnalysisAutoClose { get; set; }
        int? AutomaticRepairCloseDays { get; set; }
        int? AverageVehiclesPerMonth { get; set; }
        int? BillingCycleId { get; set; }
        string CCCShopId { get; set; }
        string City { get; set; }
        int? CurrencyId { get; set; }
        int? DefaultInsuranceCompanyId { get; set; }
        bool AllowShopBillingNotification { get; set; }
        bool AllowShopStatementNotification { get; set; }
        int? DiscountPercentage { get; set; }
        int? EstimatePlanId { get; set; }
        string Fax { get; set; }
        decimal? FirstScanCost { get; set; }
        bool HideFromReports { get; set; }
        string Name { get; set; }
        string Phone { get; set; }
        int? PricingPlanId { get; set; }
        int RegistrationShopId { get; set; }
        bool SendToMitchellInd { get; set; }
        bool ShopFixedPriceInd { get; set; }
        string StateId { get; set; }
        List<int> VehicleMakes { get; set; }
        List<int> InsuranceCompanies { get; set; }
        List<int> AllowedRequestTypes { get; set; }
        string Zip { get; set; }
        List<IShopInsuranceCompanyPlanDto> ShopInsuranceCompanyPlans { get; set; }
        List<IShopInsuranceCompanyPlanDto> ShopInsuranceCompanyEstimatePlans { get; set; }
        List<IShopVehicleMakesPricingDto> ShopVehicleMakesPricingPlans { get; set; }
        bool AllowSelfScanAssessment { get; set; }
    }
}
