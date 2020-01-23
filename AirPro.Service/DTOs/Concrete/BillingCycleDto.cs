using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Service.DTOs.Concrete
{
    public class BillingCycleDto : IBillingCycleDto
    {
        public int CycleId { get; set; }
        public string CycleName { get; set; }
    }
}
