namespace AirPro.Service.DTOs.Interface
{
    public interface ITroubleCodeRecommendationUsageDto
    {
        int VehicleMakeId { get; set; }
        string VehicleMakeName { get; set; }
        int ControllerId { get; set; }
        string ControllerName { get; set; }
        int TroubleCodeId { get; set; }
        string TroubleCode { get; set; }
        string TroubleCodeDescription { get; set; }
        int UsageCount { get; set; }
    }
}