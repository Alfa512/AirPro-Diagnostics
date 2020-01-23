using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirPro.Entities.Access
{
    [Table("Logins", Schema = "Access")]
    public class LoginEntityModel
    {
        [Key]
        public int LoginId { get; set; }
        [ForeignKey(nameof(User))]
        public Guid? UserGuid { get; set; }
        public UserEntityModel User { get; set; }
        public string LoginName { get; set; }
        public string UserAgent { get; set; }
        public string UserHostAddress { get; set; }
        public string UserHostName { get; set; }
        public bool AccountLockedOut { get; set; } = false;
        public bool TwoFactorChallenge { get; set; } = false;
        public bool TwoFactorVerified { get; set; } = false;
        public DateTimeOffset LoginAttemptDt { get; set; } = DateTimeOffset.UtcNow;
    }
}
