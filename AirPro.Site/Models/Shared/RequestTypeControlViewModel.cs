using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AirPro.Entities.Scan;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Site.Models.Shared
{
    public class RequestTypeControlViewModel
    {
        public IEnumerable<SelectListItem> RequestTypes { get; set; }
        public IEnumerable<KeyValuePair<string, string>> RequestTypeCategories { get; set; }
        [Required, Display(Name = "Request Category")]
        public int? RequestTypeCategoryId { get; set; }
        [Required, Display(Name = "Request Type")]
        public int RequestTypeId { get; set; }
        public List<RequestCategoryTypeEntityModel> RequestCategoryTypes { get; set; }
    }
}