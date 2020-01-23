using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace AirPro.Library.Models.Concrete
{
    public class AcceptPaymentViewModel
    {
        [Display(Name = "Date"), DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}"), Required]
        public DateTime PaymentDate { get; set; } = DateTime.Today;

        [Display(Name = "Type"), Required]
        public int PaymentTypeID { get; set; }

        public IEnumerable<SelectListItem> PaymentTypeSelectListItems { get; set; }

        [Display(Name = "Shop"), Required]
        public Guid PaymentReceivedFromShopGuid { get; set; }

        public IEnumerable<SelectListItem> PaymentReceivedFromShopSelectListItems { get; set; }

        [Display(Name = "Amount"), DisplayFormat(DataFormatString = "{0:c}")]
        public decimal PaymentAmount { get; set; }

        [Display(Name = "Discount")]
        public int PaymentDiscountPercentage { get; set; }

        [Display(Name = "Memo"), DataType(DataType.MultilineText)]
        public string PaymentMemo { get; set; }

        [Display(Name = "Ref No")]
        public string PaymentReferenceNumber { get; set; }

        public IEnumerable<OutstandingInvoiceItemViewModel> ShopOutstandingInvoiceItems { get; set; }

        public IEnumerable<int> ShopInvoicesSelected { get; set; }

        [Display(Name = "Currency")]
        public int CurrencyId { get; set; }
    }
}