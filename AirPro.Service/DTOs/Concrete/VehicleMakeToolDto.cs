using AirPro.Service.DTOs.Interface;

namespace AirPro.Service.DTOs.Concrete
{
    public class VehicleMakeToolDto : IVehicleMakeToolDto
    {
        public int? VehicleMakeToolId { get; set; }
        public string Name { get; set; }
    }
}
