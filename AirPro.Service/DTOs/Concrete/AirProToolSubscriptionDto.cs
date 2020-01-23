using AirPro.Service.DTOs.Interface;

namespace AirPro.Service.DTOs.Concrete
{
    public class AirProToolSubscriptionDto : IAirProToolSubscriptionDto
    {
        public int ToolSubscriptionId { get; set; }
        public int ToolId { get; set; }
        public string Vendor { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}