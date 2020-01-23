using AirPro.Common.Enumerations;

namespace AirPro.Service.DTOs.Interface
{
    public interface IVehicleDto
    {
        string VehicleVIN { get; set; }
        int VehicleMakeId { get; set; }
        string VehicleMakeName { get; set; }
        string VehicleModel { get; set; }
        string VehicleYear { get; set; }
        string VehicleTransmission { get; set; }
        int VehicleMakeTypeId { get; set; }
        string VehicleMakeTypeName { get; set; }
        VehicleLookupService? LookupService { get; set; }
        bool ManualEntryInd { get; set; }
    }
}
