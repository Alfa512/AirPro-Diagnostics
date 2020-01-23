namespace AirPro.Service.DTOs.Interface
{
    public interface IReportRequestTypeSelectionItemDto
    {
        int RequestCategoryId { get; set; }
        string RequestCategoryName { get; set; }
        int RequestTypeId { get; set; }
        string RequestTypeName { get; set; }
    }
}
