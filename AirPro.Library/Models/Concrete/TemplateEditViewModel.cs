using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using AirPro.Entities.Notifications;
using UniMatrix.Common.Extensions;

namespace AirPro.Library.Models.Concrete
{
    public class TemplateEditViewModel
    {
        public TemplateEditViewModel() { }

        public TemplateEditViewModel(NotificationTemplateEntityModel template, string timezoneInfoId)
        {
            TemplateID = template.NotificationTemplateId;
            Name = template.Name;
            Fields = template.Options;
            EmailBody = template.EmailBody;
            Subject = template.Subject;
            TextMessage = template.TextMessage;

            var updateBy = template.UpdatedBy ?? template.CreatedBy;
            UpdatedBy = $"{updateBy.LastName}, {updateBy.FirstName}";

            var updateDt = template.UpdatedDt ?? template.CreatedDt;
            UpdatedDt = updateDt.ConvertToUserTime(timezoneInfoId);
        }

        [Required, ScaffoldColumn(false)]
        public int TemplateID { get; set; }

        [Display(Name = "Template Name")]
        public string Name { get; set; }

        [Display(Name = "Available Fields")]
        public string Fields { get; set; } // Delimited String

        [Required, Display(Name = "Message Subject")]
        public string Subject { get; set; }

        [Required, AllowHtml, Display(Name = "Message Body"), DataType(DataType.MultilineText)]
        public string EmailBody { get; set; }

        [Display(Name = "Text Message"), DataType(DataType.MultilineText)]
        public string TextMessage { get; set; }

        [Display(Name = "Updated By")]
        public string UpdatedBy { get; set; }

        [Display(Name = "Updated Date"), DisplayFormat(DataFormatString = "{0:MM/dd/yy HH:mm tt}")]
        public DateTime UpdatedDt { get; set; }
    }
}
