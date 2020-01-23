using System;

namespace AirPro.WebJob.Mitchell.Models.Interface
{
    public interface IMitchellReportQueueItem
    {
        int RequestId { get; set; }
        Guid UserGuid { get; set; }
        DateTimeOffset TimestampOffset { get; set; }
    }
}