namespace AirPro.Service.DTOs.Interface
{
    public interface IValidationRuleDto
    {
        int ValidationRuleId { get; set; }
        string ValidationRuleText { get; set; }
        string ValidationRuleDetails { get; set; }
        int ValidationRuleSortOrder { get; set; }
    }
}