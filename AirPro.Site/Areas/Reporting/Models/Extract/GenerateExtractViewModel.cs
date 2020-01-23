using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AirPro.Site.Areas.Reporting.Models.Extract
{
    public class GenerateExtractViewModel
    {
        [Display(Name = "Account")]
        public Guid? AccountGuid { get; set; }

        [Display(Name = "Shop")]
        public Guid? ShopGuid { get; set; }

        [Display(Name = "Repair Status")]
        public int? RepairStatus { get; set; }

        [Display(Name = "Request Type")]
        public int? RequestType { get; set; }

        [Required, Display(Name = "Date Filter")]
        public string DateFieldFilter { get; set; }

        [Required, Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Required, Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        [Required, Display(Name = "Field List")]
        public IEnumerable<string> FieldList { get; set; }
    }
}