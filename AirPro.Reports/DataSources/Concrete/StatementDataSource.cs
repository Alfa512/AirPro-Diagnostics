using System;
using AirPro.Reports.DataSources.Interface;

namespace AirPro.Reports.DataSources.Concrete
{
    public class StatementDataSource : IStatementDataSource
    {
        // Payment Info.
        public int PaymentId { get; set; }
        public DateTime PaymentDt { get; set; }
        public string PaymentType { get; set; }
        public string PaymentRefNumber { get; set; }
        public string PaymentCurrency { get; set; }
        public decimal PaymentTotalAmount { get; set; }
        public decimal PaymentDiscountAmount { get; set; }
        public string PaymentMethodMasked { get; set; }
        public string PaymentMemo { get; set; }

        // Shop Info.
        public string ShopName { get; set; }
        public string ShopPhone { get; set; }
        public string ShopFax { get; set; }
        public string ShopAddress1 { get; set; }
        public string ShopAddress2 { get; set; }
        public string ShopCity { get; set; }
        public string ShopState { get; set; }
        public string ShopZip { get; set; }
    }
}
