using System.Collections.Generic;

namespace AirPro.Service.DTOs.Interface
{
    public interface IEstimatePlanDto
    {
        int EstimatePlanId { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        bool ActiveInd { get; set; }
        List<IEstimatePlanVehicleDto> VehiclePlans { get; set; }
        IUpdateResultDto UpdateResult { get; set; }
    }
}