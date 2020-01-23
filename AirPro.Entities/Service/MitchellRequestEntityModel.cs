using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AirPro.Entities.Access;

namespace AirPro.Entities.Service
{
    [Table("MitchellRequests", Schema = "Service")]
    public class MitchellRequestEntityModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RequestId { get; set; }

        public Guid? ShopGuid { get; set; }
        [ForeignKey(nameof(ShopGuid))]
        public ShopEntityModel Shop { get; set; }

        [MaxLength(128), Index]
        public string MitchellRecId { get; set; }

        [MaxLength(128), Index]
        public string VehicleVIN { get; set; }

        [MaxLength(128)]
        public string ShopRONum { get; set; }
        [MaxLength(128)]
        public string InsuranceCoName { get; set; }
        public int? Odometer { get; set; }
        public bool DrivableInd { get; set; }
        public bool AirBagsDeployedInd { get; set; }

        public string RequestBody { get; set; }

        public DateTimeOffset RequestDt { get; set; }
    }
}
