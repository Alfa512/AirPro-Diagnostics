using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AirPro.Common.Enumerations;

namespace AirPro.Entities.Inventory
{
    [Table("AirProToolsArchive", Schema = "Inventory")]
    public class AirProToolArchiveEntityModel : AuditBaseEntityModel, IAirProTool
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ToolArchiveId { get; set; }

        [Index]
        public int ToolId { get; set; }

        public Guid ToolKey { get; set; }

        [MaxLength(20)]
        public string ToolPassword { get; set; }

        [MaxLength(20)]
        public string AutoEnginuityNum { get; set; }
        [MaxLength(20)]
        public string AutoEnginuityVersion { get; set; }
        [MaxLength(20)]
        public string CarDaqNum { get; set; }
        [MaxLength(20)]
        public string DGNum { get; set; }

        [MaxLength(20)]
        public string TeamViewerId { get; set; }
        [MaxLength(20)]
        public string TeamViewerPassword { get; set; }

        [MaxLength(100)]
        public string WindowsVersion { get; set; }
        [MaxLength(100)]
        public string TabletModel { get; set; }
        [MaxLength(200)]
        public string HubModel { get; set; }

        [MaxLength(100)]
        public string OBD2YConnector { get; set; }
        [MaxLength(100)]
        public string AELatestCode { get; set; }
        [MaxLength(100)]
        public string ChargerStyle { get; set; }
        [MaxLength(100)]
        public string TabletSerialNumber { get; set; }
        [MaxLength(100)]
        public string WifiCard { get; set; }
        [MaxLength(100)]
        public string WifiHardwareId { get; set; }
        public DateTimeOffset? WifiDriverDate { get; set; }
        [MaxLength(100)]
        public string WifiDriverVersion { get; set; }
        [MaxLength(100)]
        public string ImageVersion { get; set; }
        [MaxLength(100)]
        public string HondaVersion { get; set; }
        [MaxLength(100)]
        public string FJDSVersion { get; set; }
        [MaxLength(100)]
        public string TechstreamVersion { get; set; }
        public bool CellularActiveInd { get; set; }
        [MaxLength(100)]
        public string CellularProvider { get; set; }
        [MaxLength(100)]
        public string CellularIMEI { get; set; }
        [MaxLength(100)]
        public string WifiMacAddress { get; set; }

        public int? J2534Brand { get; set; }
        public int? J2534Model { get; set; }
        public ToolType Type { get; set; }
        [MaxLength(100)]
        public string J2534Serial { get; set; }

        public bool IPV6DisabledInd { get; set; }
        public bool OneDriveSyncEnabledInd { get; set; }
        public bool UpdatesServiceInd { get; set; }
        public bool MeteredConnectionInd { get; set; }
        public bool SelfScanEnabledInd { get; set; }
    }
}
