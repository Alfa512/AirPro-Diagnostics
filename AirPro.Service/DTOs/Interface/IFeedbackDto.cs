namespace AirPro.Service.DTOs.Interface
{
    public interface IFeedbackDto
    {
        int RepairId { get; set; }
        int ResponseTimeRate { get; set; }
        int TechnicianKnowledgeRate { get; set; }
        int ReportCompletionRate { get; set; }
        int ConcernsAddressedRate { get; set; }
        int TechnicianCommunicationRate { get; set; }
        string AdditionalFeedback { get; set; }
        IUpdateResultDto UpdateResult { get; set; }
    }
}
