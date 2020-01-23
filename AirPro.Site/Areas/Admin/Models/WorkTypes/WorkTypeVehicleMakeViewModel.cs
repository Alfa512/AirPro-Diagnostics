using AirPro.Service.DTOs.Interface;

namespace AirPro.Site.Areas.Admin.Models.WorkTypes
{
    public class WorkTypeVehicleMakeViewModel : IWorkTypeVehicleMakeDto
    {
        public int VehicleMakeId { get; set; }
        public string VehicleMakeName { get; set; }
        public bool SelectedInd { get; set; }
    }
}