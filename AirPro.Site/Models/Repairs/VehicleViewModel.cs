using System.ComponentModel.DataAnnotations;
using AirPro.Common.Enumerations;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Site.Models.Repairs
{
    public class VehicleViewModel : IVehicleDto
    {
        [Required, Display(Name = "Vehicle VIN")]
        public string VehicleVIN { get; set; }
        [Required, Display(Name = "Vehicle Make")]
        public int VehicleMakeId { get; set; }
        [Display(Name = "Vehicle Make")]
        public string VehicleMakeName { get; set; }
        [Required, Display(Name = "Vehicle Model")]
        public string VehicleModel { get; set; }
        [Required, Display(Name = "Vehicle Year"), MinLength(4)]
        public string VehicleYear { get; set; }
        [Required, Display(Name = "Vehicle Transmission")]
        public string VehicleTransmission { get; set; }
        public int VehicleMakeTypeId { get; set; }
        public string VehicleMakeTypeName { get; set; }
        public VehicleLookupService? LookupService { get; set; }
        public bool ManualEntryInd { get; set; }
    }
}