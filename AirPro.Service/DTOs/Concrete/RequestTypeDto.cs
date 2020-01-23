using System;
using System.Collections.Generic;
using System.Linq;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Service.DTOs.Concrete
{
    internal class RequestTypeDto : IRequestTypeDto
    {
        public int RequestTypeId { get; set; }
        public string RequestTypeName { get; set; }

        public IEnumerable<int> RequestCategoryIds { get; set; }
        internal string RequestCategoryIdList
        {
            set => RequestCategoryIds = value.Split(',').Select(int.Parse).ToList();
            get => string.Join(",", RequestCategoryIds ?? new List<int>());
        }

        public IEnumerable<int> ValidationRuleIds { get; set; }
        internal string ValidationRuleIdList
        {
            set => ValidationRuleIds = value.Split(',').Select(int.Parse).ToList();
            get => string.Join(",", ValidationRuleIds ?? new List<int>());
        }

        public bool ActiveFlag { get; set; }
        public bool BillableFlag { get; set; }

        public int SortOrder { get; set; }

        public string Instructions { get; set; }

        public string InvoiceMemo { get; set; }
        public double DefaultPrice { get; set; }

        public Guid CreatedByUserGuid { get; set; }
        public string CreatedByUserDisplay { get; set; }
        public DateTime CreatedDt { get; set; }

        public Guid? UpdatedByUserGuid { get; set; }
        public string UpdatedByUserDisplay { get; set; }
        public DateTime? UpdatedDt { get; set; }

        public IUpdateResultDto UpdateResult { get; set; }
    }
}
