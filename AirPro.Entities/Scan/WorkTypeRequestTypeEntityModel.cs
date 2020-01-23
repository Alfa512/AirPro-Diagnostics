using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirPro.Entities.Scan
{
    [Table("WorkTypeRequestTypes", Schema = "Scan")]
    public class WorkTypeRequestTypeEntityModel
    {
        [Column(Order = 0), Key, Required, ForeignKey(nameof(WorkType))]
        public int WorkTypeId { get; set; }
        public virtual WorkTypeEntityModel WorkType { get; set; }

        [Column(Order = 1), Key, Required, ForeignKey(nameof(RequestType))]
        public int RequestTypeId { get; set; }
        public virtual RequestTypeEntityModel RequestType { get; set; }
    }
}