using AirPro.Service.DTOs.Interface;

namespace AirPro.Site.Models.Repairs
{
    public class PointOfImpactViewModel : IPointOfImpactDto
    {
        public int OrderId { get; set; }
        public string Quadrant { get; set; }
    }
}