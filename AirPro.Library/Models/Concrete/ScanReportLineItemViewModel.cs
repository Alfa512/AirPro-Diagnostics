using System;

namespace AirPro.Library.Models.Concrete
{
    public class ScanReportLineItemViewModel
    {
        public int RequestId { get; set; }
        public string RequestTypeName { get; set; }
        public string TechnicianName { get; set; }
        public DateTime? CompletedDateTime { get; set; }
    }
}