using System;

namespace AirPro.Entities.Interfaces
{
    public interface IUserEntityModel
    {
        string UserName { get; set; }
        string Email { get; set; }
        bool EmailConfirmed { get; set; }
        string ContactNumber { get; set; }
        string FirstName { get; set; }
        string JobTitle { get; set; }
        string LastName { get; set; }
        string PhoneNumber { get; set; }
        bool PhoneNumberConfirmed { get; set; }
        string SessionId { get; set; }
        bool ShopBillingNotification { get; set; }
        bool ShopReportNotification { get; set; }
        bool ShopStatementNotification { get; set; }
        string TimeZoneInfoId { get; set; }
        bool TwoFactorEnabled { get; set; }
        bool LockoutEnabled { get; set; }
        DateTime? LockoutEndDateUtc { get; set; }
        int AccessFailedCount { get; set; }
        bool EmployeeInd { get; set; }
    }
}