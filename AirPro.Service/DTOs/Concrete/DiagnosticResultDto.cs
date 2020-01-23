using System;
using System.Collections.Generic;
using AirPro.Common.Enumerations;
using AirPro.Service.DTOs.Interface;
using Newtonsoft.Json;

namespace AirPro.Service.DTOs.Concrete
{
    internal class DiagnosticResultDto : IDiagnosticResultDto
    {
        public int? ResultId { get; set; }
        public int? RequestId { get; set; }
        public DiagnosticTool DiagnosticTool { get; set; }
        public DiagnosticFileType DiagnosticFileType { get; set; }
        public string DiagnosticFileText { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
        public string CustomerRo { get; set; }
        public DateTime? ScanDateTime { get; set; }
        public string ShopName { get; set; }
        public string ShopAddress { get; set; }
        public string ShopEmail { get; set; }
        public string ShopFax { get; set; }
        public string ShopPhone { get; set; }
        public string VehicleVin { get; set; }
        public string VehicleMake { get; set; }
        public string VehicleModel { get; set; }
        public string VehicleYear { get; set; }

        public ICollection<string> TestabilityIssuesList
        {
            get => JsonConvert.DeserializeObject<ICollection<string>>(TestabilityIssues);
            set => TestabilityIssues = JsonConvert.SerializeObject(value);
        }
        internal string TestabilityIssues { get; set; }

        public ICollection<IDiagnosticControllerDto> Controllers { get; set; }
        public IUpdateResultDto UpdateResult { get; set; }
    }
}