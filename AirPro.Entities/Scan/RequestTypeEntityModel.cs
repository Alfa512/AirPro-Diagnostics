using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirPro.Entities.Scan
{
    [Table("RequestTypes", Schema = "Scan")]
    public class RequestTypeEntityModel : AuditBaseEntityModel
    {
        [Key]
        public int RequestTypeId { get; set; }

        [MaxLength(100)]
        public string TypeName { get; set; }

        [MaxLength(800)]
        public string Instructions { get; set; }

        public bool ActiveFlag { get; set; }

        public bool BillableFlag { get; set; }

        public double DefaultPrice { get; set; }

        public int SortOrder { get; set; }

        [ForeignKey(nameof(RequestCategoryTypeEntityModel.RequestCategoryId))]
        public virtual ICollection<RequestCategoryTypeEntityModel> CategoryTypes { get; set; }

        public virtual ICollection<Access.ShopRequestTypeEntityModel> AllowedShops { get; set; }

        [MaxLength(800)]
        public string InvoiceMemo { get; set; }

        [ForeignKey(nameof(RequestTypeValidationRuleEntityModel.RequestTypeId))]
        public virtual ICollection<RequestTypeValidationRuleEntityModel> ValidationRules { get; set; }

        public override string ToString()
        {
            return TypeName;
        }
    }
}
