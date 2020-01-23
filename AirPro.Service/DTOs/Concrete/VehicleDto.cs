using AirPro.Common.Enumerations;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Service.DTOs.Concrete
{
    internal class VehicleDto : IVehicleDto
    {
        public string VehicleVIN { get; set; }
        public int VehicleMakeId { get; set; }
        public string VehicleMakeName { get; set; }
        public string VehicleModel { get; set; }
        public string VehicleYear { get; set; }
        public string VehicleTransmission { get; set; }
        public int VehicleMakeTypeId { get; set; }
        public string VehicleMakeTypeName { get; set; }
        public VehicleLookupService? LookupService { get; set; }
        public bool ManualEntryInd { get; set; } = true;
    }
}