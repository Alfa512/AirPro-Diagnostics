using System;

namespace AirPro.Notifications.WebJob.Models
{
    internal class RepairNotificationModel
    {
        public int RepairId { get; set; }
        public string VehicleVIN { get; set; }
        public string VehicleMake { get; set; }
        public string VehicleModel { get; set; }
        public string VehicleYear { get; set; }
        public string ShopRONumber { get; set; }
        public Guid ShopGuid { get; set; }
        public string ShopName { get; set; }
        public string ShopPhone { get; set; }
        public bool InvoicedInd { get; set; }
        public decimal InvoiceTotal { get; set; }
        public int VehicleMakeId { get; set; }
    }
}
