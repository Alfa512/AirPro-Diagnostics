using System;

namespace AirPro.Service.DTOs.Interface
{
    public interface IReportValidationRuleDto
    {
        int ValidationRuleId { get; set; }
        string ValidationRuleText { get; set; }
        string ValidationRuleDetails { get; set; }
        int ValidationRuleSortOrder { get; set; }
        bool ValidationRuleResultInd { get; set; }
        bool ResultAcknowledgedInd { get; set; }
        Guid? ResultAcknowledgedByUserGuid { get; set; }
        string ResultAcknowledgedByUserDisplay { get; set; }
    }
}