using AirPro.Service.DTOs.Interface;

namespace AirPro.Service.DTOs.Concrete
{
    internal class DecisionVehicleMakeDto : IDecisionVehicleMakeDto
    {
        public int VehicleMakeId { get; set; }
        public string VehicleMakeName { get; set; }
        public bool SelectedInd { get; set; }
        public bool PreSelectedInd { get; set; }
    }
}