using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AirPro.Entities.Access;

namespace AirPro.Entities.Notifications
{
    [Table("UserOptOuts", Schema = "Notification")]
    public class NotificationUserOptOutEntityModel
    {
        [Column(Order = 0), Key, Required, ForeignKey(nameof(Type))]
        public Guid TypeGuid { get; set; }
        public virtual NotificationTypeEntityModel Type { get; set; }

        [Column(Order = 1), Key, Required, ForeignKey(nameof(User))]
        public Guid UserGuid { get; set; }
        public virtual UserEntityModel User { get; set; }
    }
}