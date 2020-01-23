namespace AirPro.Service.DTOs.Interface
{
    public interface IDecisionRequestCategoryDto
    {
        int RequestCategoryId { get; set; }
        string RequestCategoryName { get; set; }
        bool SelectedInd { get; set; }
        bool PreSelectedInd { get; set; }
    }
}