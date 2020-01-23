using AirPro.Common.Enumerations;
using AirPro.Service.DTOs.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using UniMatrix.Common.Extensions;

namespace AirPro.Service.DTOs.Concrete
{
    internal class RegistrationDto : RegistrationOptionsDto, IRegistrationDto
    {
        public Guid RegistrationId { get; set; }
        public string ShortRegistrationId
        {
            get
            {
                return RegistrationId.ToShortGuid();
            }
        }

        public Guid? StatusUpdateByUserGuid { get; set; }

        public DateTimeOffset? StatusUpdateDt { get; set; } = null;

        public Guid? CompletedByUserGuid { get; set; }

        public DateTimeOffset? CompletedDt { get; set; } = null;

        public string Email { get; set; }

        public Guid? ShopGuid { get; set; }

        public Guid? ClientUserGuid { get; set; }
        public RegistrationStatus RegistrationStatus { get; set; }
        public string CallbackUrl { get; set; }
        public Guid? AccountGuid { get; set; }
        public bool ShopStateNotSelected { get; set; }
        public bool AccountStateNotSelected { get; set; }
        public bool AccountNameNotSelected { get; set; }
        public bool ShopNameNotSelected { get; set; }
        public IRegistrationAccountDto RegistrationAccount { get; set; }
        public IRegistrationShopDto RegistrationShop { get; set; }
        public IRegistrationUserDto RegistrationUser { get; set; }
        public string StatusUpdateBy { get; set; }
        public string CompletedBy { get; set; }
        public string CreatedUser { get; set; }
        public string CreatedShop { get; set; }
        public string CreatedAccount { get; set; }
        public IUpdateResultDto UpdateResult { get; set; }
        public Guid? CreatedByUserGuid { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset? CreatedDt { get; set; }
        public DateTimeOffset? UpdatedDt { get; set; }
        public bool DifferentShopInfo { get; set; }
        public int? PassedStep { get; set; }
        public string RegistrationStatusString
        {
            get
            {
                return Enum.GetName(typeof(RegistrationStatus), RegistrationStatus);
            }
        }

    }
}
