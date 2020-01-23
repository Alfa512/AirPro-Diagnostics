using AirPro.Service.DTOs.Interface;

namespace AirPro.Service.DTOs.Concrete
{
    internal class TroubleCodeRecommendationUsageDto : ITroubleCodeRecommendationUsageDto
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