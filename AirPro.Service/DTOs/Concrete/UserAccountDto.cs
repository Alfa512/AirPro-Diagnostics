using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Service.DTOs.Concrete
{
    public class UserAccountDto : IUserAccountDto
    {
        public Guid UserGuid { get; set; }
        public Guid AccountGuid { get; set; }
    }
}
