namespace AirPro.Service.DTOs.Interface
{
    public interface IReportFrequentRecommendationSelectionItemDto
    {
        int ControllerId { get; set; }
        int TroubleCodeId { get; set; }
        int TroubleCodeRecommendationId { get; set; }
        string TroubleCodeRecommendationText { get; set; }
        int TroubleCodeRecommendationRank { get; set; }
    }
}