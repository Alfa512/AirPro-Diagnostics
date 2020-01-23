using System.Collections.Generic;

namespace AirPro.Service.DTOs.Interface
{
    public interface IWorkTypeDto : IWorkTypeGroupDto
    {
        int WorkTypeId { get; set; }
        string WorkTypeName { get; set; }
        int? WorkTypeSortOrder { get; set; }
        string WorkTypeDescription { get; set; }
        bool WorkTypeActiveInd { get; set; }

        ICollection<IWorkTypeRequestTypeDto> RequestTypeSelection { get; set; }
        ICollection<IWorkTypeVehicleMakeDto> VehicleMakeSelection { get; set; }

        IUpdateResultDto UpdateResult { get; set; }
    }
}
