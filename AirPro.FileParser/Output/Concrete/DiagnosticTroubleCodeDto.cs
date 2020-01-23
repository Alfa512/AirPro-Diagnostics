using System.Collections.Generic;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Parser.Output.Concrete
{
    internal class DiagnosticTroubleCodeDto : IDiagnosticTroubleCodeDto
    {
        public int? DiagnosticTroubleCodeId { get; set; }
        public string DiagnosticTroubleCode { get; set; }
        public string DiagnosticTroubleCodeDescription { get; set; }
        public ICollection<string> DiagnosticTroubleCodeInformationList { get; set; }
    }
}
