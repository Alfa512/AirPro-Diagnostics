namespace AirPro.Service.DTOs.Interface
{
    public interface IDecisionVehicleMakeDto
    {
        int VehicleMakeId { get; set; }
        string VehicleMakeName { get; set; }
        bool SelectedInd { get; set; }
        bool PreSelectedInd { get; set; }
    }
}