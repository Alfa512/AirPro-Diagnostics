using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Service.DTOs.Concrete
{
    public class InvoiceWorkItemDto : IInvoiceWorkItemDto
    {
        public string WorkTypeName { get; set; }
        public int WorkTypeId { get; set; }
        public int RequestId { get; set; }
        public decimal InvoicedAmount { get; set; }
        public int SortOrder { get; set; }
        public bool InvoicedInd { get; set; }
    }
}
