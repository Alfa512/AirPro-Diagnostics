using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirPro.Entities.Notifications
{
    [Table("Types", Schema = "Notification")]
    public class NotificationTypeEntityModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid TypeGuid { get; set; }

        public string Name { get; set; }
    }
}
