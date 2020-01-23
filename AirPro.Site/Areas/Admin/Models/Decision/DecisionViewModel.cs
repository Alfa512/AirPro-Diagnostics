using System;
using System.Collections.Generic;
using AirPro.Common.Enumerations;
using AirPro.Site.Models.Shared;

namespace AirPro.Site.Areas.Admin.Models.Decision
{
    public class DecisionViewModel
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

        public IEnumerable<DecisionVehicleMakeViewModel> VehicleMakes { get; set; }
        public IEnumerable<DecisionRequestCategoryViewModel> RequestCategories { get; set; }
        public IEnumerable<DecisionRequestTypeViewModel> RequestTypes { get; set; }

        public UpdateResultViewModel UpdateResult { get; set; }
    }
}