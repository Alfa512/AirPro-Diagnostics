namespace AirPro.Reports.DataSources.Interface
{
    public interface IRepairEstimateLineItemsDataSource
    {
        decimal EstimateAmount { get; set; }
        string TypeOfScan { get; set; }
    }
}