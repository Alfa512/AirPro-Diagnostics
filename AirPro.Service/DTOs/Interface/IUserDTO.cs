using System;
using System.Collections.Generic;

namespace AirPro.Service.DTOs.Interface
{
    public interface IUserDto
    {
        Guid UserGuid { get; set; }

        string PasswordHash { get; }

        string FirstName { get; set; }
        string LastName { get; set; }
        string DisplayName { get; set; }
        string JobTitle { get; set; }

        string ContactNumber { get; set; }

        string Email { get; set; }
        bool EmailConfirmed { get; set; }

        string PhoneNumber { get; set; }
        bool PhoneNumberConfirmed { get; set; }

        bool TwoFactorEnabled { get; set; }

        bool AccountLocked { get; set; }
        int AccessFailedCount { get; set; }

        bool ShopBillingNotification { get; set; }
        bool ShopReportNotification { get; set; }
        bool ShopStatementNotification { get; set; }

        ICollection<Guid> GroupMemberships { get; set; }
        ICollection<Guid> AccountMemberships { get; set; }
        ICollection<Guid> ShopMemberships { get; set; }

        string TimeZoneInfoId { get; set; }
        bool EmployeeInd { get; set; }
        bool EmployeeAssignedInd { get; set; }

        IUpdateResultDto UpdateResult { get; set; }
    }
}