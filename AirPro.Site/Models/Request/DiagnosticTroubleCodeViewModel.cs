using System.Collections.Generic;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Site.Models.Request
{
    public class DiagnosticTroubleCodeViewModel : IDiagnosticTroubleCodeDto
    {
        public int? DiagnosticTroubleCodeId { get; set; }
        public string DiagnosticTroubleCode { get; set; }
        public string DiagnosticTroubleCodeDescription { get; set; }
        public ICollection<string> DiagnosticTroubleCodeInformationList { get; set; }
    }
}