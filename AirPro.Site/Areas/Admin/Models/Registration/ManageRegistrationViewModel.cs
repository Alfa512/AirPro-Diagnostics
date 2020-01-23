using AirPro.Common.Enumerations;
using AirPro.Service.DTOs.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using UniMatrix.Common.Extensions;

namespace AirPro.Site.Areas.Admin.Models.Registration
{
    public class ManageRegistrationViewModel : IRegistrationDto
    {
        public ManageRegistrationViewModel()
        {
            User = new UserDetailsViewModel();
            Account = new AccountInformationViewModel();
            Shop = new ShopInformationViewModel();
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
        public Guid? StatusUpdateByUserGuid { get; set; }
        [Display(Name = "Status Since")]
        public DateTimeOffset? StatusUpdateDt { get; set; } = null;
        public Guid? CompletedByUserGuid { get; set; }
        public string CompletedBy { get; set; }
        [Required]
        [Remote("IsEmailValid", "Registration", "Admin", AdditionalFields = "RegistrationId", HttpMethod = "POST", ErrorMessage = "Email already exists in database.")]
        public string Email { get; set; }
        [Display(Name = "Registration Completed")]
        public DateTimeOffset? CompletedDt { get; set; } = null;
        [Display(Name = "Created Account")]
        public string CreatedAccount { get; set; }
        [Display(Name = "Created Shop")]
        public string CreatedShop { get; set; }
        [Display(Name = "Created User")]
        public string CreatedUser { get; set; }
        [Display(Name = "Callback Url")]
        public string CallbackUrl { get; set; }
        [Display(Name = "Different Shop Info?")]
        public bool DifferentShopInfo { get; set; }
        [Display(Name = "Time Zone")]
        public string TimeZoneInfoId
        {
            get => User.TimeZoneInfoId;
            set => User.TimeZoneInfoId = value;
        }

        [Display(Name = "Access Groups")]
        public List<Guid> AccessGroupIds
        {
            get => User.AccessGroupIds;
            set => User.AccessGroupIds = value;
        }
        public UserDetailsViewModel User { get; set; }
        public ShopInformationViewModel Shop { get; set; }
        public AccountInformationViewModel Account { get; set; }

        public string StatusUpdateBy { get; set; }
        public Guid? ShopGuid { get; set; }
        public Guid? ClientUserGuid { get; set; }
        public Guid? AccountGuid { get; set; }
        public IRegistrationAccountDto RegistrationAccount
        {
            get => Account;
            set => Account = (AccountInformationViewModel)value;
        }
        public IRegistrationShopDto RegistrationShop
        {
            get => Shop;
            set => Shop = (ShopInformationViewModel)value;
        }
        public IRegistrationUserDto RegistrationUser
        {
            get => User;
            set => User = (UserDetailsViewModel)value;
        }

        public IUpdateResultDto UpdateResult { get; set; }
        public Guid? CreatedByUserGuid { get; set; }
        public string CreatedBy { get; set; }
        [Display(Name = "Registration Created")]
        public DateTimeOffset? CreatedDt { get; set; }
        public DateTimeOffset? UpdatedDt { get; set; }
        public string RegistrationStatusString => Enum.GetName(typeof(RegistrationStatus), RegistrationStatus);

        public List<KeyValuePair<string, string>> StateSelection { get; set; }
        public List<KeyValuePair<string, string>> PricingPlanSelection { get; set; }
        public List<KeyValuePair<string, string>> EstimatePlanSelection { get; set; }
        public List<KeyValuePair<string, string>> BillingCycleSelection { get; set; }

        public List<KeyValuePair<string, string>> GroupSelection { get; set; }
        public List<KeyValuePair<string, string>> RequestTypeSelection { get; set; }
        public List<KeyValuePair<string, string>> CurrencySelection { get; set; }
        public List<IInsuranceCompanyDto> InsuranceCompanies { get; set; }

        public List<KeyValuePair<string, string>> DirectPartnersSelection
        {
            get
            {
                if (InsuranceCompanies != null)
                {
                    return InsuranceCompanies
                        .Where(m => !string.IsNullOrEmpty(m.ProgramName))
                        .Select(d => new KeyValuePair<string, string>(d.InsuranceCompanyId.ToString(), d.ProgramName))
                        .OrderBy(m => m.Value)
                        .ToList();
                }

                return new List<KeyValuePair<string, string>>();
            }
        }
        public List<KeyValuePair<string, string>> InsuranceCompaniesSelection
        {
            get
            {
                if (InsuranceCompanies != null)
                {
                    var result = InsuranceCompanies
                        .Where(m => !string.IsNullOrEmpty(m.InsuranceCompanyName))
                        .Select(d => new KeyValuePair<string, string>(d.InsuranceCompanyId.ToString(), d.InsuranceCompanyName))
                        .OrderBy(m => m.Value)
                        .ToList();
                    result.Insert(0, new KeyValuePair<string, string>("0", @"<-- Not Assigned -->"));
                    return result;
                }

                return new List<KeyValuePair<string, string>>();
            }
        }
        public List<IVehicleMakeDto> AllVehicleMakes { get; set; }
        public List<KeyValuePair<string, string>> Programs
        {
            get
            {
                if (AllVehicleMakes != null)
                {
                    return AllVehicleMakes
                        .Where(m => !string.IsNullOrEmpty(m.ProgramName))
                        .Select(d => new KeyValuePair<string, string>(d.VehicleMakeId.ToString(), d.ProgramName))
                        .OrderBy(m => m.Value)
                        .ToList();
                }

                return new List<KeyValuePair<string, string>>();
            }
        }
        public List<KeyValuePair<string, string>> VehicleMakes
        {
            get
            {
                if (AllVehicleMakes != null)
                {
                    return AllVehicleMakes
                        .Select(d => new KeyValuePair<string, string>(d.VehicleMakeId.ToString(), d.VehicleMakeName.ToString()))
                        .OrderBy(m => m.Value)
                        .ToList();
                }

                return new List<KeyValuePair<string, string>>();
            }
        }
        public List<SelectListItem> AllInsuranceCompanies
        {
            get
            {
                return InsuranceCompaniesSelection.Select(x => new SelectListItem() { Text = x.Value, Value = x.Key }).ToList();
            }
        }

        public int? PassedStep { get; set; }
    }
}