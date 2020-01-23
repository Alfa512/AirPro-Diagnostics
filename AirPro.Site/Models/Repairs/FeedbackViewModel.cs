using AirPro.Service.DTOs.Interface;

namespace AirPro.Site.Models.Repairs
{
    public class FeedbackViewModel : IFeedbackDto
    {
        public int RepairId { get; set; }
        public int ResponseTimeRate { get; set; }
        public int TechnicianKnowledgeRate { get; set; }
        public int ReportCompletionRate { get; set; }
        public int ConcernsAddressedRate { get; set; }
        public int TechnicianCommunicationRate { get; set; }
        public string AdditionalFeedback { get; set; }
        public IUpdateResultDto UpdateResult { get; set; }
    }
}