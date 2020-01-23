using AirPro.Service.DTOs.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirPro.Service.DTOs.Concrete
{
    internal class RegistrationAccountDto : IRegistrationAccountDto
    {
        public int RegistrationAccountId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public int? DiscountPercentage { get; set; }
        public string StateId { get; set; }
    }
}
