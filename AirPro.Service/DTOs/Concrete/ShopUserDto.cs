using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirPro.Service.DTOs.Concrete
{
    internal class ShopUserDto : UserDto
    {
        public Guid ShopGuid { get; set; }
    }
}
