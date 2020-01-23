using AirPro.Service.DTOs.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirPro.Site.Areas.Admin.Models.Access
{
    public class ShopContactViewModel : IShopContactDto
    {
        public Guid? ShopContactGuid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public Guid? ShopGuid { get; set; }
        public bool HasRequests { get; set; }
    }
}