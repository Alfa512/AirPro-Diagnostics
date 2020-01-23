namespace AirPro.Service.DTOs.Interface
{
    public interface IEstimatePlanVehicleDto
    {
        int EstimatePlanId { get; set; }
        int VehicleMakeId { get; set; }
        string VehicleMakeName { get; set; }
        decimal CompletionCost { get; set; }
    }
}