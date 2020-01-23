namespace AirPro.Service.DTOs.Interface
{
    public interface IReportPossibleMissingControllerDto
    {
        int ControllerId { get; set; }
        string ControllerName { get; set; }
    }
}