using System;

namespace AirPro.Notifications.WebJob.Models
{
    internal class UserMembershipModel
    {
        public string AccessType { get; set; }
        public Guid AccountGuid { get; set; }
        public string AccountName { get; set; }
        public Guid ShopGuid { get; set; }
        public string ShopName { get; set; }
        public Guid UserGuid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TimeZoneInfoId { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool ShopBillingNotification { get; set; }
        public bool ShopReportNotification { get; set; }
        public bool ShopStatementNotification { get; set; }
        public bool DisableShopBillingNotification { get; set; }
        public bool DisableShopStatementNotification { get; set; }
    }
}
