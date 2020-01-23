using System;

namespace AirPro.Entities.Notifications
{
    public class NotificationUserEntityModel
    {
        public Guid UserGuid { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public Guid? ConnectionGuid { get; set; }
        public DateTimeOffset? ConnectionStartDt { get; set; }
    }
}
