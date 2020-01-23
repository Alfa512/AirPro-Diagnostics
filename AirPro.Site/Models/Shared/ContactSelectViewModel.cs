using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AirPro.Site.Attributes;

namespace AirPro.Site.Models.Shared
{
    public class ContactSelectViewModel
    {
        [Required, Display(Name = "Contact")]
        public string ContactUserGuid { get; set; }

        //[RequiredIf(nameof(ContactUserGuid), "other", ErrorMessage = "The Contact Name/Number field is required"), DataType(DataType.MultilineText), Display(Name = "Contact Name/Number")]
        public string ContactOther { get; set; }

        [RequiredIf(nameof(ContactUserGuid), "other", ErrorMessage = "The Contact First Name field is required"), Display(Name = "Contact First Name")]
        public string ContactOtherFirstName { get; set; }

        [RequiredIf(nameof(ContactUserGuid), "other", ErrorMessage = "The Contact Last Name field is required"), Display(Name = "Contact Last Name")]
        public string ContactOtherLastName { get; set; }

        [RequiredIf(nameof(ContactUserGuid), "other", ErrorMessage = "The Contact Phone field is required"), Display(Name = "Contact Phone")]
        public string ContactOtherPhone { get; set; }

        public SelectList Contacts { get; set; }
        public int LabelBootstrapColumn { get; set; }
    }
}