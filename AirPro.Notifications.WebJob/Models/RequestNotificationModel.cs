using System;

namespace AirPro.Notifications.WebJob.Models
{
    internal class RequestNotificationModel
    {
        public int RepairId { get; set; }
        public int RequestId { get; set; }
        public string RequestType { get; set; }
        public string ProblemDescription { get; set; }
        public string VehicleVIN { get; set; }
        public int VehicleMakeId { get; set; }
        public string VehicleMake { get; set; }
        public string VehicleModel { get; set; }
        public string VehicleYear { get; set; }
        public string ShopRONumber { get; set; }
        public Guid ShopGuid { get; set; }
        public string ShopName { get; set; }
        public string ShopPhone { get; set; }
        public bool ReportCompletedInd { get; set; }
        public bool EstimatePlanInd { get; set; }
    }
}
