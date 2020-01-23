using System.ComponentModel.DataAnnotations;
using AirPro.Common.Enumerations;

namespace AirPro.Entities.Repair
{
    public sealed class OrderEntityMetadata
    {
        [Display(Name = "Repair ID")]
        public int OrderId { get; set; }
        public RepairStatuses Status { get; set; } = RepairStatuses.Active;
        [Display(Name = "Shop RO Number")]
        public string ShopReferenceNumber { get; set; }
        [Display(Name = "Insurance Company")]
        public int InsuranceCompanyId { get; set; }
        [Display(Name = "Insurance Company Other")]
        public string InsuranceCompanyOther { get; set; }
        [Display(Name = "Claim Number")]
        public string InsuranceReferenceNumber { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int Odometer { get; set; }
        [Display(Name = "Air Bags Deployed")]
        public bool AirBagsDeployed { get; set; }
        [Display(Name = "Vehicle Drivable")]
        public bool DrivableInd { get; set; }
    }
}
