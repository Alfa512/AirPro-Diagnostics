using AirPro.Service.DTOs.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AirPro.Site.Areas.Admin.Models.Registration
{
    public class UserDetailsViewModel : IRegistrationUserDto
    {
        public int RegistrationUserId { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "Job Title")]
        public string JobTitle { get; set; }
        [Display(Name = "Contact Number")]
        public string ContactNumber { get; set; }
        [Display(Name = "Mobile Number")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Invoice Notifications")]
        public bool ShopBillingNotification { get; set; }
        [Display(Name = "Report Notifications")]
        public bool ShopReportNotification { get; set; }
        public List<SelectListItem> GroupsSelection { get; set; }
        [Display(Name = "Access Groups")]
        public List<Guid> AccessGroupIds { get; set; }

        public string GetDisplayName => $"{LastName}, {FirstName}";
        [Display(Name = "Time Zone")]
        public string TimeZoneInfoId { get; set; }
        public string PasswordHash { get; set; }
    }
}