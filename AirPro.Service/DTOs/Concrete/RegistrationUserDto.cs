using AirPro.Service.DTOs.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AirPro.Service.DTOs.Concrete
{
    internal class RegistrationUserDto : IRegistrationUserDto
    {
        public int RegistrationUserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string JobTitle { get; set; }
        public string ContactNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string AccessGroupIdsString { get; set; }
        public List<Guid> AccessGroupIds
        {
            get
            {
                return AccessGroupIdsString?.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(x => Guid.Parse(x)).ToList() ?? new List<Guid>();
            }
            set
            {
                AccessGroupIdsString = string.Join(",", AccessGroupIds ?? new List<Guid>());
            }
        }
        public bool ShopBillingNotification { get; set; }
        public bool ShopReportNotification { get; set; }
        public string TimeZoneInfoId { get; set; } = "Eastern Standard Time";
        public string GetDisplayName => $"{LastName}, {FirstName}";
        public string PasswordHash { get; set; }
    }
}
