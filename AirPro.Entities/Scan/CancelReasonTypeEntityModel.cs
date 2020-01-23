using AirPro.Common.Enumerations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirPro.Entities.Scan
{
    [Table("CancelReasonTypes", Schema = "Scan")]
    public class CancelReasonTypeEntityModel
    {
        [Key]
        public int CancelReasonTypeId { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public NotificationTemplate? NotificationTemplate { get; set; }
    }
}
