using AirPro.Messaging.Interface;

namespace AirPro.Notifications.WebJob.Models
{
    internal class NotificationDestinationModel : INotificationDestination
    {
        public string Email { get; set; }
        public string Text { get; set; }
        public string TimeZoneInfoId { get; set; }
    }
}