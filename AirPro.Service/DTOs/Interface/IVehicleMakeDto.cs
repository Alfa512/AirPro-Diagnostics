using System.Collections.Generic;

namespace AirPro.Service.DTOs.Interface
{
    public interface IVehicleMakeDto
    {
        int VehicleMakeId { get; set; }
        string VehicleMakeName { get; set; }
        int VehicleMakeTypeId { get; set; }
        string VehicleMakeTypeName { get; set; }
        string ProgramName { get; set; }
        string ProgramInstructions { get; set; }
        List<IVehicleMakeToolDto> ProgramTools { get; set; }
        IUpdateResultDto UpdateResult { get; set; }
    }
}