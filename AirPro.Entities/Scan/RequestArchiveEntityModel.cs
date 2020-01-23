using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AirPro.Entities.Access;
using AirPro.Entities.Interfaces;

namespace AirPro.Entities.Scan
{
    [Table("RequestsArchive", Schema = "Scan")]
    public class RequestArchiveEntityModel : AuditBaseEntityModel, IRequestEntityModel
    {
        [Key]
        public int RequestArchiveId { get; set; }
        public DateTimeOffset ArchiveDt { get; set; }
        public int RequestId { get; set; }
        [ForeignKey(nameof(RequestId))]
        public virtual RequestEntityModel Request { get; set; }
        public int OrderId { get; set; }
        public string Contact { get; set; }
        public Guid? ContactUserGuid { get; set; }
        [ForeignKey(nameof(ContactUserGuid))]
        public virtual UserEntityModel ContactUser { get; set; }
        public string Notes { get; set; }
        public string OtherWarningInfo { get; set; }
        public string ProblemDescription { get; set; }
        public int? ReportId { get; set; }
        public int? RequestCategoryId { get; set; }
        public int RequestTypeId { get; set; }
        public bool SeatRemovedInd { get; set; }
        public int? ToolId { get; set; }
        public Guid? ShopContactGuid { get; set; }
    }
}