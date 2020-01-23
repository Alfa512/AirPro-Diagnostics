using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AirPro.Common.Enumerations;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Site.Models.Request
{
    public class DiagnosticResultViewModel : IDiagnosticResultDto
    {
        [Display(Name = "Diagnostic ID")]
        public int? ResultId { get; set; }
        public int? RequestId { get; set; }
        [Display(Name = "Diagnostic Tool")]
        public DiagnosticTool DiagnosticTool { get; set; }
        public DiagnosticFileType DiagnosticFileType { get; set; }
        public string DiagnosticFileText { get; set; }
        [Display(Name = "Customer Firstname")]
        public string CustomerFirstName { get; set; }
        [Display(Name = "Customer Lastname")]
        public string CustomerLastName { get; set; }
        [Display(Name = "Customer RO")]
        public string CustomerRo { get; set; }
        [Display(Name = "Scan Performed")]
        public DateTime? ScanDateTime { get; set; }
        [Display(Name = "Shop Name")]
        public string ShopName { get; set; }
        [Display(Name = "Shop Address")]
        public string ShopAddress { get; set; }
        [Display(Name = "Shop Email")]
        public string ShopEmail { get; set; }
        [Display(Name = "Shop Fax")]
        public string ShopFax { get; set; }
        [Display(Name = "Shop Phone")]
        public string ShopPhone { get; set; }
        [Display(Name = "Vehicle VIN")]
        public string VehicleVin { get; set; }
        [Display(Name = "Vehicle Make")]
        public string VehicleMake { get; set; }
        [Display(Name = "Vehicle Model")]
        public string VehicleModel { get; set; }
        [Display(Name = "Vehicle Year")]
        public string VehicleYear { get; set; }
        public ICollection<string> TestabilityIssuesList { get; set; }
        public ICollection<IDiagnosticControllerDto> Controllers { get; set; }
        public IUpdateResultDto UpdateResult { get; set; }
    }
}