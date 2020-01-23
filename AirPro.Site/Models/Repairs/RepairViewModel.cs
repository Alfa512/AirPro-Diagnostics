using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AirPro.Common.Enumerations;
using AirPro.Service.DTOs.Interface;
using AirPro.Site.Models.Shared;

namespace AirPro.Site.Models.Repairs
{
    public class RepairViewModel : HasInsuranceSelectViewModel, IRepairDto
    {
        [Display(Name = "Shop")]
        public Guid ShopGuid { get; set; }

        public string ShopName { get; set; }

        public int RepairId { get; set; }

        public RepairStatuses RepairStatusId { get; set; }

        [Display(Name = "Status")]
        public string RepairStatusName { get; set; }

        public int? ActiveRequestId { get; set; }

        public string ActiveRequestType { get; set; }

        public bool ActiveRequestInProgressInd { get; set; }

        public string ActiveRequestTechnician { get; set; }

        public bool EstimateInd { get; set; }

        [Display(Name = "Shop RO Number")]
        public string ShopRONumber { get; set; }
        
        public string InsuranceCompanyDisplay { get; set; }

        [Display(Name = "Claim Number")]
        public string InsuranceReferenceNumber { get; set; }

        public string VehicleVIN { get; set; }

        public string VehicleMake { get; set; }

        public string VehicleModel { get; set; }

        public string VehicleYear { get; set; }

        public string VehicleTransmission { get; set; }

        public bool VehicleManualEntryInd { get; set; }

        [Display(Name = "Odometer")]
        public string Odometer { get; set; }

        [Display(Name = "Air Bags Deployed")]
        public bool AirBagsDeployed { get; set; }

        [Display(Name = "Vehicle Drivable")]
        public bool DrivableInd { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDt { get; set; }

        public IEnumerable<IRepairDownloadDto> RepairDownloads { get; set; }

        [Display(Name = "Describe Visual Deployments")]
        public string AirBagsVisualDeployments { get; set;} 

        [Display(Name = "Points of Impact")]
        public IEnumerable<int> PointsOfImpact { get; set; }

        public IFeedbackDto Feedback { get; set; }

        public IUpdateResultDto UpdateResult { get; set; }

        public int AllowedShopRequestTypesCount { get; set; }
    }
}