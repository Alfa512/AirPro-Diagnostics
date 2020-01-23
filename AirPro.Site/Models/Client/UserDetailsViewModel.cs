using AirPro.Service.DTOs.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AirPro.Site.Models.Client
{
    public class UserDetailsViewModel : IRegistrationUserDto
    {
        public int RegistrationUserId { get; set; }
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string JobTitle { get; set; }
        public string ContactNumber { get; set; }
        [Display(Name = "Mobile Number")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Invoice Notifications")]
        public bool ShopBillingNotification { get; set; }
        [Display(Name = "Report Notifications")]
        public bool ShopReportNotification { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public string GetDisplayName => $"{LastName}, {FirstName}";
        [Display(Name = "Time Zone")]
        public string TimeZoneInfoId { get; set; }
        [Display(Name = "Access Groups")]
        public List<Guid> AccessGroupIds { get; set; }
        public string PasswordHash { get; set; }
    }
}