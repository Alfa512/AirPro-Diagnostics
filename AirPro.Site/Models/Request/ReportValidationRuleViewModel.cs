using System;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Site.Models.Request
{
    public class ReportValidationRuleViewModel : IReportValidationRuleDto
    {
        public int ValidationRuleId { get; set; }
        public string ValidationRuleText { get; set; }
        public string ValidationRuleDetails { get; set; }
        public int ValidationRuleSortOrder { get; set; }
        public bool ValidationRuleResultInd { get; set; }
        public bool ResultAcknowledgedInd { get; set; }
        public Guid? ResultAcknowledgedByUserGuid { get; set; }
        public string ResultAcknowledgedByUserDisplay { get; set; }
    }
}