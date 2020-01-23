using System.ComponentModel.DataAnnotations;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Site.Areas.Billing.Models
{
    public class EstimatePlanVehicleViewModel : IEstimatePlanVehicleDto
    {
        public int EstimatePlanId { get; set; }
        public int VehicleMakeId { get; set; }
        [Display(Name = "Vehicle Make")]
        public string VehicleMakeName { get; set; }
        [Display(Name = "Comlpetion Cost")]
        [DataType(DataType.Currency)]
        public decimal CompletionCost { get; set; }
        public IUpdateResultDto UpdateResult { get; set; }
    }
}