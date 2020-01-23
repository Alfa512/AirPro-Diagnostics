using AirPro.Common.Enumerations;
using AirPro.Service.DTOs.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirPro.Service.DTOs.Concrete
{
    public class CancelReasonTypeDto : ICancelReasonTypeDto
    {
        public int CancelReasonTypeId { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public NotificationTemplate? NotificationTemplate { get; set; }
    }
}
