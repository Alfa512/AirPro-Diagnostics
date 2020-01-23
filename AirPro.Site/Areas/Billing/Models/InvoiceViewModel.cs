using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using AirPro.Common.Enumerations;

namespace AirPro.Site.Areas.Billing.Models
{
    public class InvoiceViewModel
    {
        [Display(Name = "Repair ID")]
        public int RepairId { get; set; }

        [Display(Name = "Status")]
        public RepairStatuses RepairStatus { get; set; }
        [Display(Name = "RO #")]
        public string ShopRoNumber { get; set; }
        [Display(Name = "Insurance Co")]
        public string InsuranceCompanyName { get; set; }
        [Display(Name = "Claim #")]
        public string InsuranceClaimNumber { get; set; }

        public DateTime RepairLastUpdatedDt { get; set; }

        [Display(Name = "VIN")]
        public string VehicleVIN { get; set; }
        [Display(Name = "Make")]
        public string VehicleMake { get; set; }
        [Display(Name = "Model")]
        public string VehicleModel { get; set; }
        [Display(Name = "Year")]
        public string VehicleYear { get; set; }
        [Display(Name = "Trans")]
        public string VehicleTransmission { get; set; }

        [Display(Name = "Name")]
        public string ShopName { get; set; }
        [Display(Name = "Phone")]
        public string ShopPhone { get; set; }
        [Display(Name = "Fax")]
        public string ShopFax { get; set; }
        [Display(Name = "Add 1")]
        public string ShopAddress1 { get; set; }
        [Display(Name = "Add 2")]
        public string ShopAddress2 { get; set; }
        [Display(Name = "City")]
        public string ShopCity { get; set; }
        [Display(Name = "State")]
        public string ShopState { get; set; }
        [Display(Name = "Zip")]
        public string ShopZip { get; set; }
        [Display(Name = "Notes")]
        public string ShopNotes { get; set; }

        public int? InvoiceId { get; set; }
        [Display(Name = "Currency")]
        public int InvoiceCurrencyId { get; set; }
        [Display(Name = "Customer Memo"), AllowHtml]
        public string InvoiceCustomerMemo { get; set; }
        [Display(Name = "Complete Invoice")]
        public bool InvoiceCompleteInd { get; set; }
        public string InvoicedByUserName { get; set; }
        public DateTime InvoicedDt { get; set; }
        public List<InvoiceLineItemViewModel> LineItems { get; set; }
    }
}