using System;
using System.Collections.Generic;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Service.DTOs.Concrete
{
    internal class RequestDto : IRequestDto
    {
        public string RepairStatusName { get; set; }
        public int RequestId { get; set; }
        public int RequestTypeId { get; set; }
        public string RequestTypeName { get; set; }
        public int RepairId { get; set; }
        public Guid ShopGuid { get; set; }
        public string ShopName { get; set; }
        public string ShopContact { get; set; }
        public string Contact { get; set; }
        public string ShopRONumber { get; set; }
        public bool DrivableInd { get; set; }
        public bool AirBagsDeployed { get; set; }
        public bool SeatRemovedInd { get; set; }
        public string InsuranceCompanyDisplay { get; set; }
        public string InsuranceReferenceNumber { get; set; }
        public IEnumerable<int> PointsOfImpact { get; set; }
        public IEnumerable<string> WarningIndicators { get; set; }
        public string OtherWarningInfo { get; set; }
        public string VehicleLookupInfo { get; set; }
        public string VehicleVIN { get; set; }
        public string VehicleMakeName { get; set; }
        public string VehicleModelName { get; set; }
        public string VehicleTransmission { get; set; }
        public string Odometer { get; set; }
        public string VehicleYear { get; set; }
        public DateTime RequestCreateDtUtc { get; set; }
        public DateTime RequestCreateDt { get; set; }
        public DateTime? ScanUploadDt { get; set; }
        public string TechnicianName { get; set; }
        public string TechnicianContactNumber { get; set; }
        public string TechnicianMobileNumber { get; set; }
        public bool CompletedInd { get; set; }
        public bool CancelledInd { get; set; }
        public string InsuranceCompanyName { get; set; }
        public int RequestCategoryId { get; set; }
        public string Notes { get; set; }
        public bool VehicleManualEntryInd { get; set; }
        public string ProblemDescription { get; set; }
        public string RequestCategoryName { get; set; }
    }
}