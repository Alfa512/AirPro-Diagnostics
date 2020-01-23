using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AirPro.Common.Enumerations;
using AirPro.Entities.Diagnostics;

namespace AirPro.Entities.Scan
{
    [Table("ReportTroubleCodeRecommendations", Schema = "Scan")]
    public class ReportTroubleCodeRecommendationEntityModel : AuditBaseEntityModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ReportTroubleCodeRecommendationId { get; set; }

        public long ReportOrderTroubleCodeId { get; set; }
        [ForeignKey(nameof(ReportOrderTroubleCodeId))]
        public virtual ReportOrderTroubleCodeEntityModel ReportOrderTroubleCode { get; set; }

        public int ReportId { get; set; }
        [ForeignKey(nameof(ReportId))]
        public virtual ReportEntityModel Report { get; set; }

        public long? ResultTroubleCodeId { get; set; }
        [ForeignKey(nameof(ResultTroubleCodeId))]
        public virtual DiagnosticResultTroubleCodeEntityModel ResultTroubleCode { get; set; }

        public bool InformCustomerInd { get; set; }
        public bool? AccidentRelatedInd { get; set; }
        public bool ExcludeFromReportInd { get; set; }
        public bool CodeClearedInd { get; set; }

        public string TroubleCodeNoteText { get; set; }

        public int? TroubleCodeRecommendationId { get; set; }
        [ForeignKey(nameof(TroubleCodeRecommendationId))]
        public virtual TroubleCodeRecommendationEntityModel TroubleCodeRecommendation { get; set; }

        public ReportTextSeverity TextSeverity { get; set; }
    }
}