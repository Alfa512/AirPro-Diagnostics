using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirPro.Entities.Notifications
{
    [Table("Templates", Schema = "Notification")]
    public class NotificationTemplateEntityModel : AuditBaseEntityModel
    {
        [Key]
        public int NotificationTemplateId { get; set; }

        public string Name { get; set; }

        public string Options { get; set; } // Array String

        public string Subject { get; set; }

        public string EmailBody { get; set; }

        public string TextMessage { get; set; }
    }
}
