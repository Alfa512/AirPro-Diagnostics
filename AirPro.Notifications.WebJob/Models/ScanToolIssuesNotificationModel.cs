using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirPro.Notifications.WebJob.Models
{
    internal class ScanToolIssuesNotificationModel
    {
        public int RepairId { get; set; }
        public Guid ShopGuid { get; set; }
        public string ShopName { get; set; }
        public string ShopPhone { get; set; }
        public string ShopRONumber { get; set; }

        public string VehicleVIN { get; set; }
        public string VehicleMake { get; set; }
        public string VehicleModel { get; set; }
        public string VehicleYear { get; set; }
        
        public int RequestId { get; set; }
        public int ReportId { get; set; }
        public string CancellationNotes { get; set; }
        public string ReportNotes { get; set; }
        public string TechnicianNotes { get; set; }
    }
}
