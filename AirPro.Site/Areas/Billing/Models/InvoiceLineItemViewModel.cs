using System.Collections.Generic;

namespace AirPro.Site.Areas.Billing.Models
{
    public class InvoiceLineItemViewModel
    {
        public int RequestId { get; set; }
        public int ReportId { get; set; }
        public bool InvoicedInd { get; set; }
        public string InvoicedAmount { get; set; }
        public string RequestTypeName { get; set; }
        public bool CanceledInd { get; set; }
        public string CancellationNotes { get; set; }
        public string RequestCreatedByName { get; set; }
        public string TechnicianName { get; set; }
        public List<InvoiceWorkItemViewModel> WorkItems { get; set; } = new List<InvoiceWorkItemViewModel>();
    }
}