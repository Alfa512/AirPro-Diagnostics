using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Site.Areas.Admin.Models.Access
{
    public class AccountViewModel : IAccountDto
    { 
        [Required]
        public Guid AccountGuid { get; set; } = Guid.Empty;
        [Display(Name = "Address 1")]
        public string Address1 { get; set; }
        [Display(Name = "Address 2")]
        public string Address2 { get; set; }
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        public string Zip { get; set; }
        [Required, Display(Name = "Name")]
        [Remote("IsAccountNameExist", "Access", "Admin", AdditionalFields = "AccountGuid", HttpMethod = "POST", ErrorMessage = "Account name already exists in database.")]
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }

        [Display(Name = "Discount")]
        public int DiscountPercentage { get; set; }

        public IUpdateResultDto UpdateResult { get; set; }
        public ICollection<IUserDto> Users { get; set; }

        public bool AllowEntry { get; set; }
        [Display(Name = "Enabled")]
        public bool ActiveInd { get; set; } = true;
        [Display(Name = "Account Rep")]
        public Guid? EmployeeGuid { get; set; }

        public string AccountRep { get; set; }
    }
}