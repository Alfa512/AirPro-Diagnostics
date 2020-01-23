using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AirPro.Entities.Repair;

namespace AirPro.Library.Models.Concrete
{
    public class RepairEditViewModel
    {
        [Required, Display(Name = "Shop")]
        public Guid ShopGuid { get; set; }
        public int RepairOrderID { get; set; }
        [Display(Name = "Shop RO Number")]
        public string ShopReferenceNumber { get; set; }
        [Required, Display(Name = "Insurance Company")]
        public InsuranceCompanyEntityModel InsuranceCompany { get; set; }
        [Display(Name = "Insurance Company Other")]
        public string InsuranceCompanyOther { get; set; }
        public int Odometer { get; set; }
        [Display(Name = "Air Bags Deployed")]
        public bool AirBagsDeployed { get; set; }
        public string AirBagsVisualDeployments { get; set; }
        [Display(Name = "Vehicle Drivable")]
        public bool DrivableInd { get; set; }

        [NotMapped]
        [Display(Name ="Points of Impact")]
        public IEnumerable<int> ImpactPoints { get; set; }
    }
}
