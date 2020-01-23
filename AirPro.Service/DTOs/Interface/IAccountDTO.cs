using System;
using System.Collections.Generic;
using AirPro.Common.Interfaces;

namespace AirPro.Service.DTOs.Interface
{
    public interface IAccountDto : IMailingAddress
    {
        Guid AccountGuid { get; set; }
        string Name { get; set; }
        string Phone { get; set; }
        string Fax { get; set; }
        int DiscountPercentage { get; set; }
        bool ActiveInd { get; set; }
        IUpdateResultDto UpdateResult { get; set; }
        ICollection<IUserDto> Users { get; set; }
        Guid? EmployeeGuid { get; set; }
        string AccountRep { get; set; }
    }
}