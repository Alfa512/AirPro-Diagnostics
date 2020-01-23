using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using AirPro.Common.Enumerations;

namespace AirPro.Entities.Repair
{
    [Table("VehicleLookups", Schema = "Repair")]
    public class VehicleLookupEntityModel
    {
        [Key]
        public int VehicleLookupId { get; set; }
        [Required]
        public string VehicleVIN { get; set; }
        public VehicleLookupService Service { get; set; }
        public string RequestBaseURL { get; set; }
        public string RequestString { get; set; }
        public string RequestMessage { get; set; }
        public bool RequestSuccess { get; set; }
        public HttpStatusCode ResponseStatusCode { get; set; }
        public string ResponseContent { get; set; }
        public DateTimeOffset RequestDt { get; set; } = DateTimeOffset.UtcNow;
    }
}
