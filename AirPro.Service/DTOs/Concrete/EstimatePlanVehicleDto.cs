using AirPro.Service.DTOs.Interface;

namespace AirPro.Service.DTOs.Concrete
{
    internal class EstimatePlanVehicleDto : IEstimatePlanVehicleDto
    {
        public int EstimatePlanId { get; set; }
        public int VehicleMakeId { get; set; }
        public string VehicleMakeName { get; set; }
        public decimal CompletionCost { get; set; }
    }
}