namespace AirPro.Reports.DataSources.Interface
{
    public interface IRepairInvoiceLineItemsDataSource
    {
        string ComputedWorkPerfomed { get; }
        decimal InvoiceAmount { get; set; }
        string RequestedBy { get; set; }
        string WorkPerfomed { get; set; }
        int? ReportId { get; set; }
        string ComputedInvoiceAmount { get; }
        bool HasSubItems { get; set; }
    }
}