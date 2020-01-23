using System;
using System.Collections.Generic;

namespace AirPro.Service.DTOs.Interface
{
    public interface ITroubleCodeRecommendationDto
    {
        int TroubleCodeRecommendationId { get; set; }
        string TroubleCodeRecommendationText { get; set; }
        bool ActiveInd { get; set; }
        Guid CreatedByUserGuid { get; set; }
        string CreatedByUserDisplay { get; set; }
        DateTime CreatedDt { get; set; }
        Guid? UpdatedByUserGuid { get; set; }
        string UpdatedByUserDisplay { get; set; }
        DateTime? UpdatedDt { get; set; }

        IEnumerable<ITroubleCodeRecommendationUsageDto> RecommendationUsage { get; set; }

        IUpdateResultDto UpdateResult { get; set; }
    }
}
