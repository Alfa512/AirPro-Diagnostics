using AirPro.Entities.Repair;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirPro.Entities.Service
{
    [Table("CCCInsuranceCompanies", Schema = "Service")]
    public class CCCInsuranceCompanyEntityModel
    {
        [Key]
        [MaxLength(128)]
        public string CCCInsuranceCompanyId { get; set; }
        public string CCCInsuranceCompanyName { get; set; }
        public int? RepairInsuranceCompanyId { get; set; }
        [ForeignKey(nameof(RepairInsuranceCompanyId))]
        public virtual InsuranceCompanyEntityModel InsuranceCompany { get; set; }
    }
}
