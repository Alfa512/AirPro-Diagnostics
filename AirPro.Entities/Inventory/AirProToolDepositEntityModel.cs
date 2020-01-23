using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirPro.Entities.Inventory
{
    [Table("AirProToolDeposits", Schema = "Inventory")]
    public class AirProToolDepositEntityModel : AuditBaseEntityModel
    {
        [Column(Order = 0), Key, Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ToolDepositId { get; set; }

        [Column(Order = 1), Key, Required, ForeignKey(nameof(AirProTool))]
        public int ToolId { get; set; }

        public DateTimeOffset Date { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public bool DeleteInd { get; set; }

        public virtual AirProToolEntityModel AirProTool { get; set; }
    }
}