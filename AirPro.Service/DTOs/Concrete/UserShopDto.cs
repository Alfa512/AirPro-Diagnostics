﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Service.DTOs.Concrete
{
    public class UserShopDto : IUserShopDto
    {
        public Guid UserGuid { get; set; }
        public Guid ShopGuid { get; set; }
    }
}
