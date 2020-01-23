namespace AirPro.Service.DTOs.Interface
{
    public interface IReportAirProToolSelectionItemDto
    {
        int AirProToolId { get; set; }
        string AirProToolName { get; set; }
        string AirProToolPassword { get; set; }
    }
}