using AirPro.Common.Enumerations;
using AirPro.Service.DTOs.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using UniMatrix.Common.Extensions;

namespace AirPro.Site.Models.Client
{
    public class CreateViewModel : IRegistrationDto
    {
        public CreateViewModel()
        {
            UserDetails = new UserDetailsViewModel();
            AccountInformation = new AccountInformationViewModel();
            ShopInformation = new ShopInformationViewModel();
            RepairInformation = new RepairInformationViewModel();
            ExternalServices = new ExternalServicesViewModel();
        }
        [Display(Name = "Registration ID")]
        public Guid RegistrationId { get; set; }
        public string ShortRegistrationId
        {
            get
            {
                return RegistrationId.ToShortGuid();
            }
        }
        [Display(Name = "Status")]
        public RegistrationStatus RegistrationStatus { get; set; }
        public string RegistrationStatusString
        {
            get
            {
                return Enum.GetName(typeof(RegistrationStatus), RegistrationStatus);
            }
        }
        [Required]
        public string Email { get; set; }
        public Guid? StatusUpdateByUserGuid { get; set; }
        public string StatusUpdateBy { get; set; }
        [Display(Name = "Status Since")]
        public DateTimeOffset? StatusUpdateDt { get; set; } = null;
        public Guid? CompletedByUserGuid { get; set; }
        public string CompletedBy { get; set; }
        public DateTimeOffset? CompletedDt { get; set; } = null;
        public Guid? CreatedByUserGuid { get; set; }
        public string CreatedBy { get; set; }
        [Display(Name = "Registration Created")]
        public DateTimeOffset? CreatedDt { get; set; }
        public DateTimeOffset? UpdatedDt { get; set; }
        public Guid? ShopGuid { get; set; }
        public Guid? ClientUserGuid { get; set; }
        public string Client { get; set; }
        [Display(Name = "Different Shop Info?")]
        public bool DifferentShopInfo { get; set; }
        public string CallbackUrl { get; set; }
        public Guid? AccountGuid { get; set; }
        public UserDetailsViewModel UserDetails { get; set; }
        public AccountInformationViewModel AccountInformation { get; set; }
        public ShopInformationViewModel ShopInformation { get; set; }
        public RepairInformationViewModel RepairInformation { get; set; }
        public ExternalServicesViewModel ExternalServices { get; set; }
        public IRegistrationUserDto RegistrationUser { get { return UserDetails; } set { UserDetails = (UserDetailsViewModel)value; } }
        public IRegistrationAccountDto RegistrationAccount { get { return AccountInformation; } set { AccountInformation = (AccountInformationViewModel)value; } }
        public IRegistrationShopDto RegistrationShop { get { return ShopInformation; } set { ShopInformation = (ShopInformationViewModel)value; } }

        public IUpdateResultDto UpdateResult { get; set; }

        public IEnumerable<KeyValuePair<string, string>> States { get; set; }
        public IEnumerable<KeyValuePair<string, string>> BillingCycles { get; set; }
        public List<KeyValuePair<string, string>> DirectRepairPartners { get; set; }
        public List<KeyValuePair<string, string>> OEMCertifications { get; set; }
        public string CreatedUser { get; set; }
        public string CreatedShop { get; set; }
        public string CreatedAccount { get; set; }
        public int? PassedStep { get; set; }
    }
}