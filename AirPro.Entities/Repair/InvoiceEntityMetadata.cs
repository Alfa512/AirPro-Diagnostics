using System.ComponentModel.DataAnnotations;

namespace AirPro.Entities.Repair
{
    public sealed class InvoiceEntityMetadata
    {
        [Display(Name = "Customer Memo")]
        public string CustomerMemo { get; set; }
    }
}
