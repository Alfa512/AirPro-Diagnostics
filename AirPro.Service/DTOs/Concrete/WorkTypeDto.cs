using System.Collections.Generic;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Service.DTOs.Concrete
{
    internal class WorkTypeDto : IWorkTypeDto
    {
        public int WorkTypeGroupId { get; set; }
        public string WorkTypeGroupName { get; set; }
        public int? WorkTypeGroupSortOrder { get; set; }
        public bool WorkTypeGroupActiveInd { get; set; }
        public int WorkTypesAssigned { get; set; }
        public int WorkTypeId { get; set; }
        public string WorkTypeName { get; set; }
        public int? WorkTypeSortOrder { get; set; }
        public string WorkTypeDescription { get; set; }
        public bool WorkTypeActiveInd { get; set; }

        public ICollection<IWorkTypeRequestTypeDto> RequestTypeSelection { get; set; }
        public ICollection<IWorkTypeVehicleMakeDto> VehicleMakeSelection { get; set; }

        public IUpdateResultDto UpdateResult { get; set; }
    }
}