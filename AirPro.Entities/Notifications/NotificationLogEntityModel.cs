using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirPro.Entities.Notifications
{
    [Table("Logs", Schema = "Notification")]
    public class NotificationLogEntityModel
    {
        [Key]
        public int NotificationLogId { get; set; }

        public string Destination { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public string StatusMessage { get; set; }

        public DateTimeOffset CreatedDt { get; set; } = DateTimeOffset.UtcNow;
    }
}
