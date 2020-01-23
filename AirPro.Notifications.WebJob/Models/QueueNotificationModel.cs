using System;
using AirPro.Common.Enumerations;
using AirPro.Common.Interfaces;

namespace AirPro.Notifications.WebJob.Models
{
    public class QueueNotificationModel : IQueueNotification
    {
        public NotificationTemplates TemplateName { get; set; }
        public int Identifier { get; set; }
        public Guid UserGuid { get; set; }
        public DateTimeOffset TimestampOffset { get; set; }
    }
}
