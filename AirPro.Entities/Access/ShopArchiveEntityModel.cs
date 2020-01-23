using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AirPro.Entities.Interfaces;

namespace AirPro.Entities.Access
{
    [Table("ShopsArchive", Schema = "Access")]
    public class ShopArchiveEntityModel : IArchiveEntityModel, IShopEntityModel, IAuditBaseEntityModel
    {
        [Key]
        public int ArchiveId { get; set; }
        public DateTimeOffset ArchiveDt { get; set; }
        public Guid AccountGuid { get; set; }
        public bool ActiveInd { get; set; }
        public decimal AdditionalScanCost { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public bool AllowAllRepairAutoClose { get; set; }
        public bool AllowAutoRepairClose { get; set; }
        public bool AllowScanAnalysisAutoClose { get; set; }
        public bool AllowDemoScan { get; set; }
        public bool AllowScanAnalysis { get; set; }
        public bool AllowSelfScan { get; set; }
        public bool AllowSelfScanAssessment { get; set; }
        public int? AutomaticRepairCloseDays { get; set; }
        public int? AverageVehiclesPerMonth { get; set; }
        public string CCCShopId { get; set; }
        public string City { get; set; }
        public int CurrencyId { get; set; }
        public int? DefaultInsuranceCompanyId { get; set; }
        public int DiscountPercentage { get; set; }
        public string DisplayName { get; }
        public int? EstimatePlanId { get; set; }
        public int? BillingCycleId { get; set; }
        public string Fax { get; set; }
        public decimal FirstScanCost { get; set; }
        public bool HideFromReports { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public string Phone { get; set; }
        public int? PricingPlanId { get; set; }
        public bool ShopFixedPriceInd { get; set; }
        public Guid ShopGuid { get; set; }
        public int ShopNumber { get; set; }
        public int StateId { get; set; }
        public string Zip { get; set; }
        public bool SendToMitchellInd { get; set; }
        public Guid CreatedByUserGuid { get; set; }
        public DateTimeOffset CreatedDt { get; set; }
        public Guid? UpdatedByUserGuid { get; set; }
        public DateTimeOffset? UpdatedDt { get; set; }
        public bool DisableShopBillingNotification { get; set; }
        public bool DisableShopStatementNotification { get; set; }
        public Guid? EmployeeGuid { get; set; }
        public bool AutomaticInvoicesInd { get; set; }
    }
}