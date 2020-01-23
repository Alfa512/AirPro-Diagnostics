using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirPro.Service.DTOs.Interface
{
    public interface IRegistrationAccountDto
    {
        string Address1 { get; set; }
        string Address2 { get; set; }
        string City { get; set; }
        int? DiscountPercentage { get; set; }
        string Fax { get; set; }
        string Name { get; set; }
        string Phone { get; set; }
        int RegistrationAccountId { get; set; }
        string Zip { get; set; }
        string StateId { get; set; }
    }
}
