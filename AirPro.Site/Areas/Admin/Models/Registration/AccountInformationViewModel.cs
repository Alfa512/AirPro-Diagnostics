using AirPro.Service.DTOs.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AirPro.Site.Areas.Admin.Models.Registration
{
    public class AccountInformationViewModel : IRegistrationAccountDto
    {
        public int RegistrationAccountId { get; set; }
        [Display(Name = "Account Name")]
        [Remote("IsAccountNameValid", "Registration", "Admin", AdditionalFields = "RegistrationAccountId", HttpMethod = "POST", ErrorMessage = "Account Name already exists in database.")]
        public string Name { get; set; }
        [Display(Name = "Address 1")]
        public string Address1 { get; set; }
        [Display(Name = "Address 2 (Optional)")]
        public string Address2 { get; set; }
        [Display(Name = "City")]
        public string City { get; set; }
        [Display(Name = "State")]
        public string StateId { get; set; }

        [Display(Name = "Discount Percentage")]
        public int? DiscountPercentage { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
    }
}