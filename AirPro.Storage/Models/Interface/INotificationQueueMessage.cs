using System;
using AirPro.Common.Enumerations;

namespace AirPro.Storage.Models.Interface
{
    public interface INotificationQueueMessage
    {
        NotificationTemplate TemplateName { get; set; }
        int Identifier { get; set; }
        Guid UserGuid { get; set; }
        DateTimeOffset TimestampOffset { get; set; }
    }
}
