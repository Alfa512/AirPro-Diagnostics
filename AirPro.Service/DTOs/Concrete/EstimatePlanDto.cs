using System.Collections.Generic;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Service.DTOs.Concrete
{
    internal class EstimatePlanDto : IEstimatePlanDto
    {
        public int EstimatePlanId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool ActiveInd { get; set; }
        public List<IEstimatePlanVehicleDto> VehiclePlans { get; set; }
        public IUpdateResultDto UpdateResult { get; set; }
    }
}