using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirPro.Notifications.WebJob.Models
{
    public class RegistrationWelcomeNotificationModel
    {
        public Guid RegistrationId { get; set; }
        public string Email { get; set; }
        public string CreatedBy { get; set; }
        public string ClientName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string JobTitle { get; set; }
        public string ShopName { get; set; }
        public string AccountName { get; set; }
    }
}
