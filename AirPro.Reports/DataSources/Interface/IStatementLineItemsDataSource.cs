using System;

namespace AirPro.Reports.DataSources.Interface
{
    public interface IStatementLineItemsDataSource
    {
        decimal DiscountAmount { get; set; }
        decimal InvoiceAmount { get; set; }
        int InvoiceId { get; set; }
        decimal PaidAmount { get; set; }
        string RepairCreatedBy { get; set; }
        string ShopRONumber { get; set; }
        string VehicleVIN { get; set; }
        DateTime InvoiceCreatedDt { get; set; }
    }
}