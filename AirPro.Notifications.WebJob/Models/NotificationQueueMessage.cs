using System;
using AirPro.Common.Enumerations;
using AirPro.Storage;
using AirPro.Storage.Models.Interface;

namespace AirPro.Notifications.WebJob.Models
{
    internal class NotificationQueueMessage : INotificationQueueMessage
    {
        public NotificationTemplate TemplateName { get; set; }
        public int Identifier { get; set; }
        public Guid UserGuid { get; set; }
        public DateTimeOffset TimestampOffset { get; set; }
    }
}
