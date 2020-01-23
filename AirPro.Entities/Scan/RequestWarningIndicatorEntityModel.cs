using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirPro.Entities.Scan
{
    [Table("RequestWarningIndicators", Schema = "Scan")]
    public class RequestWarningIndicatorEntityModel
    {
        [Column(Order = 0), Key, Required]
        public int RequestId { get; set; }
        [ForeignKey(nameof(RequestId))]
        public virtual RequestEntityModel Request { get; set; }

        [Column(Order = 1), Key, Required]
        public int WarningIndicatorId { get; set; }
        [ForeignKey(nameof(WarningIndicatorId))]
        public virtual WarningIndicatorEntityModel WarningIndicator { get; set; }

        public override string ToString()
        {
            return WarningIndicator.Name;
        }
    }
}
