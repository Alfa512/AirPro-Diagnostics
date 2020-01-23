using AirPro.Service.DTOs.Interface;

namespace AirPro.Site.Areas.Admin.Models.Decision
{
    public class DecisionVehicleMakeViewModel : IDecisionVehicleMakeDto
    {
        public int VehicleMakeId { get; set; }
        public string VehicleMakeName { get; set; }
        public bool SelectedInd { get; set; }
        public bool PreSelectedInd { get; set; }
    }
}