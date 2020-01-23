using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirPro.Entities.Repair
{
    [Table("OrderStatuses", Schema = "Repair")]
    public class OrderStatusEntityModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StatusId { get; set; }

        [MaxLength(50)]
        public string StatusName { get; set; }
    }
}
