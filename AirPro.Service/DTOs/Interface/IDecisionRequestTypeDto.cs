namespace AirPro.Service.DTOs.Interface
{
    public interface IDecisionRequestTypeDto
    {
        int RequestTypeId { get; set; }
        string RequestTypeName { get; set; }
        bool SelectedInd { get; set; }
        bool PreSelectedInd { get; set; }
    }
}