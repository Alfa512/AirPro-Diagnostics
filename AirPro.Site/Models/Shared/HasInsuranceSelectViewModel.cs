using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using AirPro.Site.Attributes;

namespace AirPro.Site.Models.Shared
{
    public class HasInsuranceSelectViewModel
    {
        [Required, Display(Name = "Insurance Company")]
        public int InsuranceCompanyId { get; set; }
        
        [RequiredIf(nameof(InsuranceCompanyId), "1", ErrorMessage = "The Insurance Company Other field is required"), DataType(DataType.MultilineText), Display(Name = "Insurance Company Other")]
        public string InsuranceCompanyOther { get; set; }

        public string InsuranceDataBind { get; set; }
        public string InsuranceOtherDataBind { get; set; }
    }
}