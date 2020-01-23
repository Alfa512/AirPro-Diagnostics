using System;
using AirPro.Common.Enumerations;

namespace AirPro.Common.Interfaces
{
    public interface IQueueNotification
    {
        NotificationTemplates TemplateName { get; set; }
        int Identifier { get; set; }
        Guid UserGuid { get; set; }
        DateTimeOffset TimestampOffset { get; set; }
    }
}
