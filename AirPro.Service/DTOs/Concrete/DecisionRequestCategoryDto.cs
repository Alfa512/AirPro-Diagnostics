using AirPro.Service.DTOs.Interface;

namespace AirPro.Service.DTOs.Concrete
{
    internal class DecisionRequestCategoryDto : IDecisionRequestCategoryDto
    {
        public int RequestCategoryId { get; set; }
        public string RequestCategoryName { get; set; }
        public bool SelectedInd { get; set; }
        public bool PreSelectedInd { get; set; }
    }
}