using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirPro.Service.DTOs.Interface
{
    public interface IShopContactDto
    {
        Guid? ShopContactGuid { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string PhoneNumber { get; set; }
        Guid? ShopGuid { get; set; }
        bool HasRequests { get; set; }
    }
}
