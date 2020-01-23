using System;
using System.Collections.Generic;
using AirPro.Common.Enumerations;
using AirPro.Library.Models.Concrete;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Library.Models.Interface
{
    public interface IScanRequestLibraryModel
    {
        bool AirBagsDeployed { get; set; }
        string AirBagsVisualDeployments { get; set; }
        string Contact { get; set; }
        bool DrivableInd { get; set; }
        string InsuranceCompanyDisplay { get; set; }
        int InsuranceCompanyId { get; set; }
        string InsuranceCompanyOther { get; set; }
        string InsuranceReferenceNumber { get; set; }
        string Notes { get; set; }
        string Odometer { get; set; }
        string OtherWarningInfo { get; set; }
        IEnumerable<int> PointsOfImpact { get; set; }
        string ProblemDescription { get; set; }
        string WarningIndicators { get; set; }
        int RepairId { get; set; }
        RepairStatuses RepairStatusId { get; set; }
        string RepairStatusName { get; set; }
        int RequestCategoryId { get; set; }
        string RequestCategoryName { get; set; }
        int RequestId { get; set; }
        int RequestTypeId { get; set; }
        string RequestTypeName { get; set; }
        string RequestTypeTemplateHtml { get; set; }
        IScanReportLibraryModel ScanReport { get; set; }
        IEnumerable<ScanResultGridItemModel> ScanResults { get; set; }
        bool SeatRemovedInd { get; set; }
        Guid ShopGuid { get; set; }
        string ShopName { get; set; }
        string ShopNotes { get; set; }
        string ShopRONumber { get; set; }
        string VehicleLookupInfo { get; set; }
        string VehicleMakeName { get; set; }
        bool VehicleManualEntryInd { get; set; }
        string VehicleModelName { get; set; }
        string VehicleTransmission { get; set; }
        string VehicleVIN { get; set; }
        string VehicleYear { get; set; }
        long DataLoadMilliseconds { get; set; }
    }
}