using AirPro.Service.DTOs.Interface;

namespace AirPro.Site.Areas.Admin.Models.Decision
{
    public class DecisionRequestTypeViewModel : IDecisionRequestTypeDto
    {
        public int RequestTypeId { get; set; }
        public string RequestTypeName { get; set; }
        public bool SelectedInd { get; set; }
        public bool PreSelectedInd { get; set; }
    }
}