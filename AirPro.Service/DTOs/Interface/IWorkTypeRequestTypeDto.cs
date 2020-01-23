namespace AirPro.Service.DTOs.Interface
{
    public interface IWorkTypeRequestTypeDto
    {
        int RequestTypeId { get; set; }
        string RequestTypeName { get; set; }
        bool SelectedInd { get; set; }
    }
}