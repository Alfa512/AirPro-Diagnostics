using System;

namespace AirPro.Reports.DataSources.Interface
{
    public interface IStatementDataSource
    {
        decimal PaymentDiscountAmount { get; set; }
        DateTime PaymentDt { get; set; }
        int PaymentId { get; set; }
        string PaymentMemo { get; set; }
        string PaymentMethodMasked { get; set; }
        string PaymentRefNumber { get; set; }
        string PaymentCurrency { get; set; }
        decimal PaymentTotalAmount { get; set; }
        string PaymentType { get; set; }
        string ShopAddress1 { get; set; }
        string ShopAddress2 { get; set; }
        string ShopCity { get; set; }
        string ShopFax { get; set; }
        string ShopName { get; set; }
        string ShopPhone { get; set; }
        string ShopState { get; set; }
        string ShopZip { get; set; }
    }
}