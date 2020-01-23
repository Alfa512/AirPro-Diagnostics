using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirPro.Notifications.WebJob.Models
{
    public class RegistrationNotificationModel
    {
        public Guid RegistrationId { get; set; }
        public string Email { get; set; }
        public string CreatedBy { get; set; }
        public string ClientName { get; set; }
    }
}
