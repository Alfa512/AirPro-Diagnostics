using System;
using System.Collections.Generic;
using AirPro.WebJob.Mitchell.Models.Interface;

namespace AirPro.WebJob.Mitchell.Models.Concrete
{
    [Serializable]
    internal class MitchellReportSystemDto : IMitchellReportSystemDto
    {
        private MitchellReportSystemDto() { }

        public MitchellReportSystemDto(IEnumerable<MitchellReportDtcDto> dtc)
        {
            DTC = dtc;
        }

        public string Name { get; set; }
        public string Status { get; set; }
        public IEnumerable<IMitchellReportDtcDto> DTC { get; set; }
    }
}