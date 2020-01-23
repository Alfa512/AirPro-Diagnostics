using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirPro.Service.DTOs.Interface
{
    public interface IRepairRequestScanDto
    {
        int OrderId { get; set; }
        int RequestTypeID { get; set; }
        ICollection<int> WarningIndicators { get; set; } //(CheckEngine, ABS, Airbag, TPMS, Stability, Security, Other)
        string OtherWarningInfo { get; set; }
        string ProblemDescription { get; set; } //Symptoms
        string Notes { get; set; }
        string ContactUserGuid { get; set; }

        string ContactOther { get; set; }
        string ContactOtherFirstName { get; set; }
        string ContactOtherLastName { get; set; }
        string ContactOtherPhone { get; set; }
        int? RequestTypeCategoryId { get; set; }
        bool SeatRemovedInd { get; set; }
        int ToolId { get; set; }
        List<KeyValuePair<string, string>> PositionStatementLinks { get; set; }
    }
}
