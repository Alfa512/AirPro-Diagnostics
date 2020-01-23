using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirPro.Entities.Scan
{
    [Table("TroubleCodeRecommendations", Schema = "Scan")]
    public class TroubleCodeRecommendationEntityModel : AuditBaseEntityModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TroubleCodeRecommendationId { get; set; }
        public string TroubleCodeRecommendationText { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed), Index]
        public int TroubleCodeRecommendationHash { get; private set; }

        public bool ActiveInd { get; set; }
    }
}