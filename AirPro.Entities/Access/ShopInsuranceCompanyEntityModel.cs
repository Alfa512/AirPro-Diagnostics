using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AirPro.Entities.Repair;

namespace AirPro.Entities.Access
{
    [Table("ShopInsuranceCompanies", Schema = "Access")]
    public class ShopInsuranceCompanyEntityModel
    {
        [Column(Order = 0), Key, Required, ForeignKey(nameof(Shop))]
        public Guid ShopId { get; set; }
        [Column(Order = 1), Key, Required, ForeignKey(nameof(InsuranceCompany))]
        public int InsuranceCompanyId { get; set; }

        public virtual ShopEntityModel Shop { get; set; }
        public virtual InsuranceCompanyEntityModel InsuranceCompany { get; set; }
    }
}
