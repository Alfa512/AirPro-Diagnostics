using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AirPro.Entities.Access;
using AirPro.Entities.Scan;

namespace AirPro.Entities.Service
{
    [Table("MitchellReports", Schema = "Service")]
    public class MitchellReportEntityModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MitchellReportId { get; set; }

        public int RequestId { get; set; }
        [ForeignKey(nameof(RequestId))]
        public virtual RequestEntityModel Request { get; set; }

        public Guid RequestUserGuid { get; set; }
        [ForeignKey(nameof(RequestUserGuid))]
        public virtual UserEntityModel RequestUser { get; set; }

        public DateTimeOffset RequestDt { get; set; }
    }
}
