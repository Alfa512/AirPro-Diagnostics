using AirPro.Common.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirPro.Service.DTOs.Interface
{
    public interface ICancelReasonTypeDto
    {
        int CancelReasonTypeId { get; set; }
        string Name { get; set; }
        int Order { get; set; }
        NotificationTemplate? NotificationTemplate { get; set; }
    }
}
