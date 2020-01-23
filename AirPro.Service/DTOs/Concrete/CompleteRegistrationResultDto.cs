using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirPro.Service.DTOs.Concrete
{
    public class CompleteRegistrationResultDto
    {
        public Guid AccountGuid { get; set; }
        public Guid UserGuid { get; set; }
        public Guid ShopGuid { get; set; }
    }
}
