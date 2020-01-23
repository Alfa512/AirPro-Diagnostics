using AirPro.Service.DTOs.Interface;
using System.ComponentModel.DataAnnotations;

namespace AirPro.Site.Models.Client
{
    public class AccountInformationViewModel : IRegistrationAccountDto
    {
        public int RegistrationAccountId { get; set; }
        [Display(Name = "Account Name")]
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        [Display(Name = "State")]
        public string StateId { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        [Display(Name = "Billing Cycle")]
        public int? BillingCycleId { get; set; }
        public bool DifShopInfo { get; set; }

        [Display(Name = "Discount Percentage")]
        public int? DiscountPercentage { get; set; }
    }
}