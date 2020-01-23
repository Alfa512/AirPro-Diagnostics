using System;
using System.Collections.Generic;
using AirPro.Common.Enumerations;

namespace AirPro.Service.DTOs.Interface
{
    public interface IDecisionDto
    {
        int DecisionId { get; set; }
        string DecisionText { get; set; }
        bool ActiveInd { get; set; }
        Guid CreatedByUserGuid { get; set; }
        string CreatedByUserDisplay { get; set; }
        DateTime CreatedDt { get; set; }
        Guid? UpdatedByUserGuid { get; set; }
        string UpdatedByUserDisplay { get; set; }
        DateTime? UpdatedDt { get; set; }

        ReportTextSeverity DefaultTextSeverity { get; set; }

        IEnumerable<IDecisionVehicleMakeDto> VehicleMakes { get; set; }
        IEnumerable<IDecisionRequestCategoryDto> RequestCategories { get; set; }
        IEnumerable<IDecisionRequestTypeDto> RequestTypes { get; set; }

        IUpdateResultDto UpdateResult { get; set; }
    }
}