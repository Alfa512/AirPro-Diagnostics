namespace AirPro.Service.DTOs.Interface
{
    public interface IAirProToolSubscriptionDto
    {
        int ToolSubscriptionId { get; set; }
        int ToolId { get; set; }
        string Vendor { get; set; }
        string Username { get; set; }
        string Password { get; set; }
    }
}