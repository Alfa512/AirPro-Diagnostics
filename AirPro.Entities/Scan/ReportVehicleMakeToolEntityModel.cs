using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AirPro.Entities.Repair;

namespace AirPro.Entities.Scan
{
    [Table("ReportVehicleMakeTools", Schema = "Scan")]
    public class ReportVehicleMakeToolEntityModel
    {
        [Column(Order = 0), Key, Required, ForeignKey(nameof(Report))]
        public int ReportId { get; set; }
        public virtual ReportEntityModel Report { get; set; }

        [Column(Order = 1), Key, Required, ForeignKey(nameof(VehicleMakeTool))]
        public int VehicleMakeToolId { get; set; }
        public virtual VehicleMakeToolEntityModel VehicleMakeTool { get; set; }

        public string ToolVersion { get; set; }
    }
}
