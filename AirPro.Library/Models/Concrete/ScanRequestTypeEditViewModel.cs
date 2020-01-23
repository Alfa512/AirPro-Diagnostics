using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using AirPro.Entities.Scan;
using UniMatrix.Common.Extensions;

namespace AirPro.Library.Models.Concrete
{
    public class ScanRequestTypeEditViewModel
    {
        public ScanRequestTypeEditViewModel() { }

        public ScanRequestTypeEditViewModel(RequestTypeEntityModel requestTypeEntity, string timeZoneInfoId)
        {
            if (requestTypeEntity == null) throw new ArgumentNullException(nameof(requestTypeEntity));
            if (timeZoneInfoId == null) throw new ArgumentNullException(nameof(timeZoneInfoId));

            this.RequestTypeID = requestTypeEntity.RequestTypeId;
            this.TypeName = requestTypeEntity.TypeName;
            this.Instructions = requestTypeEntity.Instructions;
            this.ActiveFlag = requestTypeEntity.ActiveFlag;
            this.BillableFlag = requestTypeEntity.BillableFlag;
            this.DefaultPrice = requestTypeEntity.DefaultPrice.ToString("C");
            this.SortOrder = requestTypeEntity.SortOrder;
            this.ReportTemplateHtml = requestTypeEntity.ReportTemplateHtml;
            this.CategoryTypes = requestTypeEntity.CategoryTypes?.Select(d => d.RequestCategoryId).ToArray();
            this.InvoiceMemo = requestTypeEntity.InvoiceMemo;

            if (requestTypeEntity.UpdatedBy != null)
            {
                this.UpdatedBy = $"{requestTypeEntity.UpdatedBy.LastName}, {requestTypeEntity.UpdatedBy.FirstName}";
                this.UpdatedDt = requestTypeEntity?.UpdatedDt.ConvertToUserTime(timeZoneInfoId);
            }
            else
            {
                this.UpdatedBy = $"{requestTypeEntity.CreatedBy.LastName}, {requestTypeEntity.CreatedBy.FirstName}";
                this.UpdatedDt = requestTypeEntity?.CreatedDt.ConvertToUserTime(timeZoneInfoId);
            }
        }

        [Required, ScaffoldColumn(false)]
        public int RequestTypeID { get; set; }

        [Required, Display(Name = "Type Name")]
        public string TypeName { get; set; }

        [Display(Name = "Instructions")]
        public string Instructions { get; set; }

        [Display(Name = "Active")]
        public bool ActiveFlag { get; set; }

        [Display(Name = "Billable")]
        public bool BillableFlag { get; set; }

        [Required, Display(Name = "Default Price"), DataType(DataType.Currency)]
        public string DefaultPrice { get; set; }

        [Required, Display(Name = "Sort Order")]
        public int SortOrder { get; set; }

        [AllowHtml, Display(Name = "Report Template"), DataType(DataType.MultilineText)]
        public string ReportTemplateHtml { get; set; }

        [Display(Name = "Last Updated By")]
        public string UpdatedBy { get; set; }

        [Display(Name = "Last Updated Date"), DisplayFormat(DataFormatString = "{0:MM/dd/yy HH:mm tt}")]
        public DateTime? UpdatedDt { get; set; }

        [Display(Name="Category Types")]
        public IEnumerable<int> CategoryTypes { get; set; }

        [Display(Name="Invoice Memo")]
        public string InvoiceMemo { get; set; }
    }
}