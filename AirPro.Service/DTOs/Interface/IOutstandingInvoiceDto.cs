using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirPro.Service.DTOs.Interface
{
    public interface IOutstandingInvoiceDto
    {
        Guid ShopGuid { get; set; }

        string ShopName { get; set; }

        string ShopRO { get; set; }

        DateTimeOffset RepairCreatedDateTime { get; set; }

        int InvoiceId { get; set; }

        DateTimeOffset InvoiceDateTime { get; set; }

        int InvoiceReportCount { get; set; }

        decimal InvoiceTotalAmount { get; set; }

        int PaymentsCount { get; set; }

        decimal PaymentsTotalAmount { get; set; }

        decimal InvoiceBalanceAmount { get; set; }

        bool Selected { get; set; }
    }
}
