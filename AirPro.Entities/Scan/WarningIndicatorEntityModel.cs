using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirPro.Entities.Scan
{
    [Table("WarningIndicators", Schema = "Scan")]
    public class WarningIndicatorEntityModel
    {
        [Key]
        public int WarningIndicatorId { get; set; }

        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
