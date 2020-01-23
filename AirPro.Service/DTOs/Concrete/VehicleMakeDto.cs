using System.Collections.Generic;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Service.DTOs.Concrete
{
    public class VehicleMakeDto : IVehicleMakeDto
    {
        public int VehicleMakeId { get; set; }
        public string VehicleMakeName { get; set; }
        public int VehicleMakeTypeId { get; set; }
        public string VehicleMakeTypeName { get; set; }
        public string ProgramName { get; set; }
        public string ProgramInstructions { get; set; }
        public List<IVehicleMakeToolDto> ProgramTools { get; set; }
        public IUpdateResultDto UpdateResult { get; set; }
    }
}
