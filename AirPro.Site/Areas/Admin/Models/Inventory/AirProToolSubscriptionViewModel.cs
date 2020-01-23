
namespace AirPro.Site.Areas.Admin.Models.Inventory
{
    public class AirProToolSubscriptionViewModel
    {
        public int ToolSubscriptionId { get; set; }
        public int ToolId { get; set; }
        public string Vendor { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}