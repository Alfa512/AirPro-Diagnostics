using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using AirPro.Service.DTOs.Interface;
using AirPro.Site.Attributes;
using AirPro.Site.Models.Shared;

namespace AirPro.Site.Models.Request
{
    public class ScanAnalysisViewModel : HasInsuranceSelectViewModel, IQuickRequest
    {
        public bool VehicleFound { get; set; }

        [Display(Name = "VIN")]
        public string VehicleVIN { get; set; }

        [Display(Name = "Make")]
        public int VehicleMakeId { get; set; }
        public string VehicleMakeName { get; set; }

        [Display(Name = "Model")]
        public string VehicleModel { get; set; }

        [Display(Name = "Year")]
        public string VehicleYear { get; set; }

        [Display(Name = "Transmission")]
        public string VehicleTransmission { get; set; } = "Unknown";

        public bool RepairFound { get; set; }

        public int? RepairOrderId { get; set; }

        public Guid ShopGuid { get; set; }

        [Display(Name = "Shop Name")]
        public string ShopName { get; set; }

        [Display(Name = "Shop RO Number")]
        public string ShopReferenceNumber { get; set; }

        [Display(Name = "Odometer")]
        public int Odometer { get; set; }

        [Display(Name = "Air Bags Deployed")]
        public bool AirBagsDeployed { get; set; }

        [Display(Name = "Describe Visual Deployments")]
        public string AirBagsVisualDeployments { get; set; }

        [Display(Name = "Drivable")]
        public bool DrivableInd { get; set; }

        public IEnumerable<SelectListItem> VehicleSelectList { get; set; }

        [Display(Name = "Points of Impact")]
        public IEnumerable<int> ImpactPoints { get; set; }

        public bool CccSourceInd { get; set; }

        public int? DiagnosticResultId { get; set; }

        public IEnumerable<IDiagnosticQueueDto> AvailableScans { get; set; }
        [Required, Display(Name = "Contact")]
        public string ContactUserGuid { get; set; }

        public string ContactOther { get; set; }
        [RequiredIf(nameof(ContactUserGuid), "other", ErrorMessage = "The Contact First Name field is required"), DataType(DataType.MultilineText), Display(Name = "Contact First Name")]
        public string ContactOtherFirstName { get; set; }

        [RequiredIf(nameof(ContactUserGuid), "other", ErrorMessage = "The Contact Last Name field is required"), DataType(DataType.MultilineText), Display(Name = "Contact Last Name")]
        public string ContactOtherLastName { get; set; }

        [RequiredIf(nameof(ContactUserGuid), "other", ErrorMessage = "The Contact Phone field is required"), DataType(DataType.MultilineText), Display(Name = "Contact Phone")]
        public string ContactOtherPhone { get; set; }
        public bool ShowAssistedScanRecommended { get; internal set; }
    }
}