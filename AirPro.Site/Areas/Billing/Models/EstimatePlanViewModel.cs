using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Site.Areas.Billing.Models
{
    public class EstimatePlanViewModel
    {
        public int EstimatePlanId { get; set; }
        [Display(Name = "Name"), Required]
        public string Name { get; set; }
        [Display(Name = "Description"), DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [Display(Name = "Is Active")]
        public bool ActiveInd { get; set; } = true;
        public List<EstimatePlanVehicleViewModel> VehiclePlans { get; set; }
        public IUpdateResultDto UpdateResult { get; set; }
    }
}