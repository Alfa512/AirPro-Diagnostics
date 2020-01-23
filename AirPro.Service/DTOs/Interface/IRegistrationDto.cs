using AirPro.Common.Enumerations;
using System;
using System.Collections.Generic;

namespace AirPro.Service.DTOs.Interface
{
    public interface IRegistrationDto
    {
        Guid RegistrationId { get; set; }
        string ShortRegistrationId { get; }
        RegistrationStatus RegistrationStatus { get; set; }
        string RegistrationStatusString { get; }
        string Email { get; set; }
        Guid? StatusUpdateByUserGuid { get; set; }
        string StatusUpdateBy { get; set; }
        DateTimeOffset? StatusUpdateDt { get; set; }

        Guid? CompletedByUserGuid { get; set; }
        string CompletedBy { get; set; }

        DateTimeOffset? CompletedDt { get; set; }

        Guid? CreatedByUserGuid { get; set; }
        string CreatedBy { get; set; }

        DateTimeOffset? CreatedDt { get; set; }
        DateTimeOffset? UpdatedDt { get; set; }

        Guid? ShopGuid { get; set; }

        Guid? ClientUserGuid { get; set; }
        string CreatedUser { get; set; }
        string CreatedShop { get; set; }
        string CreatedAccount { get; set; }
        bool DifferentShopInfo { get; set; }

        string CallbackUrl { get; set; }
        Guid? AccountGuid { get; set; }
        IRegistrationAccountDto RegistrationAccount { get; set; }
        IRegistrationShopDto RegistrationShop { get; set; }
        IRegistrationUserDto RegistrationUser { get; set; }

        IUpdateResultDto UpdateResult { get; set; }
        int? PassedStep {get;set;}
    }
}
