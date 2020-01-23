using System.ComponentModel.DataAnnotations;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Site.Areas.Admin.Models.WorkTypes
{
    public class WorkTypeGroupViewModel : IWorkTypeGroupDto
    {
        public int WorkTypeGroupId { get; set; }
        [Required, Display(Name = "Group Name")]
        public string WorkTypeGroupName { get; set; }
        [Display(Name = "Sort Order")]
        public int? WorkTypeGroupSortOrder { get; set; }
        [Display(Name = "Active")]
        public bool WorkTypeGroupActiveInd { get; set; }
        [Display(Name = "Types Assigned")]
        public int WorkTypesAssigned { get; set; }

        public IUpdateResultDto UpdateResult { get; set; }
    }
}