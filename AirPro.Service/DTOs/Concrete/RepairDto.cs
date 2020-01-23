using System;
using System.Collections.Generic;
using System.Linq;
using AirPro.Common.Enumerations;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Service.DTOs.Concrete
{
    [Serializable]
    internal class RepairDto : IRepairDto
    {
        public Guid ShopGuid { get; set; }
        public string ShopName { get; set; }
        public int RepairId { get; set; }
        public RepairStatuses RepairStatusId { get; set; }
        public string RepairStatusName { get; set; }
        public int? ActiveRequestId { get; set; }
        public string ActiveRequestType { get; set; }
        public bool ActiveRequestInProgressInd { get; set; }
        public string ActiveRequestTechnician { get; set; }
        public bool EstimateInd { get; set; }
        public string ShopRONumber { get; set; }
        public int InsuranceCompanyId { get; set; }
        public string InsuranceCompanyOther { get; set; }
        public string InsuranceCompanyDisplay { get; set; }
        public string InsuranceReferenceNumber { get; set; }
        public string VehicleVIN { get; set; }
        public string VehicleMake { get; set; }
        public string VehicleModel { get; set; }
        public string VehicleYear { get; set; }
        public string VehicleTransmission { get; set; }
        public bool VehicleManualEntryInd { get; set; }
        public string Odometer { get; set; }
        public bool AirBagsDeployed { get; set; }
        public string AirBagsVisualDeployments { get; set; }
        public bool DrivableInd { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDt { get; set; }
        public IEnumerable<IRepairDownloadDto> RepairDownloads { get; set; }
        public IEnumerable<int> PointsOfImpact { get; set; }
        public string PointOfImpactIdList
        {
            set => PointsOfImpact = value.Split(',').Select(int.Parse).ToList();
            get => string.Join(",", PointsOfImpact ?? new List<int>());
        }
        public IFeedbackDto Feedback { get; set; }
        public IUpdateResultDto UpdateResult { get; set; }
        public int AllowedShopRequestTypesCount { get; set; }
    }
}
