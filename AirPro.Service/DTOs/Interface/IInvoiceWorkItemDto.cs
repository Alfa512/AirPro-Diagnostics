using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirPro.Service.DTOs.Interface
{
    public interface IInvoiceWorkItemDto
    {
        string WorkTypeName { get; set; }
        int WorkTypeId { get; set; }
        int RequestId { get; set; }
        decimal InvoicedAmount { get; set; }
        int SortOrder { get; set; }
        bool InvoicedInd { get; set; }
    }
}
