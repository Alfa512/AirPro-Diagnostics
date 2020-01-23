using AirPro.Service.DTOs.Interface;

namespace AirPro.Site.Areas.Admin.Models.Decision
{
    public class DecisionRequestCategoryViewModel : IDecisionRequestCategoryDto
    {
        public int RequestCategoryId { get; set; }
        public string RequestCategoryName { get; set; }
        public bool SelectedInd { get; set; }
        public bool PreSelectedInd { get; set; }
    }
}