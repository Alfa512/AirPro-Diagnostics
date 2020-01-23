using System;
using System.Collections.Generic;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Service.DTOs.Concrete
{
    internal class TroubleCodeRecommendationDto : ITroubleCodeRecommendationDto
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
        public IEnumerable<ITroubleCodeRecommendationUsageDto> RecommendationUsage { get; set; }
        public IUpdateResultDto UpdateResult { get; set; }
    }
}
