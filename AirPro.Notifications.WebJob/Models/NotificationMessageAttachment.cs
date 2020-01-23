using System.IO;
using AirPro.Messaging.Interface;

namespace AirPro.Notifications.WebJob.Models
{
    public class NotificationMessageAttachment : INotificationMessageAttachment
    {
        public string ContentBase64 { get; set; }
        public string MimeType { get; set; }
        public string Filename { get; set; }
    }
}
