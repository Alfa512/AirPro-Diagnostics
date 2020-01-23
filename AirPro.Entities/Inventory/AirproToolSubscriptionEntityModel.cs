using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirPro.Entities.Inventory
{
    [Table("AirProToolSubscriptions", Schema = "Inventory")]
    public class AirProToolSubscriptionEntityModel
    {
        [Column(Order = 0), Key, Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ToolSubscriptionId { get; set; }
        [Column(Order = 1), Key, Required, ForeignKey(nameof(AirProTool))]
        public int ToolId { get; set; }
        public string Vendor { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public virtual AirProToolEntityModel AirProTool { get; set; }
    }
}
