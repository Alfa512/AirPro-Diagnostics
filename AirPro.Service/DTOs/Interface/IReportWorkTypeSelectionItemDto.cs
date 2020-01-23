namespace AirPro.Service.DTOs.Interface
{
    public interface IReportWorkTypeSelectionItemDto
    {
        string WorkTypeGroupName { get; set; }
        int WorkTypeId { get; set; }
        string WorkTypeName { get; set; }
        bool WorkTypeSelected { get; set; }
    }
}