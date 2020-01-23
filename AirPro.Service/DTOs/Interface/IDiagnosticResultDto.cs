using System;
using System.Collections.Generic;
using AirPro.Common.Enumerations;

namespace AirPro.Service.DTOs.Interface
{
    public interface IDiagnosticResultDto
    {
        int? ResultId { get; set; }
        int? RequestId { get; set; }

        DiagnosticTool DiagnosticTool { get; set; }
        DiagnosticFileType DiagnosticFileType { get; set; }
        string DiagnosticFileText { get; set; }

        string CustomerFirstName { get; set; }
        string CustomerLastName { get; set; }
        string CustomerRo { get; set; }

        DateTime? ScanDateTime { get; set; }

        string ShopName { get; set; }
        string ShopAddress { get; set; }
        string ShopEmail { get; set; }
        string ShopFax { get; set; }
        string ShopPhone { get; set; }

        string VehicleVin { get; set; }
        string VehicleMake { get; set; }
        string VehicleModel { get; set; }
        string VehicleYear { get; set; }

        ICollection<string> TestabilityIssuesList { get; set; }

        ICollection<IDiagnosticControllerDto> Controllers { get; set; }

        IUpdateResultDto UpdateResult { get; set; }
    }
}