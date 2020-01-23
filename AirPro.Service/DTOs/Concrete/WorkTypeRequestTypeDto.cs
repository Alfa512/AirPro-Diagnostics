using AirPro.Service.DTOs.Interface;

namespace AirPro.Service.DTOs.Concrete
{
    internal class WorkTypeRequestTypeDto : IWorkTypeRequestTypeDto
    {
        public int RequestTypeId { get; set; }
        public string RequestTypeName { get; set; }
        public bool SelectedInd { get; set; }
    }
}