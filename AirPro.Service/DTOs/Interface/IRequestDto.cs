using System;
using System.Collections.Generic;

namespace AirPro.Service.DTOs.Interface
{
    public interface IRequestDto
    {
        int RepairId { get; set; }
        string RepairStatusName { get; set; }

        int RequestId { get; set; }
        int RequestTypeId { get; set; }
        string RequestTypeName { get; set; }
        int RequestCategoryId { get; set; }
        string RequestCategoryName { get; set; }

        Guid ShopGuid { get; set; }
        string ShopName { get; set; }
        string ShopContact { get; set; }
        string Contact { get; set; }
        string ShopRONumber { get; set; }

        bool DrivableInd { get; set; }
        bool AirBagsDeployed { get; set; }
        bool SeatRemovedInd { get; set; }

        string InsuranceCompanyDisplay { get; set; }
        string InsuranceReferenceNumber { get; set; }

        IEnumerable<int> PointsOfImpact { get; set; }
        IEnumerable<string> WarningIndicators { get; set; }

        string OtherWarningInfo { get; set; }
        string ProblemDescription { get; set; }
        string Notes { get; set; }

        bool VehicleManualEntryInd { get; set; }
        string VehicleLookupInfo { get; set; }
        string VehicleVIN { get; set; }
        string VehicleYear { get; set; }
        string VehicleMakeName { get; set; }
        string VehicleModelName { get; set; }
        string VehicleTransmission { get; set; }
        string Odometer { get; set; }

        DateTime RequestCreateDtUtc { get; set; }
        DateTime RequestCreateDt { get; set; }
        DateTime? ScanUploadDt { get; set; }
        string TechnicianName { get; set; }
        string TechnicianContactNumber { get; set; }
        string TechnicianMobileNumber { get; set; }
        bool CompletedInd { get; set; }
        bool CancelledInd { get; set; }
        string InsuranceCompanyName { get; set; }
    }
}
