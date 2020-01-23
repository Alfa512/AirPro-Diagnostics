using System;
using AirPro.WebJob.Mitchell.Models.Interface;

namespace AirPro.WebJob.Mitchell.Models.Concrete
{
    [Serializable]
    internal class MitchellReportPdfDto : IMitchellReportPdfDto
    {
        public byte[] Data { get; set; }
    }
}