using System;
using System.Collections.Generic;
using AirPro.Common.Enumerations;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Service.DTOs.Concrete
{
    internal class DecisionDto : IDecisionDto
    {
        public int DecisionId { get; set; }
        public string DecisionText { get; set; }
        public bool ActiveInd { get; set; }
        public Guid CreatedByUserGuid { get; set; }
        public string CreatedByUserDisplay { get; set; }
        public DateTime CreatedDt { get; set; }
        public Guid? UpdatedByUserGuid { get; set; }
        public string UpdatedByUserDisplay { get; set; }
        public DateTime? UpdatedDt { get; set; }
        public ReportTextSeverity DefaultTextSeverity { get; set; }

        public IEnumerable<IDecisionVehicleMakeDto> VehicleMakes { get; set; }
        public IEnumerable<IDecisionRequestCategoryDto> RequestCategories { get; set; }
        public IEnumerable<IDecisionRequestTypeDto> RequestTypes { get; set; }

        public IUpdateResultDto UpdateResult { get; set; }
    }
}
