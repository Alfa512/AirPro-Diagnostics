using System;
using AirPro.WebJob.Mitchell.Models.Interface;

namespace AirPro.WebJob.Mitchell.Models.Concrete
{
    [Serializable]
    internal class MitchellReportQueueItem : IMitchellReportQueueItem
    {
        public int RequestId { get; set; }
        public Guid UserGuid { get; set; }
        public DateTimeOffset TimestampOffset { get; set; }
    }
}
