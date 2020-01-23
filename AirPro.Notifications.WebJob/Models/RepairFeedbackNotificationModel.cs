using System;

namespace AirPro.Notifications.WebJob.Models
{
    internal class RepairFeedbackNotificationModel
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

        public int ResponseTime { get; set; }
        public int RequestTime { get; set; }
        public int TechnicianKnowledge { get; set; }
        public int ReportCompletion { get; set; }
        public int ConcernsAddressed { get; set; }
        public int TechnicianCommunication { get; set; }
        public string AdditionalFeedback { get; set; }
        public float Average { get; set; }

        public DateTime RepairLastUpdated { get; set; }
    }
}
