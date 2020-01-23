using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AirPro.Entities.Access;
using AirPro.Entities.Diagnostics;
using AirPro.Entities.Interfaces;
using AirPro.Entities.Inventory;
using AirPro.Entities.Repair;

namespace AirPro.Entities.Scan
{
    [Table("Requests", Schema = "Scan")]
    [MetadataType(typeof(RequestEntityMetadata))]
    public class RequestEntityModel : AuditBaseEntityModel, IRequestEntityModel
    {
        [Key, Display(Name = "Request ID")]
        public int RequestId { get; set; }
        [Required]
        public int OrderId { get; set; }
        [ForeignKey(nameof(OrderId))]
        public virtual OrderEntityModel Repair { get; set; }
        [Required]
        public int RequestTypeId { get; set; }
        [ForeignKey(nameof(RequestTypeId))]
        public virtual RequestTypeEntityModel RequestType { get; set; }
        public virtual ICollection<RequestWarningIndicatorEntityModel> WarningIndicators { get; set; }
        public string OtherWarningInfo { get; set; }
        public string ProblemDescription { get; set; } //Symptoms
        public string Notes { get; set; }
        public string Contact { get; set; }
        public Guid? ContactUserGuid { get; set; }
        [ForeignKey(nameof(ContactUserGuid))]
        public virtual UserEntityModel ContactUser { get; set; }
        public Guid? ShopContactGuid { get; set; }
        [ForeignKey(nameof(ShopContactGuid))]
        public virtual ShopContactEntityModel ShopContact { get; set; }
        public bool SeatRemovedInd { get; set; }
        public int? ReportId { get; set; }
        [ForeignKey(nameof(ReportId))]
        public virtual ReportEntityModel Report { get; set; }
        public virtual ICollection<DiagnosticResultEntityModel> ScanUploads { get; set; }

        public int? RequestCategoryId { get; set; }
        [ForeignKey(nameof(RequestCategoryId))]
        public virtual RequestCategoryEntityModel RequestCategory { get; set; }
        
        public int? ToolId { get; set; }
        [ForeignKey(nameof(ToolId))]
        public virtual AirProToolEntityModel AirProTool { get; set; }
    }
}