using System;
using System.Collections.Generic;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Service.DTOs.Concrete
{
    [Serializable]
    internal class UserDto : IUserDto
    {
        public Guid UserGuid { get; set; }
        public string PasswordHash { get; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public string JobTitle { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }

        public bool AccountLocked { get; set; }

        public int AccessFailedCount { get; set; }

        public bool ShopBillingNotification { get; set; }
        public bool ShopReportNotification { get; set; }
        public bool ShopStatementNotification { get; set; }

        public ICollection<Guid> GroupMemberships { get; set; }
        public ICollection<Guid> AccountMemberships { get; set; }
        public ICollection<Guid> ShopMemberships { get; set; }

        public string TimeZoneInfoId { get; set; }
        public bool EmployeeInd { get; set; }
        public bool EmployeeAssignedInd { get; set; }

        public IUpdateResultDto UpdateResult { get; set; }
    }
}

