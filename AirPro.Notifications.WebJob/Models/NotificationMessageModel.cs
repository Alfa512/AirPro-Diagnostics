using System.Collections.Generic;
using AirPro.Messaging.Interface;

namespace AirPro.Notifications.WebJob.Models
{
    internal class NotificationMessageModel : INotificationMessage
    {
        public IEnumerable<INotificationDestination> Destinations { get; set; }
        public string EmailSubject { get; set; }
        public string EmailBody { get; set; }
        public string TextMessage { get; set; }
        public IEnumerable<INotificationMessageAttachment> Attachments { get; set; }
    }
}