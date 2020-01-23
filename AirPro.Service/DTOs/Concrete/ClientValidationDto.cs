using AirPro.Service.DTOs.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirPro.Service.DTOs.Concrete
{
    internal class ClientValidationDto : IClientValidationDto
    {
        public bool IsValid { get; set; }
    }
}
