using AirPro.Service.DTOs.Interface;

namespace AirPro.Site.Areas.Admin.Models.WorkTypes
{
    public class WorkTypeRequestTypeViewModel : IWorkTypeRequestTypeDto
    {
        public int RequestTypeId { get; set; }
        public string RequestTypeName { get; set; }
        public bool SelectedInd { get; set; }
    }
}