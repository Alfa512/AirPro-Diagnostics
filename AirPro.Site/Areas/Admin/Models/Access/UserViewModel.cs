using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Site.Areas.Admin.Models.Access
{
    public class UserViewModel : IUserDto
    {
        public Guid UserGuid { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Confirm Password"), DataType(DataType.Password), Compare("Password")]
        public string ConfirmPassword { get; set; }
        public string PasswordHash { get; set; }
        [Required, Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required, Display(Name = "Last Name")]
        public string LastName { get; set; }

        public string DisplayName { get; set; }

        [Display(Name = "Job Title")]
        public string JobTitle { get; set; }
        [Display(Name = "Contact Number")]
        public string ContactNumber { get; set; }
        [Required]
        public string Email { get; set; }
        [Display(Name = "Email Confirmed")]
        public bool EmailConfirmed { get; set; }
        [Display(Name = "Mobile Number")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Mobile Confirmed")]
        public bool PhoneNumberConfirmed { get; set; }
        [Display(Name = "Two Factor Enabled")]
        public bool TwoFactorEnabled { get; set; }

        [Display(Name = "Account Locked")]
        public bool AccountLocked { get; set; }

        [Display(Name = "Failed Login Count")]
        public int AccessFailedCount { get; set; }

        [Display(Name = "Invoice Notifications")]
        public bool ShopBillingNotification { get; set; }

        [Display(Name = "Report Notifications")]
        public bool ShopReportNotification { get; set; }

        [Display(Name = "Statement Notifications")]
        public bool ShopStatementNotification { get; set; }

        public ICollection<KeyValuePair<string, string>> AvailableGroups { get; set; }

        public ICollection<Guid> GroupMemberships { get; set; }
        public ICollection<Guid> AccountMemberships { get; set; }
        public ICollection<Guid> ShopMemberships { get; set; }

        [Display(Name = "Time Zone")]
        public string TimeZoneInfoId { get; set; }

        public bool EmployeeAssignedInd { get; set; }
        public IUpdateResultDto UpdateResult { get; set; }

        public bool AllowEntry { get; set; }
        public ITechnicianProfileDto TechnicianProfile { get; set; }
        [Display(Name = "AirPro Employee")]
        public bool EmployeeInd { get; set; }
    }
}