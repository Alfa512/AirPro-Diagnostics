using System;
using AirPro.Reports.DataSources.Interface;

namespace AirPro.Reports.DataSources.Concrete
{
    public class StatementLineItemsDataSource : IStatementLineItemsDataSource
    {
        public int InvoiceId { get; set; }
        public string ShopRONumber { get; set; }
        public decimal InvoiceAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public string RepairCreatedBy { get; set; }
        public string VehicleVIN { get; set; }
        public DateTime InvoiceCreatedDt { get; set; }
    }
}