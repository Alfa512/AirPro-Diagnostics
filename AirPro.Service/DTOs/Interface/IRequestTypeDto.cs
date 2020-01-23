using System;
using System.Collections.Generic;

namespace AirPro.Service.DTOs.Interface
{
    public interface IRequestTypeDto
    {
        int RequestTypeId { get; set; }
        string RequestTypeName { get; set; }

        IEnumerable<int> RequestCategoryIds { get; set; }
        IEnumerable<int> ValidationRuleIds { get; set; }

        bool ActiveFlag { get; set; }
        bool BillableFlag { get; set; }

        int SortOrder { get; set; }

        string Instructions { get; set; }

        string InvoiceMemo { get; set; }
        double DefaultPrice { get; set; }

        Guid CreatedByUserGuid { get; set; }
        string CreatedByUserDisplay { get; set; }
        DateTime CreatedDt { get; set; }

        Guid? UpdatedByUserGuid { get; set; }
        string UpdatedByUserDisplay { get; set; }
        DateTime? UpdatedDt { get; set; }

        IUpdateResultDto UpdateResult { get; set; }
    }
}
