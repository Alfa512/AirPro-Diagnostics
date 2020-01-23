using AirPro.Service.DTOs.Interface;

namespace AirPro.Service.DTOs.Concrete
{
    public class FeedbackDto : IFeedbackDto
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
