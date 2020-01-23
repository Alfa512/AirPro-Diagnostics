using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Site.Models.Request
{
    public class RequestViewModel : IRequestDto
    {
        [Display(Name = "Repair")]
        public int RepairId { get; set; }
        [Display(Name = "Repair Status")]
        public string RepairStatusName { get; set; }

        public int RequestId { get; set; }
        public int RequestTypeId { get; set; }
        [Display(Name = "Request Type")]
        public string RequestTypeName { get; set; }
        public int RequestCategoryId { get; set; }
        public string RequestCategoryName { get; set; }

        public Guid ShopGuid { get; set; }
        [Display(Name = "Shop Name")]
        public string ShopName { get; set; }
        [Display(Name = "Shop #")]
        public string ShopContact { get; set; }
        [Display(Name = "Shop RO Number")]
        public string ShopRONumber { get; set; }
        public string Contact { get; set; }

        [Display(Name = "Insurance Co")]
        public string InsuranceCompanyDisplay { get; set; }
        [Display(Name = "Claim Number")]
        public string InsuranceReferenceNumber { get; set; }

        public bool VehicleManualEntryInd { get; set; }
        public string VehicleLookupInfo { get; set; }
        [Display(Name = "VIN")]
        public string VehicleVIN { get; set; }
        [Display(Name = "Vehicle Make")]
        public string VehicleMakeName { get; set; }
        [Display(Name = "Vehicle Model")]
        public string VehicleModelName { get; set; }
        [Display(Name = "Vehicle Year")]
        public string VehicleYear { get; set; }
        [Display(Name = "Transmission")]
        public string VehicleTransmission { get; set; }
        public string Odometer { get; set; }
        public DateTime RequestCreateDtUtc { get; set; }
        public DateTime RequestCreateDt { get; set; }
        public DateTime? ScanUploadDt { get; set; }
        public string TechnicianName { get; set; }
        public string TechnicianContactNumber { get; set; }
        public string TechnicianMobileNumber { get; set; }
        public bool CompletedInd { get; set; }
        public bool CancelledInd { get; set; }
        public string InsuranceCompanyName { get; set; }
        [Display(Name = "Tools Used:")]
        public List<IVehicleMakeToolDto> VehicleMakeTools { get; set; }

        [Display(Name = "Air Bags Deployed")]
        public bool AirBagsDeployed { get; set; }
        [Display(Name = "Air Bags Visual Deployments")]
        public string AirBagsVisualDeployments { get; set; }
        [Display(Name = "Drivable")]
        public bool DrivableInd { get; set; }
        [Display(Name = "Seat Removed")]
        public bool SeatRemovedInd { get; set; }

        public IEnumerable<int> PointsOfImpact { get; set; }

        public IEnumerable<string> WarningIndicators { get; set; }
        [Display(Name = "Other Warning Info")]
        public string OtherWarningInfo { get; set; }

        public string Notes { get; set; }
        [Display(Name = "Damage Description")]
        public string ProblemDescription { get; set; }
        [Display(Name = "Warning Indicators")]

        public string ReturnUrl { get; set; }
    }
}