using AirPro.Service.DTOs.Interface;

namespace AirPro.Site.Areas.Admin.Models.Recommendation
{
    public class RecommendationUsageViewModel : ITroubleCodeRecommendationUsageDto
    {
        public int VehicleMakeId { get; set; }
        public string VehicleMakeName { get; set; }
        public int ControllerId { get; set; }
        public string ControllerName { get; set; }
        public int TroubleCodeId { get; set; }
        public string TroubleCode { get; set; }
        public string TroubleCodeDescription { get; set; }
        public int UsageCount { get; set; }
    }
}