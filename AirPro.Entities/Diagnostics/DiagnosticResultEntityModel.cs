using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AirPro.Entities.Scan;
using Newtonsoft.Json;

namespace AirPro.Entities.Diagnostics
{
    [Table("Results", Schema = "Diagnostic")]
    public class DiagnosticResultEntityModel : AuditBaseEntityModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ResultId { get; set; }

        public int DiagnosticToolId { get; set; }
        [ForeignKey(nameof(DiagnosticToolId))]
        public DiagnosticToolEntityModel DiagnosticTool { get; set; }

        public int? RequestId { get; set; }
        [ForeignKey(nameof(RequestId))]
        public virtual RequestEntityModel Request { get; set; }

        public DateTime? ScanDateTime { get; set; }

        [MaxLength(50)]
        public string CustomerFirstName { get; set; }
        [MaxLength(50)]
        public string CustomerLastName { get; set; }
        [MaxLength(50)]
        public string CustomerRo { get; set; }

        [MaxLength(150)]
        public string ShopName { get; set; }
        [MaxLength(150)]
        public string ShopAddress { get; set; }
        [MaxLength(150)]
        public string ShopEmail { get; set; }
        [MaxLength(50)]
        public string ShopFax { get; set; }
        [MaxLength(50)]
        public string ShopPhone { get; set; }

        [MaxLength(50), Index]
        public string VehicleVin { get; set; }
        [MaxLength(50)]
        public string VehicleMake { get; set; }
        [MaxLength(100)]
        public string VehicleModel { get; set; }
        [MaxLength(10)]
        public string VehicleYear { get; set; }

        [NotMapped]
        public ICollection<string> TestabilityIssuesList
        {
            get => JsonConvert.DeserializeObject<ICollection<string>>(TestabilityIssues);
            set => TestabilityIssues = JsonConvert.SerializeObject(value);
        }
        public string TestabilityIssues { get; set; }

        public bool DeletedInd { get; set; }
    }
}
