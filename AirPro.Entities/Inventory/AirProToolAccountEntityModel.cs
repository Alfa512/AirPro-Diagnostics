using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AirPro.Entities.Access;

namespace AirPro.Entities.Inventory
{
    [Table("AirProToolAccounts", Schema = "Inventory")]
    public class AirProToolAccountEntityModel : AuditBaseEntityModel
    {
        [Key, Column(Order = 0), ForeignKey(nameof(Tool))]
        public int ToolId { get; set; }
        public virtual AirProToolEntityModel Tool { get; set; }

        [Key, Column(Order = 1), ForeignKey(nameof(Account))]
        public Guid AccountGuid { get; set; }
        public virtual AccountEntityModel Account { get; set; }
    }
}