using AirPro.Service.DTOs.Interface;

namespace AirPro.Service.DTOs.Concrete
{
    internal class ReportFrequentRecommendationSelectionItemDto : IReportFrequentRecommendationSelectionItemDto
    {
        public int ControllerId { get; set; }
        public int TroubleCodeId { get; set; }
        public int TroubleCodeRecommendationId { get; set; }
        public string TroubleCodeRecommendationText { get; set; }
        public int TroubleCodeRecommendationRank { get; set; }
    }
}