using AirPro.Entities.Service;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirPro.Entities.Repair
{
    [Table("InsuranceCompanies", Schema = "Repair")]
    public sealed class InsuranceCompanyEntityModel
    {
        [Key, Display(Name = "Insurance Company")]
        public int InsuranceCompanyId { get; set; }
        [Display(Name = "Insurance Company"), MaxLength(200)]
        public string InsuranceCompanyName { get; set; }
        [MaxLength(128)]
        public string ProgramName { get; set; }
        public bool DisabledInd { get; set; }
        [ForeignKey(nameof(CCCInsuranceCompanyEntityModel.RepairInsuranceCompanyId))]
        public ICollection<CCCInsuranceCompanyEntityModel> CCCInsuranceCompanies { get; set; }
    }
}
