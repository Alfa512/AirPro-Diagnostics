using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AirPro.Entities.Access;

namespace AirPro.Entities.Notifications
{
    [Table("TypeRoles", Schema = "Notification")]
    public class NotificationTypeRoleEntityModel
    {
        [Column(Order = 0), Key, Required, ForeignKey(nameof(Type))]
        public Guid TypeGuid { get; set; }
        public virtual NotificationTypeEntityModel Type { get; set; }

        [Column(Order = 1), Key, Required, ForeignKey(nameof(Role))]
        public Guid RoleGuid { get; set; }
        public virtual RoleEntityModel Role { get; set; }
    }
}