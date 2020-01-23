using System;
using System.Collections.Generic;
using AirPro.Site.Models.Shared;

namespace AirPro.Site.Areas.Admin.Models.Recommendation
{
    public class RecommendationViewModel
    {
        public int TroubleCodeRecommendationId { get; set; }
        public string TroubleCodeRecommendationText { get; set; }
        public bool ActiveInd { get; set; }
        public Guid CreatedByUserGuid { get; set; }
        public string CreatedByUserDisplay { get; set; }
        public DateTime CreatedDt { get; set; }
        public Guid? UpdatedByUserGuid { get; set; }
        public string UpdatedByUserDisplay { get; set; }
        public DateTime? UpdatedDt { get; set; }

        public IEnumerable<RecommendationUsageViewModel> RecommendationUsage { get; set; }

        public UpdateResultViewModel UpdateResult { get; set; }
    }
}