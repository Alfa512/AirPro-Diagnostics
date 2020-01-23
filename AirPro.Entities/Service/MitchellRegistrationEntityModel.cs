using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirPro.Entities.Service
{
    [Table("MitchellRegistrations", Schema = "Service")]
    public class MitchellRegistrationEntityModel
    {
        [Key]
        public int MitchellRegistrationId { get; set; }
        public string MitchellAccountId { get; set; }
        public string CallbackUrl { get; set; }
        public string UserEmail { get; set; }
        public DateTimeOffset RequestDt { get; set; }
        public DateTimeOffset CallbackPerformedDt { get; set; }
    }
}