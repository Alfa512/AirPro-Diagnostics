using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirPro.Entities.Access
{
    [Table("RegistrationUsers", Schema = "Access")]
    public class RegistrationUserEntityModel
    {
        [Key]
        public int RegistrationUserId { get; set; }
        [MaxLength(256)]
        public string FirstName { get; set; }
        [MaxLength(256)]
        public string LastName { get; set; }
        [MaxLength(256)]
        public string JobTitle { get; set; }
        [MaxLength(50)]
        public string ContactNumber { get; set; }
        [MaxLength(50)]
        public string PhoneNumber { get; set; }
        public string AccessGroupIds { get; set; }
        public bool ShopBillingNotification { get; set; }
        public bool ShopReportNotification { get; set; }
        [MaxLength(128)]
        public string TimeZoneInfoId { get; set; } = "Eastern Standard Time";
        public string GetDisplayName => $"{LastName}, {FirstName}";
        public string PasswordHash { get; set; }
    }
}
