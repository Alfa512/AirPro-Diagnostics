using AirPro.Service.DTOs.Interface;

namespace AirPro.Service.DTOs.Concrete
{
    internal class WorkTypeGroupDto : IWorkTypeGroupDto
    {
        public int WorkTypeGroupId { get; set; }
        public string WorkTypeGroupName { get; set; }
        public int? WorkTypeGroupSortOrder { get; set; }
        public bool WorkTypeGroupActiveInd { get; set; }
        public int WorkTypesAssigned { get; set; }
        public IUpdateResultDto UpdateResult { get; set; }
    }
}