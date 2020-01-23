using AirPro.Entities.Access;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AirPro.Entities.Interfaces;

namespace AirPro.Entities
{
    [MetadataType(typeof(AuditBaseEntityMetadata))]
    public abstract class AuditBaseEntityModel : IAuditBaseEntityModel
    {
        [Required]
        public Guid CreatedByUserGuid { get; set; }
        [ForeignKey(nameof(CreatedByUserGuid))]
        public virtual UserEntityModel CreatedBy { get; set; }

        [Required]
        public DateTimeOffset CreatedDt { get; set; } = DateTimeOffset.UtcNow;

        public Guid? UpdatedByUserGuid { get; set; }
        [ForeignKey(nameof(UpdatedByUserGuid))]
        public virtual UserEntityModel UpdatedBy { get; set; }

        public DateTimeOffset? UpdatedDt { get; set; } = null;
    }

    public sealed class AuditBaseEntityMetadata
    {
        [DisplayFormat(DataFormatString = "{0:MM/dd/yy HH:mm tt}")]
        public DateTimeOffset CreatedDt { get; set; } = DateTimeOffset.UtcNow;
    }
}
