using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirPro.Entities.Scan
{
    [Table("RequestCategories", Schema = "Scan")]
    public class RequestCategoryEntityModel
    {
        [Key]
        public int RequestCategoryId { get; set; }
        public string RequestCategoryName { get; set; }
        public int Order { get; set; }
        public bool IsActive { get; set; }
    }
}