using System.Collections.Generic;

namespace AirPro.WebJob.Mitchell.Models.Interface
{
    public interface IMitchellReportSystemDto
    {
        string Name { get; set; }
        string Status { get; set; }
        IEnumerable<IMitchellReportDtcDto> DTC { get; set; }
    }
}