using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Site.Models.Request
{
    public interface IQuickRequest
    {
        bool AirBagsDeployed { get; set; }
        string AirBagsVisualDeployments { get; set; }
        IEnumerable<IDiagnosticQueueDto> AvailableScans { get; set; }
        bool CccSourceInd { get; set; }
        int? DiagnosticResultId { get; set; }
        bool DrivableInd { get; set; }
        IEnumerable<int> ImpactPoints { get; set; }
        int InsuranceCompanyId { get; set; }
        string InsuranceCompanyOther { get; set; }
        int Odometer { get; set; }
        bool RepairFound { get; set; }
        int? RepairOrderId { get; set; }
        Guid ShopGuid { get; set; }
        string ShopName { get; set; }
        string ShopReferenceNumber { get; set; }
        bool VehicleFound { get; set; }
        int VehicleMakeId { get; set; }
        string VehicleMakeName { get; set; }
        string VehicleModel { get; set; }
        IEnumerable<SelectListItem> VehicleSelectList { get; set; }
        string VehicleTransmission { get; set; }
        string VehicleVIN { get; set; }
        string VehicleYear { get; set; }
        string ContactOther { get; set; }
        string ContactOtherFirstName { get; set; }
        string ContactOtherLastName { get; set; }
        string ContactOtherPhone { get; set; }
        string ContactUserGuid { get; set; }
    }
}