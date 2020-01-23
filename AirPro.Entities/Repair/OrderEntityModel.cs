using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AirPro.Common.Enumerations;
using AirPro.Entities.Access;
using AirPro.Entities.Scan;
using AirPro.Entities.Service;

namespace AirPro.Entities.Repair
{
    [MetadataType(typeof(OrderEntityMetadata))]
    [Table("Orders", Schema = "Repair")]
    // ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
    public class OrderEntityModel : AuditBaseEntityModel
    {
        [Key]
        public int OrderId { get; set; }

        [Required]
        public RepairStatuses Status { get; set; } = RepairStatuses.Active;

        [MaxLength(500)]
        public string ShopReferenceNumber { get; set; }

        [ForeignKey("InsuranceCompany")]
        public int InsuranceCompanyId { get; set; }
        public virtual InsuranceCompanyEntityModel InsuranceCompany { get; set; }
        [MaxLength(200)]
        public string InsuranceCompanyOther { get; set; }
        [MaxLength(200)]
        public string InsuranceReferenceNumber { get; set; }
        public int Odometer { get; set; }
        public bool AirBagsDeployed { get; set; }

        [ForeignKey(nameof(Vehicle))]
        public string VehicleVIN { get; set; }
        public virtual VehicleEntityModel Vehicle { get; set; }

        [Required, Display(Name = "Shop")]
        public Guid ShopGuid { get; set; }
        [ForeignKey(nameof(ShopGuid))]
        public virtual ShopEntityModel Shop { get; set; }

        public virtual ICollection<RequestEntityModel> ScanRequests { get; set; }
        
        [ForeignKey(nameof(OrderPointOfImpactEntityModel.OrderID))]
        public virtual ICollection<OrderPointOfImpactEntityModel> PointsOfImpact { get; set; }

        public virtual InvoiceEntityModel Invoice { get; set; }

        public bool DrivableInd { get; set; } = false;

        [Index(IsUnique = false)]
        public Guid? CCCDocumentGuid { get; set; }

        public int? MitchellRequestId { get; set; }
        [ForeignKey(nameof(MitchellRequestId))]
        public MitchellRequestEntityModel MitchellRequest { get; set; }

        [NotMapped]
        [Display(Name = "Points of Impact")]
        public IEnumerable<int> ImpactPoints { get; set; }
        
        public virtual FeedbackEntityModel Feedback { get; set; }
        [MaxLength(1000)]
        public string AirBagsVisualDeployments { get; set; }
    }
}