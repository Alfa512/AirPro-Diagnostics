using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Service.DTOs.Concrete
{
    internal class ShopContactDto : IShopContactDto
    {
        public Guid? ShopContactGuid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public Guid? ShopGuid { get; set; }
        public bool HasRequests { get; set; }
    }
}
