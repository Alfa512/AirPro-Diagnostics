using System;
using System.Collections.Generic;
using AirPro.Common.Enumerations;

namespace AirPro.Service.DTOs.Interface
{
    public interface IRepairDto
    {
        Guid ShopGuid { get; set; }
        string ShopName { get; set; }
        int RepairId { get; set; }
        RepairStatuses RepairStatusId { get; set; }
        string RepairStatusName { get; set; }
        int? ActiveRequestId { get; set; }
        string ActiveRequestType { get; set; }
        bool ActiveRequestInProgressInd { get; set; }
        string ActiveRequestTechnician { get; set; }
        bool EstimateInd { get; set; }
        string ShopRONumber { get; set; }
        int InsuranceCompanyId { get; set; }
        string InsuranceCompanyOther { get; set; }
        string InsuranceCompanyDisplay { get; set; }
        string InsuranceReferenceNumber { get; set; }
        string VehicleVIN { get; set; }
        string VehicleMake { get; set; }
        string VehicleModel { get; set; }
        string VehicleYear { get; set; }
        string VehicleTransmission { get; set; }
        bool VehicleManualEntryInd { get; set; }
        string Odometer { get; set; }
        bool AirBagsDeployed { get; set; }
        string AirBagsVisualDeployments { get; set; }
        bool DrivableInd { get; set; }
        string CreatedBy { get; set; }
        DateTime CreatedDt { get; set; }
        IEnumerable<IRepairDownloadDto> RepairDownloads { get; set; }
        IEnumerable<int> PointsOfImpact { get; set; }
        IFeedbackDto Feedback { get; set; }
        IUpdateResultDto UpdateResult { get; set; }
        int AllowedShopRequestTypesCount { get; set; }
    }
}
