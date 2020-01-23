using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirPro.Site.Areas.Billing.Models
{
    public class InvoiceWorkItemViewModel
    {
        public string WorkTypeName { get; set; }
        public int WorkTypeId { get; set; }
        public int RequestId { get; set; }
        public string InvoicedAmount { get; set; }
        public int SortOrder { get; set; }
        public bool InvoicedInd { get; set; }

    }
}