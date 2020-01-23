using System;
using System.Collections.Generic;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Service.DTOs.Concrete
{
    [Serializable]
    internal class AccountDto : IAccountDto
    {
        public Guid AccountGuid { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public int DiscountPercentage { get; set; }
        public bool ActiveInd { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public Guid? EmployeeGuid { get; set; }
        public string AccountRep { get; set; }

        public IUpdateResultDto UpdateResult { get; set; }
        public ICollection<IUserDto> Users { get; set; }
    }
}
