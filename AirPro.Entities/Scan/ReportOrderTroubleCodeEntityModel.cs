using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AirPro.Entities.Diagnostics;
using AirPro.Entities.Repair;

namespace AirPro.Entities.Scan
{
    [Table("ReportOrderTroubleCodes", Schema = "Scan")]
    public class ReportOrderTroubleCodeEntityModel : AuditBaseEntityModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ReportOrderTroubleCodeId { get; set; }

        public int OrderId { get; set; }
        [ForeignKey(nameof(OrderId))]
        public virtual OrderEntityModel Order { get; set; }

        public int? RequestId { get; set; }
        [ForeignKey(nameof(RequestId))]
        public virtual RequestEntityModel Request { get; set; }

        public int ControllerId { get; set; }
        [ForeignKey(nameof(ControllerId))]
        public virtual DiagnosticControllerEntityModel Controller { get; set; }

        public int ControllerIdOrig { get; set; }
        [ForeignKey(nameof(ControllerIdOrig))]
        public virtual DiagnosticControllerEntityModel ControllerOrig { get; set; }

        public int TroubleCodeId { get; set; }
        [ForeignKey(nameof(TroubleCodeId))]
        public virtual DiagnosticTroubleCodeEntityModel TroubleCode { get; set; }

        public int TroubleCodeIdOrig { get; set; }
        [ForeignKey(nameof(TroubleCodeIdOrig))]
        public virtual DiagnosticTroubleCodeEntityModel TroubleCodeOrig { get; set; }
    }
}
