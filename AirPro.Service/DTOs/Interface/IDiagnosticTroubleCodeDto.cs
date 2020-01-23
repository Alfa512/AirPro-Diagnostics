using System.Collections.Generic;

namespace AirPro.Service.DTOs.Interface
{
    public interface IDiagnosticTroubleCodeDto
    {
        int? DiagnosticTroubleCodeId { get; set; }
        string DiagnosticTroubleCode { get; set; }
        string DiagnosticTroubleCodeDescription { get; set; }
        ICollection<string> DiagnosticTroubleCodeInformationList { get; set; }
    }
}