using System;

namespace AirPro.Storage.Models.Concrete
{
    internal class MitchellReportQueueItem
    {
        public int RequestId { get; set; }
        public Guid UserGuid { get; set; }
        public DateTimeOffset TimestampOffset { get; set; }
    }
}
