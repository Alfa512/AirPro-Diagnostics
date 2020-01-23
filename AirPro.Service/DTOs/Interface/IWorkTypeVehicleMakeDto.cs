namespace AirPro.Service.DTOs.Interface
{
    public interface IWorkTypeVehicleMakeDto
    {
        int VehicleMakeId { get; set; }
        string VehicleMakeName { get; set; }
        bool SelectedInd { get; set; }
    }
}