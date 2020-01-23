using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Service.DTOs.Concrete
{
    public class OutstandingInvoice : IOutstandingInvoiceDto
    {
        public Guid ShopGuid { get; set; }

        public string ShopName { get; set; }

        public string ShopRO { get; set; }

        public DateTimeOffset RepairCreatedDateTime { get; set; }

        public int InvoiceId { get; set; }

        public DateTimeOffset InvoiceDateTime { get; set; }

        public int InvoiceReportCount { get; set; }

        public decimal InvoiceTotalAmount { get; set; }

        public int PaymentsCount { get; set; }

        public decimal PaymentsTotalAmount { get; set; }

        public decimal InvoiceBalanceAmount { get; set; }

        public bool Selected { get; set; }
    }
}
