using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AirPro.Entities.Access;

namespace AirPro.Entities.Technician
{
    [Table("ProfileTimeOffEntries", Schema = "Technician")]
    public class ProfileTimeOffEntryEntityModel : AuditBaseEntityModel
    {
        [Key]
        public int TimeOffEntryId { get; set; }

        [ForeignKey(nameof(User))]
        public Guid UserGuid { get; set; }

        public DateTimeOffset StartDate { get; set; }

        public DateTimeOffset EndDate { get; set; }

        public string Reason { get; set; }

        public bool DeleteInd { get; set; }

        public virtual UserEntityModel User { get; set; }
    }
}
