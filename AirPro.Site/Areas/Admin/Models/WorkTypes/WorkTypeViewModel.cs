using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Site.Areas.Admin.Models.WorkTypes
{
    [Serializable]
    public class WorkTypeViewModel : IWorkTypeDto
    {
        [Required, Display(Name = "Group")]
        public int WorkTypeGroupId { get; set; }
        public string WorkTypeGroupName { get; set; }
        public int? WorkTypeGroupSortOrder { get; set; }
        public bool WorkTypeGroupActiveInd { get; set; }
        public int WorkTypesAssigned { get; set; }
        public int WorkTypeId { get; set; }
        [Required, Display(Name = "Type Name")]
        public string WorkTypeName { get; set; }
        [Display(Name = "Sort Order")]
        public int? WorkTypeSortOrder { get; set; }
        [Display(Name = "Description"), DataType(DataType.MultilineText)]
        public string WorkTypeDescription { get; set; }
        [Display(Name = "Active")]
        public bool WorkTypeActiveInd { get; set; }

        [Required, Display(Name = "Request Types")]
        public IEnumerable<int> WorkTypeRequestTypeIds { get; set; }
        public IEnumerable<SelectListItem> WorkTypeRequestTypeSelectListItems { get; set; }
        public ICollection<IWorkTypeRequestTypeDto> RequestTypeSelection { get; set; }

        [Required, Display(Name = "Vehicle Makes")]
        public IEnumerable<int> WorkTypeVehicleMakeIds { get; set; }
        public IEnumerable<SelectListItem> WorkTypeVehicleMakeSelectListItems { get; set; }
        public ICollection<IWorkTypeVehicleMakeDto> VehicleMakeSelection { get; set; }

        public IUpdateResultDto UpdateResult { get; set; }
    }
}