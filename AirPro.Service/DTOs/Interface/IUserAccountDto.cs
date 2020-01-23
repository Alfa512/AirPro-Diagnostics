using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirPro.Service.DTOs.Interface
{
    public interface IUserAccountDto
    {
        Guid UserGuid { get; set; }
        Guid AccountGuid { get; set; }
    }
}
