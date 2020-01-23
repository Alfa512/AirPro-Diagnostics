using System.Collections.Generic;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Service.DTOs.Concrete
{
    internal class InvoiceLineItemDto : IInvoiceLineItemDto
    {
        public int RequestId { get; set; }
        public int ReportId { get; set; }
        public bool InvoicedInd { get; set; }
        public decimal InvoicedAmount { get; set; }
        public int RequestTypeId { get; set; }
        public string RequestTypeName { get; set; }
        public bool CanceledInd { get; set; }
        public string CancellationNotes { get; set; }
        public string RequestCreatedByName { get; set; }
        public string TechnicianName { get; set; }
        public int RequestSortOrder { get; set; }
        public string RequestGeneratedMemo { get; set; }
        public IEnumerable<IInvoiceWorkItemDto> WorkItems { get; set; }
    }
}