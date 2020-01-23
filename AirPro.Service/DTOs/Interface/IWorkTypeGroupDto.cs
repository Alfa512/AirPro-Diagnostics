namespace AirPro.Service.DTOs.Interface
{
    public interface IWorkTypeGroupDto
    {
        int WorkTypeGroupId { get; set; }
        string WorkTypeGroupName { get; set; }
        int? WorkTypeGroupSortOrder { get; set; }
        bool WorkTypeGroupActiveInd { get; set; }
        int WorkTypesAssigned { get; set; }
        IUpdateResultDto UpdateResult { get; set; }
    }
}