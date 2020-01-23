using System;
using System.Collections.Generic;

namespace AirPro.Service.DTOs.Interface
{
    public interface IRecentPaymentDto
    {
        int PaymentId { get; set; }

        Guid ShopGuid { get; set; }

        string ShopName { get; set; }

        IEnumerable<int> InvoiceIds { get; set; }

        string PaymentTypeName { get; set; }

        decimal PaymentAmount { get; set; }

        string PaymentReferenceNumber { get; set; }

        string PaymentCreatedBy { get; set; }

        DateTime PaymentCreatedDateTime { get; set; }
    }
}
