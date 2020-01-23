using System.Collections.Generic;

namespace AirPro.Service.DTOs.Interface
{
    public interface IInvoiceLineItemDto
    {
        int RequestId { get; set; }
        int ReportId { get; set; }
        bool InvoicedInd { get; set; }
        decimal InvoicedAmount { get; set; }
        int RequestTypeId { get; set; }
        string RequestTypeName { get; set; }
        bool CanceledInd { get; set; }
        string CancellationNotes { get; set; }
        string RequestCreatedByName { get; set; }
        string TechnicianName { get; set; }
        int RequestSortOrder { get; set; }
        string RequestGeneratedMemo { get; set; }
        IEnumerable<IInvoiceWorkItemDto> WorkItems { get; set; }
    }
}