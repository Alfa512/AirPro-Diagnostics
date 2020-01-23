using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirPro.Entities.Service
{
    [Table("CCCEstimates", Schema = "Service")]
    public class CccEstimateEntityModel
    {
        [Key]
        public int EstimateId { get; set; }

        [Index]
        public Guid? RequestGuid { get; set; }

        public int AppId { get; set; }

        public string Trigger { get; set; }

        [Index]
        public Guid? DocumentGuid { get; set; }

        public int? DocumentVersion { get; set; }

        public string DocumentStatus { get; set; }

        [Index, MaxLength(128)]
        public string ShopId { get; set; }

        public string ShopName { get; set; }

        public string ShopRoNumber { get; set; }

        [Index, MaxLength(128)]
        public string VehicleVin { get; set; }

        public string VehicleYear { get; set; }

        public string VehicleMake { get; set; }

        public string VehicleModel { get; set; }

        public string VehicleOdometer { get; set; }

        public bool? VehicleDrivable { get; set; }

        [Index, MaxLength(128)]
        public string InsuranceCompanyId { get; set; }

        public string InsuranceCompanyName { get; set; }

        [Required, Column(TypeName = "XML")]
        public string RawXml { get; set; }

        public DateTimeOffset ReceivedDt { get; set; } = DateTimeOffset.UtcNow;

        [Index]
        public bool ProcessedInd { get; set; }

        public DateTimeOffset ProcessedDt { get; set; }
    }
}
