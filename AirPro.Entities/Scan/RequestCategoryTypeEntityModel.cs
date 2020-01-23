using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirPro.Entities.Scan
{
    [Table("RequestCategoryTypes", Schema = "Scan")]
    public class RequestCategoryTypeEntityModel
    {
        [Column(Order = 0), Key, Required, ForeignKey(nameof(Category))]
        public int RequestCategoryId { get; set; }
        [Column(Order = 1), Key, Required, ForeignKey(nameof(RequestType))]
        public int RequestTypeId { get; set; }

        public virtual RequestCategoryEntityModel Category { get; set; }
        public virtual RequestTypeEntityModel RequestType { get; set; }
    }
}