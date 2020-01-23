using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirPro.Service.DTOs.Interface
{
    public interface IRegistrationUserDto
    {
        List<Guid> AccessGroupIds { get; set; }
        string ContactNumber { get; set; }
        string FirstName { get; set; }
        string GetDisplayName { get; }
        string JobTitle { get; set; }
        string LastName { get; set; }
        string PhoneNumber { get; set; }
        int RegistrationUserId { get; set; }
        bool ShopBillingNotification { get; set; }
        bool ShopReportNotification { get; set; }
        string TimeZoneInfoId { get; set; }
        string PasswordHash { get; set; }
    }
}
