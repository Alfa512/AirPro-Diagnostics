using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Site.Areas.Admin.Models.VehicleMakes
{
    public class VehicleMakeViewModel : IVehicleMakeDto
    {
        public int VehicleMakeId { get; set; }

        [Required]
        [Display(Name="Vehicle Make Name")]
        public string VehicleMakeName { get; set; }

        [Required]
        [Display(Name = "Vehicle Make Type")]
        public int VehicleMakeTypeId { get; set; }

        public string VehicleMakeTypeName { get; set; }

        [Display(Name = "Program Name")]
        public string ProgramName { get; set; }

        [Display(Name = "Program Instructions"), DataType(DataType.MultilineText)]
        public string ProgramInstructions { get; set; }
        [Display(Name = "Program Tools")]
        public List<IVehicleMakeToolDto> ProgramTools { get; set; }

        public IUpdateResultDto UpdateResult { get; set; }
    }
}