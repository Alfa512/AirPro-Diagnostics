using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Library.Models.Concrete
{
    public class RequestTypeViewModel : IRequestTypeDto
    {
        [Required, ScaffoldColumn(false)]
        public int RequestTypeId { get; set; }

        [Required, Display(Name = "Type Name")]
        public string RequestTypeName { get; set; }

        [Display(Name = "Category Types")]
        public IEnumerable<int> RequestCategoryIds { get; set; }

        public IEnumerable<int> ValidationRuleIds { get; set; }

        [Display(Name = "Active")]
        public bool ActiveFlag { get; set; }

        [Display(Name = "Billable")]
        public bool BillableFlag { get; set; }

        [Required, Display(Name = "Sort Order")]
        public int SortOrder { get; set; }

        [Display(Name = "Instructions")]
        public string Instructions { get; set; }

        [Display(Name = "Invoice Memo")]
        public string InvoiceMemo { get; set; }

        [Required, Display(Name = "Default Price"), DataType(DataType.Currency)]
        public double DefaultPrice { get; set; }

        public Guid CreatedByUserGuid { get; set; }
        public string CreatedByUserDisplay { get; set; }
        public DateTime CreatedDt { get; set; }

        public Guid? UpdatedByUserGuid { get; set; }
        [Display(Name = "Last Updated By")]
        public string UpdatedByUserDisplay { get; set; }
        [Display(Name = "Last Updated Date"), DisplayFormat(DataFormatString = "{0:MM/dd/yy HH:mm tt}")]
        public DateTime? UpdatedDt { get; set; }

        public IUpdateResultDto UpdateResult { get; set; }
    }
}