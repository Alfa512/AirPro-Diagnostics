using System.Collections.Generic;
using AirPro.Service.DTOs.Interface;
using Newtonsoft.Json;

namespace AirPro.Service.DTOs.Concrete
{
    internal class DiagnosticTroubleCodeDto : IDiagnosticTroubleCodeDto
    {
        public int? DiagnosticTroubleCodeId { get; set; }
        public string DiagnosticTroubleCode { get; set; }
        public string DiagnosticTroubleCodeDescription { get; set; }

        public ICollection<string> DiagnosticTroubleCodeInformationList
        {
            get => JsonConvert.DeserializeObject<ICollection<string>>(DiagnosticTroubleCodeInformation);
            set => DiagnosticTroubleCodeInformation = JsonConvert.SerializeObject(value);
        }
        internal string DiagnosticTroubleCodeInformation { get; set; }
    }
}