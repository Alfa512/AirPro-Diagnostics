using System;
using AirPro.WebJob.Mitchell.Models.Interface;

namespace AirPro.WebJob.Mitchell.Models.Concrete
{
    [Serializable]
    internal class MitchellReportDtcDto : IMitchellReportDtcDto
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string CategoryType { get; set; }
    }
}