namespace AirPro.Service.DTOs.Interface
{
    public interface IPricingPlanLineItemDto
    {
        string PlanGroup { get; set; }
        int TypeId { get; set; }
        string TypeGroup { get; set; }
        string TypeName { get; set; }
        decimal DomesticCost { get; set; }
        decimal EuropeanCost { get; set; }
        decimal AsianCost { get; set; }
    }
}