using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AirPro.Entities.Repair;

namespace AirPro.Entities.Diagnostics
{
    [Table("VehicleControllers", Schema = "Diagnostic")]
    public class DiagnosticVehicleControllerEntityModel
    {
        [Key, Column(Order=0)]
        public int VehicleMakeId { get; set; }
        [ForeignKey(nameof(VehicleMakeId))]
        public virtual VehicleMakeEntityModel VehicleMake { get; set; }

        [Key, Column(Order = 1)]
        public string VehicleModelName { get; set; }

        [Key, Column(Order = 2)]
        public string VehicleYear { get; set; }

        [Key, Column(Order = 3)]
        public int ControllerId { get; set; }
        [ForeignKey(nameof(ControllerId))]
        public virtual DiagnosticControllerEntityModel Controller { get; set; }

        [Column(TypeName = "DATE")]
        public DateTime LastRecordedDt { get; set; }
    }
}
