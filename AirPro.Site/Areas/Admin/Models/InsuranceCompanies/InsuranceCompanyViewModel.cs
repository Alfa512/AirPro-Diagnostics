using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Site.Areas.Admin.Models.InsuranceCompanies
{
    [Serializable]
    public class InsuranceCompanyViewModel : IInsuranceCompanyDto
    {
        public int InsuranceCompanyId { get; set; }

        [Required, Display(Name = "Company Name")]
        public string InsuranceCompanyName { get; set; }

        [Display(Name = "CCC Insurance Companies")]
        public List<string> CccInsuranceCompanyIds { get; set; }
        public IEnumerable<SelectListItem> CccInsuranceCompanies { get; internal set; }

        [Display(Name = "Program Name")]
        public string ProgramName { get; set; }

        [Display(Name = "Disabled")]
        public bool DisabledInd { get; set; }

        public IUpdateResultDto UpdateResult { get; set; }
    }
}