using System;
using System.Collections.Generic;

namespace AirPro.Library.Models.Concrete
{
    public class RecentPaymentItemViewModel
    {
        public int PaymentId { get; set; }

        public Guid ShopGuid { get; set; }

        public string ShopName { get; set; }

        public IEnumerable<int> InvoiceIds { get; set; }

        public string PaymentTypeName { get; set; }

        public decimal PaymentAmount { get; set; }

        public string PaymentReferenceNumber { get; set; }

        public string PaymentCreatedBy { get; set; }

        public DateTimeOffset PaymentCreatedDateTime { get; set; }
    }
}
