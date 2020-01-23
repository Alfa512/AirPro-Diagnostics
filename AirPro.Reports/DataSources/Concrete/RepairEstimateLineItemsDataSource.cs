using AirPro.Reports.DataSources.Interface;

namespace AirPro.Reports.DataSources.Concrete
{
    public class RepairEstimateLineItemsDataSource : IRepairEstimateLineItemsDataSource
    {
        public string TypeOfScan { get; set; }
        public decimal EstimateAmount { get; set; }
    }
}
