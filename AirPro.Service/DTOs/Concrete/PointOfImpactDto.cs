using AirPro.Service.DTOs.Interface;

namespace AirPro.Service.DTOs.Concrete
{
    public class PointOfImpactDto: IPointOfImpactDto
    {
        public int OrderId { get; set; }
        public string Quadrant { get; set; }
    }
}